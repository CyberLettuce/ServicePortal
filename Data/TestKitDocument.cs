using System.ComponentModel.DataAnnotations;

namespace ServicePortal.Data;

public sealed class TestKitDocument
{
    public int Id { get; set; }
    [Required, MaxLength(260)] public string OriginalFileName { get; set; } = string.Empty;
    [Required, MaxLength(100)] public string ContentType { get; set; } = string.Empty;
    public long SizeBytes { get; set; }
    [Required, MaxLength(150)] public string UploadedBy { get; set; } = string.Empty;
    public DateTime UploadedAt { get; set; }
    public int ImportedCaseCount { get; set; }
    public byte[] Content { get; set; } = [];
}
