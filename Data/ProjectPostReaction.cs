namespace ServicePortal.Data;
public sealed class ProjectPostReaction { public int Id { get; set; } public int ProjectPostId { get; set; } public string UserId { get; set; } = string.Empty; public string Emoji { get; set; } = "👍"; }
