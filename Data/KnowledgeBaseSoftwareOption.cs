using System.ComponentModel.DataAnnotations;

namespace ServicePortal.Data;

public sealed class KnowledgeBaseSoftwareOption
{
    public int Id { get; set; }
    [MaxLength(50)] public string Software { get; set; } = string.Empty;
    [MaxLength(100)] public string Subcategory { get; set; } = string.Empty;
    public int SortOrder { get; set; }
    public byte[]? ImageContent { get; set; }
    [MaxLength(100)] public string ImageContentType { get; set; } = string.Empty;
}
