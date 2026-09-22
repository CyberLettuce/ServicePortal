using System.ComponentModel.DataAnnotations;
namespace ServicePortal.Data;
public sealed class ProjectPost { public int Id { get; set; } public int ProjectId { get; set; } [Required, MaxLength(3000)] public string Message { get; set; } = string.Empty; [Required, MaxLength(150)] public string Author { get; set; } = string.Empty; public DateTime CreatedAt { get; set; } public List<ProjectPostComment> Comments { get; set; } = []; public List<ProjectPostReaction> Reactions { get; set; } = []; }
