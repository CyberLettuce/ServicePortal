using System.ComponentModel.DataAnnotations;

namespace ServicePortal.Data;

public sealed class AutomationTestResult
{
    public long Id { get; set; }

    [MaxLength(100)] public string TestName { get; set; } = string.Empty;
    [MaxLength(100)] public string Country { get; set; } = string.Empty;
    [MaxLength(500)] public string Url { get; set; } = string.Empty;
    public bool Succeeded { get; set; }
    [MaxLength(500)] public string Summary { get; set; } = string.Empty;
    public string Output { get; set; } = string.Empty;
    [MaxLength(256)] public string StartedBy { get; set; } = string.Empty;
    public DateTime StartedAt { get; set; }
    public DateTime CompletedAt { get; set; }
}
