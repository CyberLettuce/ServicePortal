using System.ComponentModel.DataAnnotations;

namespace ServicePortal.Data;

public sealed class TestRun
{
    public int Id { get; set; }

    public int? TestPlanId { get; set; }

    public TestPlan? TestPlan { get; set; }

    [Required, MaxLength(50)]
    public string Product { get; set; } = string.Empty;

    [MaxLength(100)]
    public string ReleaseNumber { get; set; } = string.Empty;

    public string ReleaseStatus { get; set; } = "In Testing";

    public DateTime? ReleaseLiveDate { get; set; }

    [Required, MaxLength(100)]
    public string Scope { get; set; } = string.Empty;

    [Required, MaxLength(300)]
    public string TestCaseName { get; set; } = string.Empty;
    public string Reference { get; set; } = string.Empty;
    public string Instructions { get; set; } = string.Empty;
    public string ExpectedResult { get; set; } = string.Empty;

    public DateTime ScheduledDate { get; set; }

    [Required, MaxLength(20)]
    public string Status { get; set; } = "Not Started";

    [MaxLength(200)]
    public string TestedBy { get; set; } = string.Empty;

    public bool RequiresSecondTester { get; set; }

    [MaxLength(200)]
    public string SecondTestedBy { get; set; } = string.Empty;

    public DateTime? CompletedAt { get; set; }

    [MaxLength(2000)]
    public string Notes { get; set; } = string.Empty;

    [MaxLength(4000)]
    public string AxosoftNotes { get; set; } = string.Empty;

    [MaxLength(4000)]
    public string ReleaseNotes { get; set; } = string.Empty;

    [MaxLength(4000)]
    public string PointTest { get; set; } = string.Empty;

    [MaxLength(4000)]
    public string UatTest { get; set; } = string.Empty;

    public string RegressionTestsJson { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}
