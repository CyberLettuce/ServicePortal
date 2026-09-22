using Microsoft.EntityFrameworkCore;
using ServicePortal.Data;
using ServicePortal.Models;

namespace ServicePortal.Services;

public sealed class ExistingCustomerService(IDbContextFactory<ApplicationDbContext> dbFactory)
{
    public async Task<List<ExistingCustomer>> GetAllAsync()
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        return await db.ExistingCustomers.AsNoTracking().OrderBy(customer => customer.CustomerName).ToListAsync();
    }

    public async Task<ExistingCustomer?> GetAsync(int id)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        return await db.ExistingCustomers.FindAsync(id);
    }

    public async Task SaveAsync(ExistingCustomer customer)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var stored = customer.Id == 0 ? new ExistingCustomer { CreatedAt = DateTime.UtcNow } : await db.ExistingCustomers.FindAsync(customer.Id) ?? new ExistingCustomer { CreatedAt = DateTime.UtcNow };
        stored.CustomerName = customer.CustomerName.Trim(); stored.ContactName = customer.ContactName.Trim(); stored.ContactEmail = customer.ContactEmail.Trim(); stored.ProductsPurchased = customer.ProductsPurchased.Trim(); stored.SoftwareDetails = customer.SoftwareDetails.Trim(); stored.ImplementationOwner = customer.ImplementationOwner.Trim(); stored.ClientNumber = customer.ClientNumber.Trim(); stored.ChamberOfCommerce = customer.ChamberOfCommerce.Trim(); stored.Documents = customer.Documents.Trim(); stored.LanguagesRequired = customer.LanguagesRequired.Trim(); stored.ProjectNumber = customer.ProjectNumber.Trim(); stored.SalesManager = customer.SalesManager.Trim(); stored.GoLiveDate = customer.GoLiveDate; stored.RenewalDate = customer.RenewalDate; stored.VocCompleted = customer.VocCompleted; stored.Feedback = customer.Feedback.Trim(); stored.VocContactName = customer.VocContactName.Trim(); stored.VocCompletedDate = customer.VocCompletedDate; stored.Status = customer.Status; stored.UpdatedAt = DateTime.UtcNow;
        if (stored.Id == 0) db.ExistingCustomers.Add(stored);
        await db.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var customer = await db.ExistingCustomers.FindAsync(id); if (customer is null) return;
        db.ExistingCustomers.Remove(customer); await db.SaveChangesAsync();
    }

    public async Task<List<Ticket>> GetTicketsAsync(int customerId)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        return await db.Tickets.AsNoTracking().Where(ticket => ticket.ExistingCustomerId == customerId).OrderByDescending(ticket => ticket.CreatedDate).ToListAsync();
    }

    public async Task UpsertFromOnboardingAsync(CustomerOnboarding onboarding)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var customer = await db.ExistingCustomers.FirstOrDefaultAsync(item => item.ClientNumber == onboarding.ClientNumber && item.CustomerName == onboarding.CustomerName)
            ?? new ExistingCustomer { CreatedAt = DateTime.UtcNow };
        customer.CustomerName = onboarding.CustomerName; customer.ContactName = onboarding.ContactName; customer.ContactEmail = onboarding.ContactEmail; customer.ProductsPurchased = onboarding.ProductsPurchased; customer.ImplementationOwner = onboarding.ImplementationOwner; customer.ClientNumber = onboarding.ClientNumber; customer.ChamberOfCommerce = onboarding.ChamberOfCommerce; customer.LanguagesRequired = onboarding.LanguagesRequired; customer.ProjectNumber = onboarding.ProjectNumber; customer.SalesManager = onboarding.SalesManager; customer.GoLiveDate = onboarding.TargetGoLiveDate; customer.Status = "Active"; customer.UpdatedAt = DateTime.UtcNow;
        if (customer.Id == 0) db.ExistingCustomers.Add(customer);
        await db.SaveChangesAsync();
    }
}




