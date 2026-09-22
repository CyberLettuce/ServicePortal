using System.ComponentModel.DataAnnotations;

namespace ServicePortal.Data;

public sealed class Project
{
    public int Id { get; set; }
    [Required, MaxLength(200)] public string Name { get; set; } = string.Empty;
    [MaxLength(2000)] public string Description { get; set; } = string.Empty;
    [Required, MaxLength(100)] public string Owner { get; set; } = string.Empty;
    [MaxLength(150)] public string ProjectLead { get; set; } = string.Empty;
    [MaxLength(150)] public string Developer { get; set; } = string.Empty;
    [MaxLength(260)] public string ImageFileName { get; set; } = string.Empty;
    public byte[]? ImageContent { get; set; }
    [MaxLength(100)] public string ImageContentType { get; set; } = string.Empty;
    [Required, MaxLength(30)] public string Status { get; set; } = "In Progress";
    public DateTime StartDate { get; set; }
    public DateTime? TargetDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public List<ProjectUpdate> Updates { get; set; } = new();
    public List<ProjectFollower> Followers { get; set; } = new();
    public List<ProjectPost> Posts { get; set; } = new();
    public List<ProjectRole> Roles { get; set; } = new();
}
