using ServicePortal.Data;
using ServicePortal.Models;
using Microsoft.EntityFrameworkCore;

namespace ServicePortal.Services;

public sealed class ProjectService
{
    private readonly IDbContextFactory<ApplicationDbContext> dbFactory;
    private readonly AuditLogService auditLog;
    private readonly IWebHostEnvironment environment;
    public ProjectService(IDbContextFactory<ApplicationDbContext> dbFactory, AuditLogService auditLog, IWebHostEnvironment environment) { this.dbFactory = dbFactory; this.auditLog = auditLog; this.environment = environment; }

    public async Task<List<Project>> GetAllAsync()
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        return await db.Projects.AsNoTracking().Include(project => project.Updates)
            .OrderBy(project => project.Status == "Complete" || project.Status == "Completed" || project.Status == "Cancelled").ThenBy(project => project.TargetDate).ThenBy(project => project.Name).ToListAsync();
    }
    public async Task<Project?> GetAsync(int id)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        return await db.Projects.Include(project => project.Updates).Include(project => project.Followers).Include(project => project.Roles).Include(project => project.Posts).ThenInclude(post => post.Comments).Include(project => project.Posts).ThenInclude(post => post.Reactions).SingleOrDefaultAsync(project => project.Id == id);
    }

    public async Task<List<Project>> GetActiveProjectsAsync()
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        return await db.Projects.AsNoTracking()
            .Where(project => project.Status != "Complete" && project.Status != "Completed" && project.Status != "Cancelled")
            .OrderBy(project => project.Name)
            .ToListAsync();
    }

    public async Task<List<Ticket>> GetLinkedTicketsAsync(int projectId)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        return await db.Tickets.AsNoTracking()
            .Where(ticket => ticket.ProjectId == projectId)
            .OrderByDescending(ticket => ticket.CreatedDate)
            .ToListAsync();
    }
    public async Task<int> CreateAsync(Project project, string author)
    {
        project.Name = project.Name.Trim(); project.Description = project.Description.Trim(); project.Owner = project.Owner.Trim(); project.ProjectLead = project.ProjectLead.Trim(); project.Developer = project.Developer.Trim();
        project.CreatedAt = DateTime.UtcNow; project.UpdatedAt = DateTime.UtcNow;
        project.Updates.Add(new ProjectUpdate { Kind = "Milestone", Message = "Project created.", Author = author, CreatedAt = DateTime.UtcNow });
        await using var db = await dbFactory.CreateDbContextAsync(); db.Projects.Add(project); await db.SaveChangesAsync();
        await auditLog.RecordAsync("Project created", project.Id.ToString(), project.Name, "Owner: " + project.Owner + ".");
        return project.Id;
    }
    public async Task AddUpdateAsync(int projectId, string kind, string message, string author)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var project = await db.Projects.FindAsync(projectId); if (project is null) return;
        db.ProjectUpdates.Add(new ProjectUpdate { ProjectId = projectId, Kind = kind, Message = message.Trim(), Author = author, CreatedAt = DateTime.UtcNow });
        var followers = await db.ProjectFollowers.Where(follower => follower.ProjectId == projectId).ToListAsync();
        foreach (var follower in followers)
            db.Notifications.Add(new Notification { RecipientUserId = follower.UserId, ProjectId = projectId, Message = $"{author} added a {kind.ToLowerInvariant()} to {project.Name}: {message.Trim()}", CreatedAt = DateTime.UtcNow });
        project.UpdatedAt = DateTime.UtcNow; await db.SaveChangesAsync();
        await auditLog.RecordAsync("Project " + kind.ToLowerInvariant() + " added", projectId.ToString(), project.Name, message.Trim());
    }

    public async Task UpdateDetailsAsync(Project updated)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var project = await db.Projects.FindAsync(updated.Id); if (project is null) return;
        project.Name = updated.Name.Trim(); project.Description = updated.Description.Trim(); project.Owner = updated.Owner.Trim(); project.ProjectLead = updated.ProjectLead.Trim(); project.Developer = updated.Developer.Trim(); project.Status = updated.Status; project.TargetDate = updated.TargetDate; project.UpdatedAt = DateTime.UtcNow;
        await db.SaveChangesAsync();
    }

    public async Task<bool> DeleteUpdateAsync(int projectId, int updateId)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var update = await db.ProjectUpdates.SingleOrDefaultAsync(item => item.Id == updateId && item.ProjectId == projectId);
        if (update is null) return false;

        var project = await db.Projects.FindAsync(projectId);
        db.ProjectUpdates.Remove(update);
        if (project is not null) project.UpdatedAt = DateTime.UtcNow;
        await db.SaveChangesAsync();
        if (project is not null)
            await auditLog.RecordAsync("Project timeline entry deleted", projectId.ToString(), project.Name, $"Removed {update.Kind}: {update.Message}");
        return true;
    }

    public async Task<bool> UpdateUpdateAsync(int projectId, int updateId, string kind, string message)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var update = await db.ProjectUpdates.SingleOrDefaultAsync(item => item.Id == updateId && item.ProjectId == projectId);
        if (update is null || string.IsNullOrWhiteSpace(message)) return false;
        var project = await db.Projects.FindAsync(projectId);
        update.Kind = kind;
        update.Message = message.Trim();
        if (project is not null) project.UpdatedAt = DateTime.UtcNow;
        await db.SaveChangesAsync();
        if (project is not null)
            await auditLog.RecordAsync("Project timeline entry updated", projectId.ToString(), project.Name, $"Updated {kind}: {update.Message}");
        return true;
    }

    public async Task UpdateImageAsync(int projectId, string fileName)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var project = await db.Projects.FindAsync(projectId); if (project is null) return;
        project.ImageFileName = fileName;
        project.UpdatedAt = DateTime.UtcNow;
        await db.SaveChangesAsync();
        _ = PersistImageAsync(projectId, fileName);
    }

    private async Task PersistImageAsync(int projectId, string fileName)
    {
        // The browser upload stream releases the file after the calling method finishes.
        await Task.Delay(250);
        var path = Path.Combine(environment.WebRootPath, "uploads", "project-images", fileName);
        if (!File.Exists(path)) return;

        var content = await File.ReadAllBytesAsync(path);
        await using var db = await dbFactory.CreateDbContextAsync();
        var project = await db.Projects.FindAsync(projectId);
        if (project is null || project.ImageFileName != fileName) return;
        project.ImageContent = content;
        project.ImageContentType = Path.GetExtension(fileName).ToLowerInvariant() switch
        {
            ".png" => "image/png",
            ".webp" => "image/webp",
            _ => "image/jpeg"
        };
        await db.SaveChangesAsync();
    }

    public Task AddPostAsync(int projectId, string message, string author) => AddPostAsync(projectId, message, author, string.Empty);
    public async Task AddPostAsync(int projectId, string message, string author, string status)
    { if (string.IsNullOrWhiteSpace(message)) return; await using var db = await dbFactory.CreateDbContextAsync(); var project = await db.Projects.FindAsync(projectId); if (project is null) return; db.ProjectPosts.Add(new ProjectPost { ProjectId = projectId, Message = message.Trim(), Author = author, CreatedAt = DateTime.UtcNow }); if (!string.IsNullOrWhiteSpace(status)) project.Status = status; foreach (var follower in await db.ProjectFollowers.Where(item => item.ProjectId == projectId).ToListAsync()) db.Notifications.Add(new Notification { RecipientUserId = follower.UserId, ProjectId = projectId, Message = $"{author} posted an update on {project.Name}: {message.Trim()}", CreatedAt = DateTime.UtcNow }); project.UpdatedAt = DateTime.UtcNow; await db.SaveChangesAsync(); }
    public async Task UpdatePostAsync(int postId, string message)
    { if (string.IsNullOrWhiteSpace(message)) return; await using var db = await dbFactory.CreateDbContextAsync(); var post = await db.ProjectPosts.FindAsync(postId); if (post is null) return; post.Message = message.Trim(); await db.SaveChangesAsync(); }
    public async Task DeletePostAsync(int postId)
    { await using var db = await dbFactory.CreateDbContextAsync(); var post = await db.ProjectPosts.FindAsync(postId); if (post is null) return; db.ProjectPosts.Remove(post); await db.SaveChangesAsync(); }
    public async Task AddCommentAsync(int postId, string message, string author)
    { if (string.IsNullOrWhiteSpace(message)) return; await using var db = await dbFactory.CreateDbContextAsync(); db.ProjectPostComments.Add(new ProjectPostComment { ProjectPostId = postId, Message = message.Trim(), Author = author, CreatedAt = DateTime.UtcNow }); await db.SaveChangesAsync(); }
    public async Task UpdateCommentAsync(int commentId, string message)
    { if (string.IsNullOrWhiteSpace(message)) return; await using var db = await dbFactory.CreateDbContextAsync(); var comment = await db.ProjectPostComments.FindAsync(commentId); if (comment is null) return; comment.Message = message.Trim(); await db.SaveChangesAsync(); }
    public async Task DeleteCommentAsync(int commentId)
    { await using var db = await dbFactory.CreateDbContextAsync(); var comment = await db.ProjectPostComments.FindAsync(commentId); if (comment is null) return; db.ProjectPostComments.Remove(comment); await db.SaveChangesAsync(); }
    public async Task ToggleReactionAsync(int postId, string userId, string emoji)
    { await using var db = await dbFactory.CreateDbContextAsync(); var reaction = await db.ProjectPostReactions.SingleOrDefaultAsync(item => item.ProjectPostId == postId && item.UserId == userId && item.Emoji == emoji); if (reaction is null) db.ProjectPostReactions.Add(new ProjectPostReaction { ProjectPostId = postId, UserId = userId, Emoji = emoji }); else db.ProjectPostReactions.Remove(reaction); await db.SaveChangesAsync(); }
    public async Task AddRoleAsync(int projectId, string roleName, string personName)
    { if (string.IsNullOrWhiteSpace(roleName) || string.IsNullOrWhiteSpace(personName)) return; await using var db = await dbFactory.CreateDbContextAsync(); db.ProjectRoles.Add(new ProjectRole { ProjectId = projectId, RoleName = roleName.Trim(), PersonName = personName.Trim() }); await db.SaveChangesAsync(); }
    public async Task DeleteRoleAsync(int roleId)
    { await using var db = await dbFactory.CreateDbContextAsync(); var role = await db.ProjectRoles.FindAsync(roleId); if (role is null) return; db.ProjectRoles.Remove(role); await db.SaveChangesAsync(); }
    public async Task UpdateRoleAsync(int roleId, string roleName, string personName)
    { if (string.IsNullOrWhiteSpace(roleName) || string.IsNullOrWhiteSpace(personName)) return; await using var db = await dbFactory.CreateDbContextAsync(); var role = await db.ProjectRoles.FindAsync(roleId); if (role is null) return; role.RoleName = roleName.Trim(); role.PersonName = personName.Trim(); await db.SaveChangesAsync(); }

    public async Task<bool> ToggleFollowAsync(int projectId, string userId)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var follower = await db.ProjectFollowers.FindAsync(projectId, userId);
        if (follower is null) { db.ProjectFollowers.Add(new ProjectFollower { ProjectId = projectId, UserId = userId }); await db.SaveChangesAsync(); return true; }
        db.ProjectFollowers.Remove(follower); await db.SaveChangesAsync(); return false;
    }

    public async Task<bool> DeleteAsync(int projectId)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var project = await db.Projects.Include(item => item.Updates).SingleOrDefaultAsync(item => item.Id == projectId);
        if (project is null) return false;
        var updateCount = project.Updates.Count;
        db.Projects.Remove(project);
        await db.SaveChangesAsync();
        await auditLog.RecordAsync("Project deleted", projectId.ToString(), project.Name,
            "Deleted project and " + updateCount + " timeline entr" + (updateCount == 1 ? "y." : "ies."));
        return true;
    }
}
