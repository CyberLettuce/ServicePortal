using System.ComponentModel.DataAnnotations;

namespace ServicePortal.Data;

public sealed class TicketTemplate
{
    public int Id { get; set; }
    [Required, MaxLength(100)] public string Name { get; set; } = string.Empty;
    // Kept for compatibility with the first Ticket Templates release.
    [MaxLength(3000)] public string Description { get; set; } = string.Empty;
    [MaxLength(500)] public string MainIssue { get; set; } = string.Empty;
    [MaxLength(3000)] public string AdditionalDetails { get; set; } = string.Empty;
    [MaxLength(500)] public string ReferencesLrn { get; set; } = string.Empty;
    [MaxLength(500)] public string MainContact { get; set; } = string.Empty;
    [MaxLength(3000)] public string ActionsTaken { get; set; } = string.Empty;
    [MaxLength(250)] public string SoftwareModule { get; set; } = string.Empty;
    [MaxLength(250)] public string Mrn { get; set; } = string.Empty;
    [MaxLength(250)] public string Arc { get; set; } = string.Empty;
    [MaxLength(250)] public string Tsn { get; set; } = string.Empty;
    [MaxLength(250)] public string ShipmentReference { get; set; } = string.Empty;
    [MaxLength(20)] public string Priority { get; set; } = "Medium";
    [MaxLength(100)] public string Category { get; set; } = "eCERT";
    [MaxLength(150)] public string AssignedTo { get; set; } = "Unassigned";
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
