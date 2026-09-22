using System.ComponentModel.DataAnnotations;
namespace ServicePortal.Data;
public sealed class CustomOnboardingField
{
    public int Id { get; set; }
    [Required, MaxLength(100)] public string Label { get; set; } = string.Empty;
    [Required, MaxLength(20)] public string FieldType { get; set; } = "Text";
    [MaxLength(1000)] public string Options { get; set; } = string.Empty;
    public int SortOrder { get; set; }
}
