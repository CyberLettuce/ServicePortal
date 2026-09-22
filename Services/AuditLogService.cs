using ServicePortal.Data;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;

namespace ServicePortal.Services;

public sealed class AuditLogService
{
    private readonly ApplicationDbContext _db;
    private readonly AuthenticationStateProvider _authenticationStateProvider;
    private readonly UserManager<ApplicationUser> _userManager;

    public AuditLogService(
        ApplicationDbContext db,
        AuthenticationStateProvider authenticationStateProvider,
        UserManager<ApplicationUser> userManager)
    {
        _db = db;
        _authenticationStateProvider = authenticationStateProvider;
        _userManager = userManager;
    }

    public async Task RecordAsync(
        string action,
        string? targetUserId,
        string? targetEmail,
        string details)
    {
        var authState =
            await _authenticationStateProvider.GetAuthenticationStateAsync();

        var actor =
            await _userManager.GetUserAsync(authState.User);

        _db.AuditLogs.Add(new AuditLog
        {
            CreatedAt = DateTime.UtcNow,
            ActorUserId = actor?.Id,
            ActorEmail =
                actor?.Email ??
                authState.User.Identity?.Name ??
                "Unknown",
            Action = action,
            TargetUserId = targetUserId,
            TargetEmail = targetEmail ?? string.Empty,
            Details = details
        });

        await _db.SaveChangesAsync();
    }
}
