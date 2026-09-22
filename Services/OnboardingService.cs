using ServicePortal.Data;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ServicePortal.Services;

public sealed class OnboardingService
{

    public static List<string> GetRequiredFieldErrors(CustomerOnboarding onboarding)
    {
        var errors = new List<string>();
        var products = OnboardingProductCatalog.Parse(onboarding.ProductsPurchased).Products;
        if (string.IsNullOrWhiteSpace(onboarding.CustomerName)) errors.Add("Company Name is required.");
        if (string.IsNullOrWhiteSpace(onboarding.ContactName)) errors.Add("Company Contact is required.");
        if (string.IsNullOrWhiteSpace(onboarding.ContactEmail)) errors.Add("Contact Email is required.");
        if (products.Count == 0) errors.Add("Software / Package Purchased is required.");

        if (products.Contains("eGTA"))
        {
            if (string.IsNullOrWhiteSpace(onboarding.EgtaSubscription)) errors.Add("eGTA Subscription is required.");
            if (string.IsNullOrWhiteSpace(onboarding.SystemBuildStatus)) errors.Add("System Build is required.");
            if (!onboarding.OrderDateAccepted.HasValue) errors.Add("Order Date Accepted is required.");
            if (!onboarding.SubscriptionStartDate.HasValue) errors.Add("Subscription Start Date is required.");
            if (string.IsNullOrWhiteSpace(onboarding.SalesOwner)) errors.Add("Sales Owner is required.");
        }
        if (products.Contains("TransitNet"))
        {
            if (!onboarding.SignedDocumentsReceivedDate.HasValue) errors.Add("Signed Documents Received is required.");
            if (string.IsNullOrWhiteSpace(onboarding.TNetSubscription)) errors.Add("TNet Subscription is required.");
            if (string.IsNullOrWhiteSpace(onboarding.TrainingGivenStatus)) errors.Add("Training Given is required.");
            if (string.IsNullOrWhiteSpace(onboarding.SalesOwner)) errors.Add("Sales Owner is required.");
        }
        return errors;
    }

    private readonly IDbContextFactory<ApplicationDbContext> _dbFactory;
    private readonly AuditLogService _auditLogService;
    private readonly NotificationService _notificationService;
    private readonly AuthenticationStateProvider _authenticationStateProvider;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ExistingCustomerService _existingCustomerService;

    public OnboardingService(
        IDbContextFactory<ApplicationDbContext> dbFactory,
        AuditLogService auditLogService,
        NotificationService notificationService,
        AuthenticationStateProvider authenticationStateProvider,
        UserManager<ApplicationUser> userManager, ExistingCustomerService existingCustomerService)
    {
        _dbFactory = dbFactory;
        _auditLogService = auditLogService;
        _notificationService = notificationService;
        _authenticationStateProvider = authenticationStateProvider;
        _userManager = userManager;
        _existingCustomerService = existingCustomerService;
    }

    public async Task<List<CustomerOnboarding>> GetAllAsync()
    {
        await using var db = await _dbFactory.CreateDbContextAsync();

        var onboardings = await db.CustomerOnboardings
            .Include(onboarding => onboarding.ChecklistItems)
            .OrderBy(onboarding => onboarding.Status == "Complete")
            .ThenBy(onboarding => onboarding.TargetGoLiveDate)
            .ThenBy(onboarding => onboarding.CustomerName)
            .ToListAsync();

        var referencesChanged = PopulateMissingReferences(onboardings);
        var checklistsChanged = ReplacePreviousChecklists(db, onboardings);
        if (referencesChanged || checklistsChanged)
        {
            await db.SaveChangesAsync();
        }

        return onboardings;
    }

    public async Task<List<string>> GetImplementationOwnersAsync()
    {
        await using var db = await _dbFactory.CreateDbContextAsync();

        return await db.Users
            .AsNoTracking()
            .Select(user => user.DisplayName)
            .Where(displayName => !string.IsNullOrWhiteSpace(displayName))
            .Select(displayName => displayName!)
            .Distinct()
            .OrderBy(displayName => displayName)
            .ToListAsync();
    }

    public async Task<CustomerOnboarding?> GetAsync(int id)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();

        var onboarding = await db.CustomerOnboardings
            .Include(onboarding => onboarding.ChecklistItems
                .OrderBy(item => item.SortOrder))
            .SingleOrDefaultAsync(onboarding => onboarding.Id == id);

        if (onboarding is not null &&
            (string.IsNullOrWhiteSpace(onboarding.OnboardingReference) ||
             ReplacePreviousChecklist(db, onboarding)))
        {
            if (string.IsNullOrWhiteSpace(onboarding.OnboardingReference))
                onboarding.OnboardingReference = BuildReference(onboarding);
            await db.SaveChangesAsync();
        }

        return onboarding;
    }

    public async Task<int> CreateAsync(CustomerOnboarding onboarding)
    {
        var now = DateTime.UtcNow;
        onboarding.CreatedAt = now;
        onboarding.UpdatedAt = now;
        onboarding.Status = "Not Started";
        onboarding.ChecklistItems = BuildChecklist(onboarding)
            .Select((title, index) => new OnboardingChecklistItem
            {
                Title = title,
                SortOrder = index + 1
            })
            .ToList();

        await using var db = await _dbFactory.CreateDbContextAsync();
        db.CustomerOnboardings.Add(onboarding);
        await db.SaveChangesAsync();

        if (string.Equals(onboarding.Status, "Complete", StringComparison.OrdinalIgnoreCase))
            await _existingCustomerService.UpsertFromOnboardingAsync(onboarding);

        onboarding.OnboardingReference = BuildReference(onboarding);
        await db.SaveChangesAsync();

        await _auditLogService.RecordAsync(
            "Onboarding created",
            onboarding.Id.ToString(),
            onboarding.CustomerName,
            $"Created onboarding for {onboarding.ProductsPurchased}; " +
            $"target go-live {onboarding.TargetGoLiveDate:dd MMM yyyy}.");

        return onboarding.Id;
    }

    public async Task<bool> UpdateAsync(CustomerOnboarding updated)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        var onboarding = await db.CustomerOnboardings.FindAsync(updated.Id);

        if (onboarding is null)
        {
            return false;
        }

        var previousStatus = onboarding.Status;
        var changes = DescribeChanges(onboarding, updated);
        onboarding.CustomerName = updated.CustomerName.Trim();
        onboarding.ContactName = updated.ContactName.Trim();
        onboarding.ContactEmail = updated.ContactEmail.Trim();
        onboarding.ProductsPurchased = updated.ProductsPurchased.Trim();
        onboarding.ImplementationOwner = updated.ImplementationOwner.Trim();
        onboarding.ClientNumber = updated.ClientNumber.Trim();
        onboarding.EgtaSubscription = updated.EgtaSubscription.Trim();
        onboarding.AxosoftId = updated.AxosoftId.Trim();
        onboarding.OrderDateAccepted = updated.OrderDateAccepted;
        onboarding.PaymentReceivedDate = updated.PaymentReceivedDate;
        onboarding.SubscriptionInvoiceNumber = updated.SubscriptionInvoiceNumber.Trim();
        onboarding.SystemBuildStatus = updated.SystemBuildStatus.Trim();
        onboarding.SendToBrokerage = updated.SendToBrokerage.Trim();
        onboarding.TrainingStatus = updated.TrainingStatus.Trim();
        onboarding.SalesOwner = updated.SalesOwner.Trim();
        onboarding.SoftwareOwners = updated.SoftwareOwners.Trim();
        onboarding.FinanceNotified = updated.FinanceNotified || updated.FinanceNotifiedDate.HasValue;
        onboarding.FinanceNotifiedDate = updated.FinanceNotifiedDate;
        onboarding.BackOfficeNotified = updated.BackOfficeNotified || updated.BackOfficeNotifiedDate.HasValue;
        onboarding.BackOfficeNotifiedDate = updated.BackOfficeNotifiedDate;
        onboarding.ChamberContact = updated.ChamberContact.Trim();
        onboarding.ChamberOfCommerce = updated.ChamberOfCommerce.Trim();
        onboarding.DDConfirmed = updated.DDConfirmed || updated.DDConfirmedDate.HasValue;
        onboarding.DDConfirmedDate = updated.DDConfirmedDate;
        onboarding.LanguagesRequired = updated.LanguagesRequired.Trim();
        onboarding.OnboardingQuestionnaireReceived =
            updated.OnboardingQuestionnaireReceived ||
            updated.OnboardingQuestionnaireReceivedDate.HasValue;
        onboarding.OnboardingQuestionnaireReceivedDate = updated.OnboardingQuestionnaireReceivedDate;
        onboarding.Paid = updated.Paid;
        onboarding.PracticeSiteAccess = updated.PracticeSiteAccess;
        onboarding.PricingSetupWithIT = updated.PricingSetupWithIT;
        onboarding.ProjectNumber = updated.ProjectNumber.Trim();
        onboarding.SharePointUrl = updated.SharePointUrl.Trim();
        onboarding.SignedDocumentsReceived = updated.SignedDocumentsReceived || updated.SignedDocumentsReceivedDate.HasValue;
        onboarding.SignedDocumentsReceivedDate = updated.SignedDocumentsReceivedDate;
        onboarding.OrderDate = updated.OrderDate;
        onboarding.AdminToolAndPriceGroupDate = updated.AdminToolAndPriceGroupDate;
        onboarding.PricingSetupDate = updated.PricingSetupDate;
        onboarding.SubscriptionStartDate = updated.SubscriptionStartDate;
        onboarding.SystemBuild = updated.SystemBuild.Trim();
        onboarding.SystemBuildDate = updated.SystemBuildDate;
        onboarding.TNetSubscription = updated.TNetSubscription.Trim();
        onboarding.TrainingGiven = updated.TrainingGiven;
        onboarding.TrainingGivenStatus = updated.TrainingGivenStatus.Trim();
        onboarding.TrainingRequired = updated.TrainingRequired;
        onboarding.WelcomeEmailSent = updated.WelcomeEmailSent;
        onboarding.WelcomeEmailSentDate = updated.WelcomeEmailSentDate;
        onboarding.PracticeSiteAccessDetails = updated.PracticeSiteAccessDetails.Trim();
        onboarding.SalesManager = updated.SalesManager.Trim();
        onboarding.StartDate = updated.StartDate;
        onboarding.TargetGoLiveDate = updated.TargetGoLiveDate;
        onboarding.Status = updated.Status == "Blocked" ? "In Progress" : updated.Status;
        onboarding.Notes = updated.Notes.Trim();
        onboarding.UpdatedAt = DateTime.UtcNow;

        var existingChecklist = await db.OnboardingChecklistItems
            .Where(item => item.CustomerOnboardingId == onboarding.Id)
            .ToListAsync();
        ReplacePreviousChecklist(db, onboarding, existingChecklist);

        await db.SaveChangesAsync();

        if (string.Equals(onboarding.Status, "Complete", StringComparison.OrdinalIgnoreCase))
            await _existingCustomerService.UpsertFromOnboardingAsync(onboarding);

        if (changes.Count > 0 &&
            !string.IsNullOrWhiteSpace(onboarding.ImplementationOwner))
        {
            var recipient = await db.Users.FirstOrDefaultAsync(user =>
                user.DisplayName == onboarding.ImplementationOwner);
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var actor = await _userManager.GetUserAsync(authState.User);

            if (recipient is not null && recipient.Id != actor?.Id)
            {
                var changedBy = actor?.DisplayName ?? actor?.Email ?? "A user";
                await _notificationService.CreateOnboardingChangeAsync(
                    recipient.Id,
                    onboarding.Id,
                    onboarding.CustomerName,
                    changedBy,
                    string.Join(", ", changes));
            }
        }

        await _auditLogService.RecordAsync(
            "Onboarding updated",
            onboarding.Id.ToString(),
            onboarding.CustomerName,
            $"Status: {previousStatus} to {onboarding.Status}. " +
            $"Target go-live: {onboarding.TargetGoLiveDate:dd MMM yyyy}.");

        return true;
    }

    public async Task<bool> UpdateImageAsync(int onboardingId, string fileName, string contentType, byte[] content)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        var onboarding = await db.CustomerOnboardings.FindAsync(onboardingId);
        if (onboarding is null) return false;

        onboarding.ImageFileName = fileName;
        onboarding.ImageContent = content;
        onboarding.ImageContentType = contentType;
        onboarding.UpdatedAt = DateTime.UtcNow;
        await db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
        if (!authState.User.IsInRole("Administrator")) return false;
        await using var db = await _dbFactory.CreateDbContextAsync();
        var onboarding = await db.CustomerOnboardings.FindAsync(id);
        if (onboarding is null) return false;
        var customerName = onboarding.CustomerName;
        db.CustomerOnboardings.Remove(onboarding);
        await db.SaveChangesAsync();
        await _auditLogService.RecordAsync("Onboarding deleted", id.ToString(), customerName, "Customer onboarding record deleted.");
        return true;
    }

    private static List<string> DescribeChanges(
        CustomerOnboarding current,
        CustomerOnboarding updated)
    {
        var changes = new List<string>();
        AddChange(changes, "customer name", current.CustomerName, updated.CustomerName);
        AddChange(changes, "contact email", current.ContactEmail, updated.ContactEmail);
        AddChange(changes, "software", current.ProductsPurchased, updated.ProductsPurchased);
        AddChange(changes, "implementation owner", current.ImplementationOwner, updated.ImplementationOwner);
        AddChange(changes, "status", current.Status, updated.Status);
        AddChange(changes, "planned go-live", current.TargetGoLiveDate, updated.TargetGoLiveDate);
        AddChange(changes, "start date", current.StartDate, updated.StartDate);
        AddChange(changes, "client number", current.ClientNumber, updated.ClientNumber);
        AddChange(changes, "TNet subscription", current.TNetSubscription, updated.TNetSubscription);
        AddChange(changes, "signed documents", current.SignedDocumentsReceivedDate, updated.SignedDocumentsReceivedDate);
        AddChange(changes, "order date", current.OrderDate, updated.OrderDate);
        AddChange(changes, "finance notification", current.FinanceNotifiedDate, updated.FinanceNotifiedDate);
        AddChange(changes, "pricing setup", current.PricingSetupDate, updated.PricingSetupDate);
        AddChange(changes, "system build", current.SystemBuildDate, updated.SystemBuildDate);
        AddChange(changes, "training", current.TrainingGivenStatus, updated.TrainingGivenStatus);
        AddChange(changes, "sales manager", current.SalesManager, updated.SalesManager);
        AddChange(changes, "notes", current.Notes, updated.Notes);
        return changes.Take(5).ToList();
    }

    private static void AddChange<T>(List<string> changes, string label, T before, T after)
    {
        if (!EqualityComparer<T>.Default.Equals(before, after)) changes.Add(label);
    }

    private static IReadOnlyList<string> BuildChecklist(CustomerOnboarding onboarding)
    {
        var products = OnboardingProductCatalog.Parse(onboarding.ProductsPurchased).Products;
        var hasTransitNet = products.Contains("TransitNet");
        var hasSoftware = products.Any(product => product is "eCERT" or "eGTA" or "Brokerage Portal");
        var items = new List<string>
        {
            "Signed contract received",
            "Axosoft log created",
            "System build requested"
        };

        if (hasSoftware) items.Add("System built");
        if (hasTransitNet) items.Add("Admin Tool and Price Group setup");
        if (hasSoftware && onboarding.TrainingRequired) items.Add("Training delivered");
        if (hasTransitNet) items.Add("Inform BO");
        if (hasSoftware) items.Add("Go-live date");
        if (hasTransitNet)
        {
            items.Add("Account activated and training scheduled");
            items.Add("Training delivered");
            items.Add("Go-live date");
        }

        return items;
    }

    private static bool ReplacePreviousChecklists(
        ApplicationDbContext db,
        IEnumerable<CustomerOnboarding> onboardings)
    {
        var changed = false;
        foreach (var onboarding in onboardings)
            changed |= ReplacePreviousChecklist(db, onboarding, onboarding.ChecklistItems);
        return changed;
    }

    private static bool ReplacePreviousChecklist(
        ApplicationDbContext db,
        CustomerOnboarding onboarding,
        IEnumerable<OnboardingChecklistItem>? existingItems = null)
    {
        var existing = (existingItems ?? onboarding.ChecklistItems)
            .OrderBy(item => item.SortOrder)
            .ToList();
        var expected = BuildChecklist(onboarding);

        if (existing.Select(item => item.Title).SequenceEqual(expected)) return false;

        db.OnboardingChecklistItems.RemoveRange(existing);
        var replacements = expected.Select((title, index) => new OnboardingChecklistItem
        {
            CustomerOnboardingId = onboarding.Id,
            Title = title,
            SortOrder = index + 1
        }).ToList();
        db.OnboardingChecklistItems.AddRange(replacements);
        onboarding.ChecklistItems = replacements;
        return true;
    }

    private static bool PopulateMissingReferences(
        IEnumerable<CustomerOnboarding> onboardings)
    {
        var changed = false;

        foreach (var onboarding in onboardings.Where(item =>
                     string.IsNullOrWhiteSpace(item.OnboardingReference)))
        {
            onboarding.OnboardingReference = BuildReference(onboarding);
            changed = true;
        }

        return changed;
    }

    private static string BuildReference(CustomerOnboarding onboarding)
    {
        var selection = OnboardingProductCatalog.Parse(
            onboarding.ProductsPurchased);

        var productCode = selection.Products.Contains("eCERT")
            ? "eCERT"
            : selection.Products.Contains("eGTA")
                ? "eGTA"
                : selection.Products.Contains("TransitNet")
                    ? "TransitNet"
                    : selection.Products.Contains("Brokerage Portal")
                        ? "BrokeragePortal"
                        : "ONB";

        var ownerInitials = string.Concat(
            onboarding.ImplementationOwner
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(part => char.ToUpperInvariant(part[0])));

        if (string.IsNullOrWhiteSpace(ownerInitials))
        {
            ownerInitials = "NA";
        }

        return $"{productCode}-{ownerInitials}-{onboarding.Id:0000}";
    }

    public async Task<bool> SetChecklistItemAsync(int itemId, bool isComplete)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        var item = await db.OnboardingChecklistItems
            .Include(checklistItem => checklistItem.CustomerOnboarding)
            .SingleOrDefaultAsync(checklistItem => checklistItem.Id == itemId);

        if (item?.CustomerOnboarding is null)
        {
            return false;
        }

        item.IsComplete = isComplete;
        item.CompletedAt = isComplete ? DateTime.UtcNow : null;

        var onboarding = item.CustomerOnboarding;
        var allItems = await db.OnboardingChecklistItems
            .Where(checklistItem =>
                checklistItem.CustomerOnboardingId == onboarding.Id)
            .ToListAsync();

        if (allItems.All(checklistItem => checklistItem.IsComplete))
        {
            onboarding.Status = "Complete";
        }
        else if (onboarding.Status is "Not Started" or "Complete")
        {
            onboarding.Status = allItems.Any(checklistItem => checklistItem.IsComplete)
                ? "In Progress"
                : "Not Started";
        }

        onboarding.UpdatedAt = DateTime.UtcNow;
        await db.SaveChangesAsync();

        await _auditLogService.RecordAsync(
            isComplete ? "Onboarding task completed" : "Onboarding task reopened",
            onboarding.Id.ToString(),
            onboarding.CustomerName,
            item.Title);

        return true;
    }
}
