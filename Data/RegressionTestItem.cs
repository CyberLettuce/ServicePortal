namespace ServicePortal.Data;

public sealed class RegressionTestItem
{
    public string Reference { get; set; } = string.Empty;
    public string Scope { get; set; } = string.Empty;
    public string TestCaseName { get; set; } = string.Empty;
    public string Status { get; set; } = "Not Started";
    public string TestedBy { get; set; } = string.Empty;
    public string SecondTestedBy { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
}
