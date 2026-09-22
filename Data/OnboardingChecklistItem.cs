using System.ComponentModel.DataAnnotations;

namespace ServicePortal.Data;

public sealed class OnboardingChecklistItem
{
    public int Id { get; set; }

    public int CustomerOnboardingId { get; set; }

    [Required, MaxLength(250)]
    public string Title { get; set; } = string.Empty;

    public bool IsComplete { get; set; }

    public int SortOrder { get; set; }

    public DateTime? CompletedAt { get; set; }

    public CustomerOnboarding? CustomerOnboarding { get; set; }
}
