using Microsoft.EntityFrameworkCore;
using fringaleAPI;
using Microsoft.AspNetCore.Components.Forms;
using System.Formats.Tar;
using System;
using System.Text.Json;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Annotations;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<FringaleAPIDb>(opt => opt.UseSqlite("Data Source=FringaleAPI.db"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("V1", new OpenApiInfo
    {
        Title = "Todo API",
        Version = "V1",
        Description = "Une API pour g�rer une liste de t�ches",
        Contact = new OpenApiContact
        {
            Name = "Julie",
            Email = "dbt.julie@gmail.com",
            Url = new Uri("https://votre-site.com"),
        }
    });

    // Activer les annotations
    c.EnableAnnotations();
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/V1/swagger.json", "FringaleAPI V1");
        c.RoutePrefix = ""; // possibilité de changer le prefix 
    });
}

// Initialiser la base de donn�es mon json (méthode seed)
using (var scope = app.Services.CreateScope())
{
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<FringaleAPIDb>();
        DbInitializer.Seed(dbContext);
    }
}


// ROUTES

// MAP GROUP CLIENT
var gestionClients = app.MapGroup("/clients").WithTags("Gestion clients");
gestionClients.MapGet("/", GetAllClients);
gestionClients.MapGet("/{id}", GetClientById);
gestionClients.MapPost("/", CreateClient);
gestionClients.MapPut("/{id}", UpdateClient);
gestionClients.MapDelete("/{id}", DeleteClient);

// Récupérer tous les clients
static async Task<IResult> GetAllClients(FringaleAPIDb db)
{
    return TypedResults.Ok(await db.Clients.ToListAsync());
}

// Récupérer un client par son ID
static async Task<IResult> GetClientById(int id, FringaleAPIDb db)
{
    return await db.Clients.FindAsync(id)
       is Client client
           ? TypedResults.Ok(client)
           : TypedResults.NotFound();
}

// Créer un client 
static async Task<IResult> CreateClient(Client client, FringaleAPIDb db)
{
    db.Clients.Add(client);
    await db.SaveChangesAsync();

    return TypedResults.Created($"/clients/{client.Id_cl}", client);
}

//// Modifier un client par son ID
static async Task<IResult> UpdateClient(int id, Client inputClient, FringaleAPIDb db)
{
    var client = await db.Clients.FindAsync(id);

    if (client is null) return Results.NotFound();

    client.Nom_cl = inputClient.Nom_cl;
    client.Prenom_cl = inputClient.Prenom_cl;
    client.Adresse_cl = inputClient.Adresse_cl;
    client.Telephone_cl = inputClient.Telephone_cl;

    await db.SaveChangesAsync();

    return TypedResults.NoContent();
}

// Supprimer un client par son ID 
static async Task<IResult> DeleteClient(int id, FringaleAPIDb db)
{
    if (await db.Clients.FindAsync(id) is Client client)
    {
        db.Clients.Remove(client);
        await db.SaveChangesAsync();
        return TypedResults.NoContent();
    }

    return TypedResults.NotFound();
}

//Récupérer toutes les commandes d'un client 
gestionClients.MapGet("/{id}/commandes", async (int id, FringaleAPIDb db) =>
{
    var client = await db.Clients.FindAsync(id);
    if (client == null) return Results.NotFound();

    var commandes = await db.Commandes
                            .Where(o => o.Id_cl == id)
                            .ToListAsync(); // requ�te LINQ 

    return Results.Ok(commandes);
});

var gestionCommandes = app.MapGroup("/commandes").WithTags("Gestion commandes");
gestionCommandes.MapGet("/", GetAllCommands);
gestionCommandes.MapGet("/{id}", GetCommandbyId);
gestionCommandes.MapGet("/{date_co}", GetCommandbyDate);
gestionCommandes.MapPost("/", CreateCommand);
gestionCommandes.MapPut("/{id}", UpdateCommand);
gestionCommandes.MapDelete("/{id}", DeleteCommand);


// Récupérer toutes les commandes
static async Task<IResult> GetAllCommands(FringaleAPIDb db)
{
    return TypedResults.Ok(await db.Commandes.ToListAsync());
}

// Récupérer une commande par son ID
static async Task<IResult> GetCommandbyId(int id, FringaleAPIDb db)
{
    return await db.Commandes.FindAsync(id)
        is Commande commande
            ? TypedResults.Ok(commande)
            : TypedResults.NotFound();
}

// Récupérer une commande par sa date
static async Task<IResult> GetCommandbyDate(DateTime date_co, FringaleAPIDb db)
{
    var commandes = await db.Commandes.Where(c => c.Date_co.Date == date_co).ToListAsync();
    return TypedResults.Ok(commandes);
}

// Créer une commande
static async Task<IResult> CreateCommand(Commande commande, FringaleAPIDb db)
{
    db.Commandes.Add(commande);
    await db.SaveChangesAsync();

    return TypedResults.Created($"/clients/{commande.Id_co}", commande);
}


// Modifier une commande

static async Task<IResult> UpdateCommand(int id, Commande inputCommande, FringaleAPIDb db)
{
    var commande = await db.Commandes.FindAsync(id);

    if (commande is null) return TypedResults.NotFound();

    commande.Montant_co = inputCommande.Montant_co;
    commande.Date_co = inputCommande.Date_co;

    await db.SaveChangesAsync();
    return TypedResults.NoContent();
}



// Supprimer une commande par son ID
static async Task<IResult> DeleteCommand(int id, FringaleAPIDb db)
{
    if (await db.Commandes.FindAsync(id) is Commande commande)
    {
        db.Commandes.Remove(commande);
        await db.SaveChangesAsync();
        return TypedResults.NoContent();
    }

    return TypedResults.NotFound();
}


// MAP GROUP PLATS
var gestionPlats = app.MapGroup("/plats").WithTags("Gestion plats");
gestionPlats.MapGet("/", GetAllPlats);
gestionPlats.MapGet("/{id}", GetPlatbyId);
gestionPlats.MapPost("/", CreatePlat);
gestionPlats.MapPut("/{id}", UpdatePlat);
gestionPlats.MapDelete("/{id}", DeletePlat);

// Récupérer les plats 
static async Task<IResult> GetAllPlats(FringaleAPIDb db)
{
    return TypedResults.Ok(await db.Plats.ToArrayAsync());
}


// Récupérer les plats par ID 
static async Task<IResult> GetPlatbyId(int id, FringaleAPIDb db)
{
    return await db.Plats.FindAsync(id)
       is Plat plat
           ? TypedResults.Ok(plat)
           : TypedResults.NotFound();
}

// Créer un plat 
static async Task<IResult> CreatePlat(Plat plat, FringaleAPIDb db)
{
    db.Plats.Add(plat);
    await db.SaveChangesAsync();

    return TypedResults.Created($"/clients/{plat.Id_pl}", plat);
}


// Modifier un plat

static async Task<IResult> UpdatePlat(int id, Plat inputPlat, FringaleAPIDb db)
{
    var plat = await db.Plats.FindAsync(id);

    if (plat is null) return TypedResults.NotFound();

    plat.Nom_pl = inputPlat.Nom_pl;
    plat.Prix_pl = inputPlat.Prix_pl;
    plat.Categorie_pl = inputPlat.Categorie_pl;

    await db.SaveChangesAsync();
    return TypedResults.NoContent();
}



// Supprimer une commande par son ID
static async Task<IResult> DeletePlat(int id, FringaleAPIDb db)
{
    if (await db.Plats.FindAsync(id) is Plat plat)
    {
        db.Plats.Remove(plat);
        await db.SaveChangesAsync();
        return TypedResults.NoContent();
    }

    return TypedResults.NotFound();
}

app.Run();