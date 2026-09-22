using System.ComponentModel.DataAnnotations;

namespace ServicePortal.Data;

public sealed class BrowserTest
{
    public int Id { get; set; }
    [MaxLength(30)] public string Product { get; set; } = string.Empty;
    [MaxLength(150)] public string Name { get; set; } = string.Empty;
    [MaxLength(500)] public string StartUrl { get; set; } = string.Empty;
    [MaxLength(256)] public string CreatedBy { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public List<BrowserTestStep> Steps { get; set; } = [];
}

public sealed class BrowserTestStep
{
    public int Id { get; set; }
    public int BrowserTestId { get; set; }
    public int SortOrder { get; set; }
    [MaxLength(30)] public string Action { get; set; } = "Click";
    [MaxLength(500)] public string Target { get; set; } = string.Empty;
    [MaxLength(1000)] public string Value { get; set; } = string.Empty;
}
