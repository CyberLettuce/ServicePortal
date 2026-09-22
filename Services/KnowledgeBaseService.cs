using Microsoft.EntityFrameworkCore;
using ServicePortal.Data;

namespace ServicePortal.Services;

public sealed class KnowledgeBaseService(IDbContextFactory<ApplicationDbContext> dbFactory, AxosoftImportService importService)
{
    private static readonly (string Software, string Subcategory)[] DefaultSoftwareOptions = [("eCERT", "Arab Certificate of Origin"), ("eCERT", "ATA Carnet"), ("eCERT", "EUR1/ EUR-MED"), ("eCERT", "International Import Certificate"), ("eCERT", "UK/EU Certificate of Origin"), ("eCERT", "Chamber Site"), ("eCERT", "Exporter Site"), ("eGTA", "CDS Imports"), ("eGTA", "CDS Exports"), ("eGTA", "EMCS"), ("eGTA", "ENS (S&SGB)"), ("eGTA", "GVMS"), ("eGTA", "ICS2"), ("eGTA", "Shipments"), ("eCONSIGN", ""), ("UKCS Portal", "")];
    private static readonly string[] DefaultCategories = ["Getting Started", "Accounts & Passwords", "Software Applications", "Troubleshooting", "How-To Guides", "Frequently Asked Questions"];
    public async Task EnsureDefaultsAsync()
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        if (await db.KnowledgeBaseCategories.AnyAsync()) return;
        db.KnowledgeBaseCategories.AddRange(DefaultCategories.Select((name, index) => new KnowledgeBaseCategory { Name = name, SortOrder = index }));
        await db.SaveChangesAsync();
    }
    public async Task<List<KnowledgeBaseSoftwareOption>> GetSoftwareOptionsAsync()
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        if (!await db.KnowledgeBaseSoftwareOptions.AnyAsync()) { db.KnowledgeBaseSoftwareOptions.AddRange(DefaultSoftwareOptions.Select((option, index) => new KnowledgeBaseSoftwareOption { Software = option.Software, Subcategory = option.Subcategory, SortOrder = index })); await db.SaveChangesAsync(); }
        return await db.KnowledgeBaseSoftwareOptions.AsNoTracking().OrderBy(item => item.SortOrder).ThenBy(item => item.Software).ThenBy(item => item.Subcategory).ToListAsync();
    }
    public async Task SaveSoftwareOptionAsync(KnowledgeBaseSoftwareOption option)
    { await using var db = await dbFactory.CreateDbContextAsync(); var entity = option.Id == 0 ? new KnowledgeBaseSoftwareOption() : await db.KnowledgeBaseSoftwareOptions.FindAsync(option.Id) ?? new KnowledgeBaseSoftwareOption(); entity.Software = option.Software.Trim(); entity.Subcategory = option.Subcategory.Trim(); entity.SortOrder = option.SortOrder; entity.ImageContent = option.ImageContent; entity.ImageContentType = option.ImageContentType; if (option.Id == 0) db.KnowledgeBaseSoftwareOptions.Add(entity); await db.SaveChangesAsync(); }
    public async Task DeleteSoftwareOptionAsync(int id) { await using var db = await dbFactory.CreateDbContextAsync(); var item = await db.KnowledgeBaseSoftwareOptions.FindAsync(id); if (item is null) return; db.KnowledgeBaseSoftwareOptions.Remove(item); await db.SaveChangesAsync(); }
    public async Task SaveSoftwareImageAsync(string software, byte[] content, string contentType)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var options = await db.KnowledgeBaseSoftwareOptions.Where(item => item.Software == software).ToListAsync();
        foreach (var option in options) { option.ImageContent = content; option.ImageContentType = contentType; }
        await db.SaveChangesAsync();
    }
    public async Task<List<KnowledgeBaseCategory>> GetCategoriesAsync() { await EnsureDefaultsAsync(); await using var db = await dbFactory.CreateDbContextAsync(); return await db.KnowledgeBaseCategories.AsNoTracking().OrderBy(item => item.SortOrder).ThenBy(item => item.Name).ToListAsync(); }
    public async Task<List<KnowledgeArticle>> GetArticlesAsync(bool includeUnpublished = false)
    { await using var db = await dbFactory.CreateDbContextAsync(); IQueryable<KnowledgeArticle> query = db.KnowledgeBaseArticles.Include(item => item.Category).AsNoTracking(); if (!includeUnpublished) query = query.Where(item => item.IsPublished && !item.IsArchived); return await query.OrderBy(item => item.Title).ToListAsync(); }
    public async Task<KnowledgeArticle?> GetArticleAsync(int id, bool countView = true)
    { await using var db = await dbFactory.CreateDbContextAsync(); var article = await db.KnowledgeBaseArticles.Include(item => item.Category).SingleOrDefaultAsync(item => item.Id == id); if (article is not null && countView) { article.ViewCount++; await db.SaveChangesAsync(); } return article; }
    public async Task SaveArticleAsync(KnowledgeArticle article)
    { await using var db = await dbFactory.CreateDbContextAsync(); var entity = article.Id == 0 ? new KnowledgeArticle() : await db.KnowledgeBaseArticles.FindAsync(article.Id) ?? new KnowledgeArticle(); entity.CategoryId = article.CategoryId; entity.Title = article.Title.Trim(); entity.Summary = article.Summary.Trim(); entity.Symptoms = article.Symptoms.Trim(); entity.Resolution = article.Resolution.Trim(); entity.AdditionalInformation = article.AdditionalInformation.Trim(); entity.Tags = article.Tags.Trim(); entity.Software = article.Software.Trim(); entity.Subcategory = article.Subcategory.Trim(); entity.IsPublished = article.IsPublished; entity.IsArchived = article.IsArchived; entity.UpdatedAt = DateTime.UtcNow; if (article.Id == 0) db.KnowledgeBaseArticles.Add(entity); await db.SaveChangesAsync(); }
    public async Task RateAsync(int id, bool useful) { await using var db = await dbFactory.CreateDbContextAsync(); var article = await db.KnowledgeBaseArticles.FindAsync(id); if (article is null) return; if (useful) article.UsefulCount++; else article.NotUsefulCount++; await db.SaveChangesAsync(); }
    public async Task<List<KnowledgeArticle>> SearchAsync(string term) { var articles = await GetArticlesAsync(); return string.IsNullOrWhiteSpace(term) ? articles : articles.Where(item => ($"{item.Title} {item.Summary} {item.Symptoms} {item.Resolution} {item.Tags}").Contains(term, StringComparison.OrdinalIgnoreCase)).ToList(); }
    public async Task<int> ImportLogAsync(string fileName, byte[] content)
    {
        var entries = importService.Parse(fileName, content);
        await using var db = await dbFactory.CreateDbContextAsync();
        var category = await db.KnowledgeBaseCategories.SingleOrDefaultAsync(item => item.Name == "Imported articles");
        if (category is null) { category = new KnowledgeBaseCategory { Name = "Imported articles", SortOrder = 999 }; db.KnowledgeBaseCategories.Add(category); await db.SaveChangesAsync(); }
        var existingTitles = await db.KnowledgeBaseArticles.Select(item => item.Title).ToListAsync();
        var imported = entries.Where(entry => !existingTitles.Contains(entry.Title, StringComparer.OrdinalIgnoreCase)).Select(entry => new KnowledgeArticle
        {
            CategoryId = category.Id, Title = entry.Title, Summary = string.IsNullOrWhiteSpace(entry.Notes) ? "Imported support article." : entry.Notes[..Math.Min(500, entry.Notes.Length)],
            Symptoms = entry.ReleaseNotes, Resolution = string.IsNullOrWhiteSpace(entry.TestNotes) ? entry.Notes : entry.TestNotes,
            AdditionalInformation = $"Imported reference: {entry.Id}", Tags = "Imported", Software = string.Empty, Subcategory = string.Empty, IsPublished = true, UpdatedAt = DateTime.UtcNow
        }).ToList();
        if (imported.Count > 0) { db.KnowledgeBaseArticles.AddRange(imported); await db.SaveChangesAsync(); }
        return imported.Count;
    }
    public async Task DeleteArticleAsync(int id)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var article = await db.KnowledgeBaseArticles.FindAsync(id);
        if (article is null) return;
        db.KnowledgeBaseArticles.Remove(article);
        await db.SaveChangesAsync();
    }
}
