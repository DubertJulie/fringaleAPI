using fringaleAPI;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<FringaleDb>(opt => opt.UseInMemoryDatabase("Fringale"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();

app.MapGet("/Plats", async (FringaleDb db) =>
    await db.Plats.ToListAsync());

app.MapGet("/Plats/{id_pl}", async (int id_pl, FringaleDb db) =>
    await db.Plats.FindAsync(id_pl)
        is Plat plat
            ? Results.Ok(plat)
            : Results.NotFound());

app.MapPost("/Plats", async (Plat plat, FringaleDb db) =>
{
    db.Plats.Add(plat);
    await db.SaveChangesAsync();

    return Results.Created($"/Plats/{plat.Id_pl}", plat);
});

app.MapPut("/Plats/{id_pl}", async (int id_pl, Plat inputPlat, FringaleDb db) =>
{
    var plat = await db.Plats.FindAsync(id_pl);

    if (plat is null) return Results.NotFound();

    plat.Nom_pl = inputPlat.Nom_pl;
    plat.Prix_pl = inputPlat.Prix_pl;
    plat.Categorie_pl = inputPlat.Categorie_pl;

    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("/Plats/{id}", async (int id_pl, FringaleDb db) =>
{
    if (await db.Plats.FindAsync(id_pl) is Plat plat)
    {
        db.Plats.Remove(plat);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }

    return Results.NotFound();
});

app.Run();