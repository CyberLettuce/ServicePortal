using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using ServicePortal.Data;

namespace ServicePortal.Services;

public sealed class NotificationService
{
    private readonly IDbContextFactory<ApplicationDbContext> _dbFactory;
    private readonly AuthenticationStateProvider _authenticationStateProvider;

    public NotificationService(
        IDbContextFactory<ApplicationDbContext> dbFactory,
        AuthenticationStateProvider authenticationStateProvider)
    {
        _dbFactory = dbFactory;
        _authenticationStateProvider = authenticationStateProvider;
    }

    public async Task<List<Notification>> GetForCurrentUserAsync()
    {
        var userId = await GetCurrentUserIdAsync();
        if (string.IsNullOrWhiteSpace(userId)) return [];

        await using var db = await _dbFactory.CreateDbContextAsync();
        return await db.Notifications
            .Where(notification => notification.RecipientUserId == userId)
            .OrderBy(notification => notification.IsRead)
            .ThenByDescending(notification => notification.CreatedAt)
            .Take(20)
            .ToListAsync();
    }

    public async Task<int> GetUnreadCountAsync()
    {
        var userId = await GetCurrentUserIdAsync();
        if (string.IsNullOrWhiteSpace(userId)) return 0;

        await using var db = await _dbFactory.CreateDbContextAsync();
        return await db.Notifications.CountAsync(notification =>
            notification.RecipientUserId == userId && !notification.IsRead);
    }

    public async Task CreateTicketAssignmentAsync(string recipientUserId, int ticketNumber, string description)
    {
        if (string.IsNullOrWhiteSpace(recipientUserId)) return;

        await using var db = await _dbFactory.CreateDbContextAsync();
        db.Notifications.Add(new Notification
        {
            RecipientUserId = recipientUserId,
            TicketNumber = ticketNumber,
            Message = $"You have been assigned ticket #{ticketNumber}: {description}",
            CreatedAt = DateTime.UtcNow
        });
        await db.SaveChangesAsync();
    }

    public async Task CreateOnboardingChangeAsync(
        string recipientUserId,
        int onboardingId,
        string customerName,
        string changedBy,
        string changes)
    {
        if (string.IsNullOrWhiteSpace(recipientUserId)) return;

        await using var db = await _dbFactory.CreateDbContextAsync();
        db.Notifications.Add(new Notification
        {
            RecipientUserId = recipientUserId,
            CustomerOnboardingId = onboardingId,
            Message = $"{changedBy} updated {customerName}: {changes}",
            CreatedAt = DateTime.UtcNow
        });
        await db.SaveChangesAsync();
    }

    public async Task CreateUpcomingRenewalNotificationsAsync(IEnumerable<ExistingCustomer> customers)
    {
        var userId = await GetCurrentUserIdAsync();
        if (string.IsNullOrWhiteSpace(userId)) return;

        var today = DateTime.Today;
        var upcoming = customers
            .Where(customer => customer.RenewalDate is not null && customer.RenewalDate.Value.Date >= today && customer.RenewalDate.Value.Date <= today.AddDays(30))
            .ToList();
        if (upcoming.Count == 0) return;

        await using var db = await _dbFactory.CreateDbContextAsync();
        var customerIds = upcoming.Select(customer => customer.Id).ToList();
        var existing = await db.Notifications
            .Where(notification => notification.RecipientUserId == userId && notification.ExistingCustomerId != null && customerIds.Contains(notification.ExistingCustomerId.Value))
            .Select(notification => new { notification.ExistingCustomerId, notification.Message })
            .ToListAsync();
        var existingKeys = existing.Select(notification => $"{notification.ExistingCustomerId}:{notification.Message}").ToHashSet();

        foreach (var customer in upcoming)
        {
            var message = $"Renewal due for {customer.CustomerName} on {customer.RenewalDate!.Value:dd/MM/yyyy}.";
            if (!existingKeys.Add($"{customer.Id}:{message}")) continue;
            db.Notifications.Add(new Notification
            {
                RecipientUserId = userId,
                ExistingCustomerId = customer.Id,
                Message = message,
                CreatedAt = DateTime.UtcNow
            });
        }

        await db.SaveChangesAsync();
    }
    public async Task MarkAsReadAsync(int notificationId)
    {
        var userId = await GetCurrentUserIdAsync();
        await using var db = await _dbFactory.CreateDbContextAsync();
        var notification = await db.Notifications.FirstOrDefaultAsync(item =>
            item.Id == notificationId && item.RecipientUserId == userId);
        if (notification is null) return;

        notification.IsRead = true;
        notification.ReadAt = DateTime.UtcNow;
        await db.SaveChangesAsync();
    }

    public async Task MarkAllAsReadAsync()
    {
        var userId = await GetCurrentUserIdAsync();
        if (string.IsNullOrWhiteSpace(userId)) return;

        await using var db = await _dbFactory.CreateDbContextAsync();
        var unread = await db.Notifications
            .Where(notification => notification.RecipientUserId == userId && !notification.IsRead)
            .ToListAsync();
        foreach (var notification in unread)
        {
            notification.IsRead = true;
            notification.ReadAt = DateTime.UtcNow;
        }
        await db.SaveChangesAsync();
    }

    private async Task<string?> GetCurrentUserIdAsync()
    {
        var state = await _authenticationStateProvider.GetAuthenticationStateAsync();
        return state.User.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}

