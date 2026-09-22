using Microsoft.EntityFrameworkCore;
using ServicePortal.Data;

namespace ServicePortal.Services;

public sealed class BrandingService(IDbContextFactory<ApplicationDbContext> dbFactory)
{
    public async Task SaveLogoAsync(byte[] content, string contentType)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var branding = await db.Set<PortalBranding>().FindAsync(1);
        if (branding is null)
        {
            branding = new PortalBranding { Id = 1 };
            db.Add(branding);
        }
        branding.LogoContent = content;
        branding.LogoContentType = contentType;
        await db.SaveChangesAsync();
    }
}
