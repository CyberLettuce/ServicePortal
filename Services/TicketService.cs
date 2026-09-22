using ServicePortal.Data;
using ServicePortal.Models;
using Microsoft.EntityFrameworkCore;

namespace ServicePortal.Services;

public sealed class TicketService
{
    private readonly IDbContextFactory<ApplicationDbContext> _dbFactory;
    private List<Ticket> _tickets = new();

    public TicketService(IDbContextFactory<ApplicationDbContext> dbFactory)
    {
        _dbFactory = dbFactory;
    }

    public async Task LoadAsync()
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        _tickets = await db.Tickets
            .Include(ticket => ticket.EmailMessages)
            .OrderByDescending(ticket => ticket.CreatedDate)
            .ToListAsync();
    }

    public List<Ticket> GetTickets() => _tickets;

    public async Task<List<TicketTemplate>> GetTemplatesAsync()
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        return await db.TicketTemplates.AsNoTracking().OrderBy(template => template.Name).ToListAsync();
    }

    public async Task SaveTemplateAsync(TicketTemplate template)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        var stored = template.Id == 0 ? new TicketTemplate() : await db.TicketTemplates.FindAsync(template.Id) ?? new TicketTemplate();
        stored.Name = template.Name.Trim();
        stored.Description = template.AdditionalDetails.Trim();
        stored.MainIssue = template.MainIssue.Trim();
        stored.AdditionalDetails = template.AdditionalDetails.Trim();
        stored.ReferencesLrn = template.ReferencesLrn.Trim();
        stored.MainContact = template.MainContact.Trim();
        stored.ActionsTaken = template.ActionsTaken.Trim();
        stored.SoftwareModule = template.SoftwareModule.Trim();
        stored.Mrn = template.Mrn.Trim(); stored.Arc = template.Arc.Trim(); stored.Tsn = template.Tsn.Trim(); stored.ShipmentReference = template.ShipmentReference.Trim();
        stored.Priority = template.Priority;
        stored.Category = template.Category;
        stored.AssignedTo = template.AssignedTo;
        stored.UpdatedAt = DateTime.UtcNow;
        if (stored.Id == 0) db.TicketTemplates.Add(stored);
        await db.SaveChangesAsync();
    }

    public async Task DeleteTemplateAsync(int id)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        var template = await db.TicketTemplates.FindAsync(id);
        if (template is null) return;
        db.TicketTemplates.Remove(template);
        await db.SaveChangesAsync();
    }

    public async Task UpdateTicketStatusesAsync(IEnumerable<int> ticketNumbers, string status)
    {
        var ids = ticketNumbers.Distinct().ToList();
        if (ids.Count == 0 || string.IsNullOrWhiteSpace(status)) return;
        await using var db = await _dbFactory.CreateDbContextAsync();
        await db.Tickets.Where(ticket => ids.Contains(ticket.TicketNumber))
            .ExecuteUpdateAsync(setters => setters.SetProperty(ticket => ticket.Status, status));
        await LoadAsync();
    }

    public async Task<int> AddTicketAsync(Ticket ticket)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        ticket.TicketNumber =
            (await db.Tickets.MaxAsync(item => (int?)item.TicketNumber) ?? 0) + 1;
        ticket.TicketReference = await CreateTicketReferenceAsync(db, ticket.Category);
        db.Tickets.Add(ticket);
        await db.SaveChangesAsync();
        await AddAssignmentNotificationAsync(db, ticket, null);
        await db.SaveChangesAsync();
        _tickets.Add(ticket);
        return ticket.TicketNumber;
    }

    public async Task<List<string>> GetAssignableUserDisplayNamesAsync()
    {
        await using var db = await _dbFactory.CreateDbContextAsync();

        var users = await db.Users
            .Where(user => !string.IsNullOrWhiteSpace(user.DisplayName))
            .OrderBy(user => user.DisplayName)
            .Select(user => user.DisplayName)
            .ToListAsync();

        users.Insert(0, "Unassigned");
        return users.Distinct(StringComparer.OrdinalIgnoreCase).ToList();
    }

    public async Task UpdateTicketAsync(Ticket ticket)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();

        var stored = await db.Tickets.FindAsync(ticket.TicketNumber);
        if (stored is null)
        {
            return;
        }

        var previousAssignee = stored.AssignedTo;
        stored.Description = string.IsNullOrWhiteSpace(ticket.AdditionalDetails) ? ticket.Description : ticket.AdditionalDetails;
        stored.Priority = ticket.Priority;
        stored.Status = ticket.Status;
        stored.Notes = ticket.Notes;
        stored.Category = ticket.Category;
        stored.ChamberOfCommerce = ticket.ChamberOfCommerce;
        stored.AssignedTo = ticket.AssignedTo;
        stored.ProjectId = ticket.ProjectId;
        stored.ExistingCustomerId = ticket.ExistingCustomerId;
        stored.MainIssue = ticket.MainIssue;
        stored.AdditionalDetails = ticket.AdditionalDetails;
        stored.ReferencesLrn = ticket.ReferencesLrn;
        stored.MainContact = ticket.MainContact;
        stored.ActionsTaken = ticket.ActionsTaken;
        stored.SoftwareModule = ticket.SoftwareModule;
        stored.Mrn = ticket.Mrn; stored.Arc = ticket.Arc; stored.Tsn = ticket.Tsn; stored.ShipmentReference = ticket.ShipmentReference;

        await AddAssignmentNotificationAsync(db, stored, previousAssignee);
        await db.SaveChangesAsync();
    }

    public async Task<int> CreateEmailTicketAsync(
        string requesterName,
        string requesterEmail,
        string subject,
        string body,
        string graphMessageId)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        var ticket = new Ticket
        {
            TicketNumber =
                (await db.Tickets.MaxAsync(item => (int?)item.TicketNumber) ?? 0) + 1,
            TicketReference = await CreateTicketReferenceAsync(db, "General"),
            Description = body,
            EmailSubject = subject,
            RequesterName = requesterName,
            RequesterEmail = requesterEmail,
            Priority = "Medium",
            Status = "Open",
            Category = "General",
            AssignedTo = "Unassigned",
            CreatedDate = DateTime.UtcNow
        };

        db.Tickets.Add(ticket);
        await db.SaveChangesAsync();
        db.TicketEmailMessages.Add(new TicketEmailMessage
        {
            TicketNumber = ticket.TicketNumber,
            Direction = "Incoming",
            From = requesterEmail,
            To = "Support",
            Subject = subject,
            Body = body,
            GraphMessageId = graphMessageId
        });
        await db.SaveChangesAsync();
        _tickets.Add(ticket);
        return ticket.TicketNumber;
    }

    public async Task AddEmailReplyAsync(int ticketNumber, string sender, string body)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        var ticket = await db.Tickets.FindAsync(ticketNumber);
        if (ticket is null) return;

        db.TicketEmailMessages.Add(new TicketEmailMessage
        {
            TicketNumber = ticketNumber,
            Direction = "Outgoing",
            From = sender,
            To = ticket.RequesterEmail,
            Subject = $"Re: [SGS-{ticketNumber}] {ticket.EmailSubject}",
            Body = body,
            DeliveryPending = true
        });
        await db.SaveChangesAsync();
    }

    public async Task CloseTicketAsync(int ticketNumber)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        var ticket = await db.Tickets.FindAsync(ticketNumber);
        if (ticket is null) return;
        ticket.Status = "Closed";
        await db.SaveChangesAsync();
    }

    public async Task DeleteTicketAsync(int ticketNumber)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        var ticket = await db.Tickets.FindAsync(ticketNumber);
        if (ticket is null) return;
        db.Tickets.Remove(ticket);
        await db.SaveChangesAsync();
        _tickets.RemoveAll(item => item.TicketNumber == ticketNumber);
    }

    public async Task SaveTicketsAsync()
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        foreach (var ticket in _tickets)
        {
            db.Tickets.Update(ticket);
        }
        await db.SaveChangesAsync();
    }

    private static async Task AddAssignmentNotificationAsync(
        ApplicationDbContext db,
        Ticket ticket,
        string? previousAssignee)
    {
        if (string.Equals(previousAssignee, ticket.AssignedTo, StringComparison.OrdinalIgnoreCase) ||
            string.IsNullOrWhiteSpace(ticket.AssignedTo) ||
            string.Equals(ticket.AssignedTo, "Unassigned", StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        var recipient = await db.Users.FirstOrDefaultAsync(user =>
            user.DisplayName == ticket.AssignedTo);

        if (recipient is null)
        {
            return;
        }

        db.Notifications.Add(new Notification
        {
            RecipientUserId = recipient.Id,
            TicketNumber = ticket.TicketNumber,
            Message = $"You have been assigned ticket #{ticket.TicketNumber}: {ticket.Description}",
            CreatedAt = DateTime.UtcNow
        });
    }

    private static async Task<string> CreateTicketReferenceAsync(ApplicationDbContext db, string category)
    {
        var prefix = category switch
        {
            "eCERT" => "eCERT",
            "eGTA" => "eGTA",
            "UKCS Portal" or "ePortal" => "UKCS",
            "eCONSIGN" => "eCON",
            _ => "GEN"
        };

        var count = await db.Tickets.CountAsync(ticket => ticket.TicketReference.StartsWith(prefix + "-"));
        return $"{prefix}-{count + 1:0000}";
    }
}
