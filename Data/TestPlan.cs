using System.ComponentModel.DataAnnotations;

namespace ServicePortal.Data;

public sealed class TestPlan
{
    public int Id { get; set; }

    [Required, MaxLength(50)]
    public string Product { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string Scope { get; set; } = string.Empty;

    [Required, MaxLength(300)]
    public string TestCaseName { get; set; } = string.Empty;
    [MaxLength(100)] public string Reference { get; set; } = string.Empty;
    [MaxLength(3000)] public string Instructions { get; set; } = string.Empty;
    [MaxLength(3000)] public string ExpectedResult { get; set; } = string.Empty;

    [Required, MaxLength(20)]
    public string Frequency { get; set; } = "Monthly";

    public bool IsActive { get; set; } = true;

    public bool RequiresSecondTester { get; set; }

    public DateTime CreatedAt { get; set; }

    public List<TestRun> TestRuns { get; set; } = new();
}
