using System.ComponentModel.DataAnnotations;

namespace ServicePortal.Data;

public sealed class KnowledgeBaseCategory
{
    public int Id { get; set; }
    [MaxLength(100)] public string Name { get; set; } = string.Empty;
    public int SortOrder { get; set; }
}

public sealed class KnowledgeArticle
{
    public int Id { get; set; }
    public int CategoryId { get; set; }
    [MaxLength(200)] public string Title { get; set; } = string.Empty;
    [MaxLength(500)] public string Summary { get; set; } = string.Empty;
    public string Symptoms { get; set; } = string.Empty;
    public string Resolution { get; set; } = string.Empty;
    public string AdditionalInformation { get; set; } = string.Empty;
    [MaxLength(500)] public string Tags { get; set; } = string.Empty;
    [MaxLength(50)] public string Software { get; set; } = string.Empty;
    [MaxLength(100)] public string Subcategory { get; set; } = string.Empty;
    public bool IsPublished { get; set; }
    public bool IsArchived { get; set; }
    public int ViewCount { get; set; }
    public int UsefulCount { get; set; }
    public int NotUsefulCount { get; set; }
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public KnowledgeBaseCategory? Category { get; set; }
}
