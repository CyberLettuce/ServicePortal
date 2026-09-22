using System.ComponentModel.DataAnnotations;

namespace ServicePortal.Data;

public sealed class AuditLog
{
    public long Id { get; set; }

    public DateTime CreatedAt { get; set; }

    [MaxLength(450)]
    public string? ActorUserId { get; set; }

    [MaxLength(256)]
    public string ActorEmail { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Action { get; set; } = string.Empty;

    [MaxLength(450)]
    public string? TargetUserId { get; set; }

    [MaxLength(256)]
    public string TargetEmail { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string Details { get; set; } = string.Empty;
}
