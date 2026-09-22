using ServicePortal.Data;
using Microsoft.EntityFrameworkCore;
using System.IO.Compression;
using System.Xml.Linq;

namespace ServicePortal.Services;

public sealed class TestKitDocumentService
{
    private readonly IDbContextFactory<ApplicationDbContext> dbFactory;
    private readonly AuditLogService auditLog;
    public TestKitDocumentService(IDbContextFactory<ApplicationDbContext> dbFactory, AuditLogService auditLog) { this.dbFactory = dbFactory; this.auditLog = auditLog; }
    public async Task<List<TestKitDocument>> GetAllAsync()
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        return await db.TestKitDocuments.AsNoTracking().OrderByDescending(item => item.UploadedAt)
            .Select(item => new TestKitDocument { Id = item.Id, OriginalFileName = item.OriginalFileName, ContentType = item.ContentType, SizeBytes = item.SizeBytes, UploadedBy = item.UploadedBy, UploadedAt = item.UploadedAt, ImportedCaseCount = item.ImportedCaseCount }).ToListAsync();
    }
    public async Task<int> AddAndImportAsync(string name, string contentType, long sizeBytes, byte[] content, string uploadedBy)
    {
        var cases = ExtractCases(content);
        await using var db = await dbFactory.CreateDbContextAsync();
        var existingReferences = await db.TestPlans.Where(plan => plan.Product == "eGTA" && plan.Scope == "CDS").Select(plan => plan.Reference).ToHashSetAsync();
        var plans = cases.Where(item => !existingReferences.Contains(item.Reference)).Select(item => new TestPlan
        {
            Product = "eGTA", Scope = "CDS", Reference = item.Reference, TestCaseName = item.Name,
            Instructions = item.Instructions, ExpectedResult = item.ExpectedResult, Frequency = "Monthly", IsActive = true, CreatedAt = DateTime.UtcNow
        }).ToList();
        var document = new TestKitDocument { OriginalFileName = name, ContentType = contentType, SizeBytes = sizeBytes, Content = content, UploadedBy = uploadedBy, UploadedAt = DateTime.UtcNow, ImportedCaseCount = plans.Count };
        db.TestKitDocuments.Add(document); await db.SaveChangesAsync();
        if (plans.Count > 0) { db.TestPlans.AddRange(plans); await db.SaveChangesAsync(); }
        await auditLog.RecordAsync("Test kit uploaded", document.Id.ToString(), name, "Uploaded by " + uploadedBy + "; " + plans.Count + " test case(s) imported.");
        return plans.Count;
    }

    private static List<ImportedCase> ExtractCases(byte[] content)
    {
        using var memory = new MemoryStream(content);
        using var archive = new ZipArchive(memory, ZipArchiveMode.Read);
        var entry = archive.GetEntry("word/document.xml") ?? throw new InvalidOperationException("The uploaded Word document does not contain readable document data.");
        using var stream = entry.Open();
        var document = XDocument.Load(stream);
        XNamespace word = "http://schemas.openxmlformats.org/wordprocessingml/2006/main";
        var cases = new List<ImportedCase>();
        foreach (var table in document.Descendants(word + "tbl"))
        {
            var rows = table.Elements(word + "tr").Select(row => row.Elements(word + "tc").Select(cell => string.Concat(cell.Descendants(word + "t").Select(text => text.Value)).Trim()).ToList()).Where(row => row.Count > 1).ToList();
            if (rows.Count < 2) continue;
            var headerCells = rows[0].Select(cell => cell.ToLowerInvariant()).ToList();
            var header = string.Join(" ", headerCells);
            if (header.Contains("password") || header.Contains("username") || header.Contains("eori")) continue;
            var isTestTable = header.Contains("shipment") || header.Contains("test") || header.Contains("xml name");
            if (!isTestTable) continue;
            var testIndex = headerCells.FindIndex(cell => cell.Contains("test"));
            var descriptionIndex = headerCells.FindIndex(cell => cell.Contains("description"));
            var resultIndex = headerCells.FindIndex(cell => cell.Contains("result") || cell.Contains("returned"));
            foreach (var row in rows.Skip(1))
            {
                var reference = row.ElementAtOrDefault(0)?.Trim() ?? string.Empty;
                var instructions = row.ElementAtOrDefault(testIndex >= 0 ? testIndex : Math.Min(1, row.Count - 1))?.Trim() ?? string.Empty;
                var expected = row.ElementAtOrDefault(resultIndex >= 0 ? resultIndex : Math.Min(2, row.Count - 1))?.Trim() ?? string.Empty;
                if (string.IsNullOrWhiteSpace(reference) || reference.StartsWith("#") || reference.Length > 100) continue;
                var name = row.ElementAtOrDefault(descriptionIndex >= 0 ? descriptionIndex : testIndex >= 0 ? testIndex : 0)?.Trim() ?? reference;
                cases.Add(new ImportedCase(Trim(reference, 100), Trim(name, 300), Trim(instructions, 3000), Trim(expected, 3000)));
            }
        }
        return cases.GroupBy(item => item.Reference, StringComparer.OrdinalIgnoreCase).Select(group => group.First()).ToList();
    }
    private static string Trim(string value, int length) => value.Length <= length ? value : value[..length];
    private sealed record ImportedCase(string Reference, string Name, string Instructions, string ExpectedResult);
}
