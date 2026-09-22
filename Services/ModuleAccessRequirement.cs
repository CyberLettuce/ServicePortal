using System.Security.Claims;
using ServicePortal.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ServicePortal.Services;

public sealed class ModuleAccessRequirement : IAuthorizationRequirement
{
    public ModuleAccessRequirement(string moduleKey)
    {
        ModuleKey = moduleKey;
    }

    public string ModuleKey { get; }
}

public sealed class ModuleAccessHandler
    : AuthorizationHandler<ModuleAccessRequirement>
{
    private readonly IDbContextFactory<ApplicationDbContext> _dbFactory;

    public ModuleAccessHandler(
        IDbContextFactory<ApplicationDbContext> dbFactory)
    {
        _dbFactory = dbFactory;
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        ModuleAccessRequirement requirement)
    {
        var userId =
            context.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(userId))
        {
            return;
        }

        await using var db =
            await _dbFactory.CreateDbContextAsync();

        var hasAccess = await db.UserModules.AnyAsync(assignment =>
            assignment.UserId == userId &&
            assignment.ModuleKey == requirement.ModuleKey);

        if (hasAccess)
        {
            context.Succeed(requirement);
        }
    }
}
