using System.ComponentModel.DataAnnotations;
namespace ServicePortal.Data;
public sealed class ProjectRole { public int Id { get; set; } public int ProjectId { get; set; } [Required, MaxLength(100)] public string RoleName { get; set; } = string.Empty; [Required, MaxLength(150)] public string PersonName { get; set; } = string.Empty; }
