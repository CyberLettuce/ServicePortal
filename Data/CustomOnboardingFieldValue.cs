using System.ComponentModel.DataAnnotations;
namespace ServicePortal.Data;
public sealed class CustomOnboardingFieldValue
{
    public int Id { get; set; }
    public int CustomerOnboardingId { get; set; }
    public int CustomOnboardingFieldId { get; set; }
    [MaxLength(2000)] public string Value { get; set; } = string.Empty;
}
