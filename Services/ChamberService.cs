using Microsoft.EntityFrameworkCore;
using ServicePortal.Data;

namespace ServicePortal.Services;

public sealed class ChamberService(IDbContextFactory<ApplicationDbContext> dbFactory)
{
    private static readonly string[] DefaultNames = """
Aberdeen & Grampian Chamber of Commerce
Arab British Chamber Of Commerce
Ayrshire Chamber of Commerce & Industry
Bedfordshire & Luton Chamber of Commerce Business & Industry
Birmingham Chamber of Commerce and Industry
Black Country Chamber of Commerce & Industry
Bristol Chamber of Commerce
Calgary Chamber of Commerce
Cambridgeshire Chamber of Commerce
Chamber International
Chamber of Commerce (Barnsley & Rotherham)
Chambers Wales Chamber of Commerce
Coventry & Warwickshire Chamber of Commerce
Cumbria Chamber of Commerce
Devon Chamber of Commerce and Cornwall Chamber of Commerce
Doncaster Chamber of Commerce & Enterprise
Dorset Chamber of Commerce & Industry
Dublin Chamber of Commerce
Dundee & Angus Chamber of Commerce
East Cheshire Chamber of Commerce & Enterprise
Estonia Chamber of Commerce
East Lancashire Chamber of Commerce
East Midlands Chamber of Commerce
Essex Chamber of Commerce
Fife Chamber of Commerce & Enterprise
Glasgow Chamber of Commerce
Greater Manchester Airport Chamber of Commerce
Greater Manchester Chamber of Commerce
Guernsey Chamber of Commerce
Halton Chamber of Commerce & Enterprise
Hampshire Chamber of Commerce
Herefordshire and Worcestershire Chamber
Hertfordshire Chamber of Commerce
Hull & Humber Chamber of Commerce Industry & Shipping
Inverness Chamber of Commerce
Isle of Wight Chamber of Commerce
Kent Invicta Chamber of Commerce
Lancaster & Morecambe Chamber of Commerce
Lincolnshire Chamber of Commerce & Industry
Liverpool Chamber of Commerce CIC
Malta Chamber of Commerce
Mid-Yorkshire Chamber of Commerce and Industry
Norfolk Chamber of Commerce & Industry
North and Western Lancashire Chamber of Commerce
North East Chamber of Commerce
Northamptonshire Chamber of Commerce
Northern Ireland Chamber of Commerce
Renfrewshire Chamber of Commerce Limited
Sheffield Chamber of Commerce & Industry
South and North Cheshire Chamber of Commerce and Industry Ltd
Staffordshire & Shropshire Chambers of Commerce & Industry Ltd
Suffolk Chamber of Commerce.
Surrey Chambers of Commerce
Surrey Heathrow Chambers of Commerce
Sussex Chamber of Commerce & Enterprise
Thames Valley Chamber of Commerce & Industry (Banbury)
Thames Valley Chamber of Commerce & Industry (Heathrow)
Thames Valley Chamber of Commerce and Industry (Slough)
Turkish-British Chamber of Commerce & Industry
Warrington Chamber of Commerce & Industry
West Cheshire and North Wales Chamber of Commerce
Wirral Chamber of Commerce
""".Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
    public async Task<List<string>> GetNamesAsync()
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var custom = await db.ChambersOfCommerce.Select(x => x.Name).ToListAsync();
        return DefaultNames.Concat(custom).Distinct(StringComparer.OrdinalIgnoreCase).OrderBy(x => x).ToList();
    }

    public async Task AddAsync(string name)
    {
        name = name.Trim();
        if (string.IsNullOrWhiteSpace(name)) return;
        await using var db = await dbFactory.CreateDbContextAsync();
        if (!await db.ChambersOfCommerce.AnyAsync(x => x.Name == name))
        {
            db.ChambersOfCommerce.Add(new ChamberOfCommerce { Name = name });
            await db.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(int id)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var chamber = await db.ChambersOfCommerce.FindAsync(id);
        if (chamber is null) return;
        db.ChambersOfCommerce.Remove(chamber);
        await db.SaveChangesAsync();
    }

    public async Task<List<ChamberOfCommerce>> GetAllAsync()
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        return await db.ChambersOfCommerce.OrderBy(x => x.Name).ToListAsync();
    }
}

