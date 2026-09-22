using ServicePortal.Data;
using Microsoft.EntityFrameworkCore;

namespace ServicePortal.Services;

public sealed class ModuleAccessService
{
    private readonly IDbContextFactory<ApplicationDbContext> _dbFactory;

    public ModuleAccessService(
        IDbContextFactory<ApplicationDbContext> dbFactory)
    {
        _dbFactory = dbFactory;
    }

    public async Task<HashSet<string>> GetUserIdsAsync(string moduleKey)
    {
        await using var db =
            await _dbFactory.CreateDbContextAsync();

        return await db.UserModules
            .AsNoTracking()
            .Where(assignment => assignment.ModuleKey == moduleKey)
            .Select(assignment => assignment.UserId)
            .ToHashSetAsync();
    }

    public async Task SetAccessAsync(
        string userId,
        string moduleKey,
        bool enabled)
    {
        await using var db =
            await _dbFactory.CreateDbContextAsync();

        var assignment = await db.UserModules.FindAsync(
            userId,
            moduleKey);

        if (enabled && assignment is null)
        {
            db.UserModules.Add(new UserModule
            {
                UserId = userId,
                ModuleKey = moduleKey
            });
        }
        else if (!enabled && assignment is not null)
        {
            db.UserModules.Remove(assignment);
        }
        else
        {
            return;
        }

        await db.SaveChangesAsync();
    }
}
