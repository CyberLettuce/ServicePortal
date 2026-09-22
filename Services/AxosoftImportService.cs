using System.IO.Compression;
using System.Xml.Linq;

namespace ServicePortal.Services;

public sealed record AxosoftItem(
    string Id,
    string Title,
    string Release,
    string Notes,
    string ReleaseNotes,
    string TestNotes);

public sealed class AxosoftImportService
{
    public List<AxosoftItem> Parse(string fileName, byte[] content)
    {
        var rows = Path.GetExtension(fileName).ToLowerInvariant() switch
        {
            ".csv" => ParseCsv(System.Text.Encoding.UTF8.GetString(content)),
            ".xlsx" => ParseWorkbook(content),
            _ => throw new InvalidOperationException("Choose a CSV or Excel export.")
        };
        if (rows.Count < 2) return [];
        var headers = rows[0].Select(NormalizeHeader).ToList();
        var id = headers.FindIndex(header => header == "id");
        var title = headers.FindIndex(header => header == "title");
        var release = headers.FindIndex(header => header == "release");
        var notes = headers.FindIndex(header => header == "notes");
        var testNotes = headers.FindIndex(header => header == "test notes");
        var releaseNotes = headers.FindIndex(header => header == "release notes");
        var jiraKey = headers.FindIndex(header => header is "issue key" or "key");
        var jiraSummary = headers.FindIndex(header => header is "summary" or "issue summary");
        var jiraRelease = headers.FindIndex(header => header.Contains("fix version") || header == "sprint");
        var jiraDescription = headers.FindIndex(header => header == "description");
        if (jiraKey >= 0 && jiraSummary >= 0)
        {
            return rows.Skip(1).Select(row => new AxosoftItem(
                Value(row, jiraKey), Value(row, jiraSummary), Value(row, jiraRelease), Value(row, jiraDescription), string.Empty, string.Empty))
                .Where(item => !string.IsNullOrWhiteSpace(item.Id) && !string.IsNullOrWhiteSpace(item.Title)).ToList();
        }
        if (id < 0 || title < 0 || release < 0)
        {
            // Axosoft's standard export places these fields in the first three columns.
            // Accept that layout even when a spreadsheet application has altered header text.
            if (rows[0].Count >= 3)
            {
                id = 0; title = 1; release = 2;
                notes = rows[0].Count > 3 ? 3 : -1;
            }
            else
            {
                throw new InvalidOperationException("The export must include Axosoft ID, Title, and Release columns, or JIRA Issue Key and Summary columns.");
            }
        }
        return rows.Skip(1).Select(row => new AxosoftItem(
                Value(row, id),
                Value(row, title),
                Value(row, release),
                Value(row, notes),
                Value(row, releaseNotes),
                Value(row, testNotes)))
            .Where(item => !string.IsNullOrWhiteSpace(item.Id) && !string.IsNullOrWhiteSpace(item.Title)).ToList();
    }

    private static List<List<string>> ParseCsv(string text)
    {
        var rows = new List<List<string>>(); var row = new List<string>(); var value = new System.Text.StringBuilder(); var quoted = false;
        for (var index = 0; index < text.Length; index++)
        {
            var character = text[index];
            if (character == '"') { if (quoted && index + 1 < text.Length && text[index + 1] == '"') { value.Append('"'); index++; } else quoted = !quoted; }
            else if (character == ',' && !quoted) { row.Add(value.ToString()); value.Clear(); }
            else if ((character == '\n' || character == '\r') && !quoted) { if (character == '\r' && index + 1 < text.Length && text[index + 1] == '\n') index++; row.Add(value.ToString()); value.Clear(); rows.Add(row); row = new(); }
            else value.Append(character);
        }
        if (value.Length > 0 || row.Count > 0) { row.Add(value.ToString()); rows.Add(row); }
        return rows;
    }

    private static List<List<string>> ParseWorkbook(byte[] content)
    {
        using var memory = new MemoryStream(content); using var archive = new ZipArchive(memory, ZipArchiveMode.Read);
        XNamespace main = "http://schemas.openxmlformats.org/spreadsheetml/2006/main";
        var shared = archive.GetEntry("xl/sharedStrings.xml");
        var strings = shared is null ? new List<string>() : XDocument.Load(shared.Open()).Descendants(main + "si").Select(item => string.Concat(item.Descendants(main + "t").Select(text => text.Value))).ToList();
        var sheet = archive.GetEntry("xl/worksheets/sheet1.xml") ?? throw new InvalidOperationException("The Excel export does not contain a first worksheet.");
        var document = XDocument.Load(sheet.Open());
        return document.Descendants(main + "row").Select(row => row.Elements(main + "c").Select(cell =>
        {
            var raw = cell.Element(main + "v")?.Value ?? string.Empty;
            return cell.Attribute("t")?.Value == "s" && int.TryParse(raw, out var sharedIndex) && sharedIndex < strings.Count ? strings[sharedIndex] : raw;
        }).ToList()).ToList();
    }
    private static string Value(List<string> row, int index) => index >= 0 && index < row.Count ? row[index].Trim() : string.Empty;
    private static string NormalizeHeader(string value) => value.Trim().TrimStart('\uFEFF').ToLowerInvariant();
}
