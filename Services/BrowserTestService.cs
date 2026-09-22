using Microsoft.EntityFrameworkCore;
using ServicePortal.Data;

namespace ServicePortal.Services;

public sealed class BrowserTestService(IDbContextFactory<ApplicationDbContext> dbFactory)
{
    public async Task<List<BrowserTest>> GetTestsAsync()
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        return await db.BrowserTests.AsNoTracking().Include(test => test.Steps).OrderBy(test => test.Product).ThenBy(test => test.Name).ToListAsync();
    }

    public async Task SaveAsync(BrowserTest test, string createdBy)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var entity = test.Id == 0
            ? new BrowserTest { CreatedBy = createdBy, CreatedAt = DateTime.UtcNow }
            : await db.BrowserTests.Include(item => item.Steps).SingleAsync(item => item.Id == test.Id);
        entity.Product = test.Product.Trim();
        entity.Name = test.Name.Trim();
        entity.StartUrl = test.StartUrl.Trim();
        if (test.Id != 0) db.BrowserTestSteps.RemoveRange(entity.Steps);
        entity.Steps = test.Steps.Where(step => !string.IsNullOrWhiteSpace(step.Target) || step.Action == "Wait")
            .Select((step, index) => new BrowserTestStep { SortOrder = index + 1, Action = step.Action, Target = step.Target.Trim(), Value = step.Value.Trim() }).ToList();
        if (test.Id == 0) db.BrowserTests.Add(entity);
        await db.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var test = await db.BrowserTests.FindAsync(id);
        if (test is null) return;
        db.BrowserTests.Remove(test);
        await db.SaveChangesAsync();
    }
}
