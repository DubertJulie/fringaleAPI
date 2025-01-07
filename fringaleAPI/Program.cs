using Microsoft.EntityFrameworkCore;
using fringaleAPI;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<FringaleAPIDb>(opt => opt.UseInMemoryDatabase("Fringale"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();

app.MapGet("/clients", async (FringaleAPIDb db) =>
    await db.Clients.ToListAsync());

app.MapGet("/Clients/{id}", async (int id, FringaleAPIDb db) =>
    await db.Clients.FindAsync(id)
        is Client client
            ? Results.Ok(client)
            : Results.NotFound());

app.MapPost("/clients", async (Client client, FringaleAPIDb db) =>
    {
        db.Clients.Add(client);
        await db.SaveChangesAsync();

        return Results.Created($"/clients/{client.id_cl}", client);
    });

app.MapPut("/clients/{id}", async (int id, Client inputClient, FringaleAPIDb db) =>
{
    var client = await db.Clients.FindAsync(id);

    if (client is null) return Results.NotFound();

    client.Nom_cl = inputClient.Nom_cl;
    client.Prenom_cl = inputClient.Prenom_cl;
    client.Adresse_cl = inputClient.Adresse_cl;
    client.Telephone_cl = inputClient.Telephone_cl;

    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("/clients/{id}", async (int id, FringaleAPIDb db) =>
{
    if (await db.Clients.FindAsync(id) is Client client)
    {
        db.Clients.Remove(client);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }
    return Results.NotFound();
});