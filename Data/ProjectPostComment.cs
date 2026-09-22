using System.ComponentModel.DataAnnotations;
namespace ServicePortal.Data;
public sealed class ProjectPostComment { public int Id { get; set; } public int ProjectPostId { get; set; } [Required, MaxLength(150)] public string Author { get; set; } = string.Empty; [Required, MaxLength(2000)] public string Message { get; set; } = string.Empty; public DateTime CreatedAt { get; set; } }
