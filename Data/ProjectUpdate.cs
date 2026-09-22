using System.ComponentModel.DataAnnotations;

namespace ServicePortal.Data;

public sealed class ProjectUpdate
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public Project? Project { get; set; }
    [Required, MaxLength(30)] public string Kind { get; set; } = "Update";
    [Required, MaxLength(3000)] public string Message { get; set; } = string.Empty;
    [Required, MaxLength(150)] public string Author { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
