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
        Title = "FRINGALE API",
        Version = "V1",
        Description = "Une solution de gestion de commandes pour les restaurateurs",
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

// Initialiser la base de donn�es avec un json (méthode seed)
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
gestionClients.MapGet("/", GetAllClients)
    .WithMetadata(new SwaggerOperationAttribute(
        summary: "Récupère tous les éléments Clients",
        description: "Renvoie une liste de tous les éléments Clients."))
    .WithMetadata(new SwaggerResponseAttribute(200, "Liste de clients trouvés"))
    .WithMetadata(new SwaggerResponseAttribute(404, "Aucun client n'a été trouvé."));
gestionClients.MapGet("/{id}", GetClientById)
    .WithMetadata(new SwaggerOperationAttribute(
        summary: "Récupère un client par son identifiant",
        description: "Renvoie le client pointé par son identifiant"))
    .WithMetadata(new SwaggerResponseAttribute(200, "Un client correspondant"))
    .WithMetadata(new SwaggerResponseAttribute(404, "Aucun client n'a été trouvé."));
gestionClients.MapPost("/", CreateClient)
    .WithMetadata(new SwaggerOperationAttribute(
        summary: "Crée un client",
        description: "Ajoute un nouveau client dans la base de données"))
    .WithMetadata(new SwaggerResponseAttribute(201, "Client bien créé !"))
    .WithMetadata(new SwaggerResponseAttribute(400, "Requête invalide"));
gestionClients.MapPut("/{id}", UpdateClient)
    .WithMetadata(new SwaggerOperationAttribute(
        summary: "Modifie un client",
        description: "Modification des informations d'un client"))
    .WithMetadata(new SwaggerResponseAttribute(201, "Client bien modifié !"))
    .WithMetadata(new SwaggerResponseAttribute(400, "La modification du client a échoué."));
gestionClients.MapDelete("/{id}", DeleteClient)
    .WithMetadata(new SwaggerOperationAttribute(
        summary: "Supprime un client",
        description: "Suppression d'un client de la base de données"))
    .WithMetadata(new SwaggerResponseAttribute(201, "Client bien supprimé !"))
    .WithMetadata(new SwaggerResponseAttribute(400, "La suppression du client a échoué."));

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
gestionCommandes.MapGet("/", GetAllCommands)
    .WithMetadata(new SwaggerOperationAttribute(
        summary: "Récupère tous les éléments Commandes",
        description: "Renvoie une liste de tous les éléments Commandes."))
    .WithMetadata(new SwaggerResponseAttribute(200, "Liste de commandes trouvées"))
    .WithMetadata(new SwaggerResponseAttribute(404, "Aucune commande n'a été trouvée."));
gestionCommandes.MapGet("/{id}", GetCommandbyId)
    .WithMetadata(new SwaggerOperationAttribute(
        summary: "Récupère une commande par son identifiant",
        description: "Renvoie le commande pointé par son identifiant"))
    .WithMetadata(new SwaggerResponseAttribute(200, "Une commande correspond"))
    .WithMetadata(new SwaggerResponseAttribute(404, "Aucune commande n'a été trouvée."));
gestionCommandes.MapGet("date/{date_co}", GetCommandbyDate)
    .WithMetadata(new SwaggerOperationAttribute(
        summary: "Récupère une commande par sa date",
        description: "Renvoie le commande pointé par sa date"))
    .WithMetadata(new SwaggerResponseAttribute(200, "Une commande trouvée à la date demandée."))
    .WithMetadata(new SwaggerResponseAttribute(404, "Aucune commande n'a été trouvée."));
gestionCommandes.MapPost("/", CreateCommand)
    .WithMetadata(new SwaggerOperationAttribute(
        summary: "Crée une commande",
        description: "Ajoute une nouvelle commande dans la base de données"))
    .WithMetadata(new SwaggerResponseAttribute(201, "Commande bien créé !"))
    .WithMetadata(new SwaggerResponseAttribute(400, "Requête invalide"));
gestionCommandes.MapPut("/{id}", UpdateCommand)
    .WithMetadata(new SwaggerOperationAttribute(
        summary: "Modifie une commande",
        description: "Modification des informations d'une commande"))
    .WithMetadata(new SwaggerResponseAttribute(201, "La commande a bien été modifiée !"))
    .WithMetadata(new SwaggerResponseAttribute(400, "La modification de la commande a échoué."));
gestionCommandes.MapDelete("/{id}", DeleteCommand)
    .WithMetadata(new SwaggerOperationAttribute(
        summary: "Supprime une commande",
        description: "Supression de la commande dans la base de données"))
    .WithMetadata(new SwaggerResponseAttribute(201, "La commande a bien été supprimée !"))
    .WithMetadata(new SwaggerResponseAttribute(400, "La suppression de la commande a échoué."));


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


// Modifier une commande par son ID
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
gestionPlats.MapGet("/", GetAllPlats)
    .WithMetadata(new SwaggerOperationAttribute(
        summary: "Récupère tous les éléments Plats",
        description: "Renvoie une liste de tous les éléments Plats."))
    .WithMetadata(new SwaggerResponseAttribute(200, "Liste de plats trouvés"))
    .WithMetadata(new SwaggerResponseAttribute(404, "Aucun plat n'a été trouvé."));
gestionPlats.MapGet("/{id}", GetPlatbyId)
    .WithMetadata(new SwaggerOperationAttribute(
        summary: "Récupère un plat par son identifiant",
        description: "Renvoie le plat pointé par son identifiant"))
    .WithMetadata(new SwaggerResponseAttribute(200, "Un plat correspondant"))
    .WithMetadata(new SwaggerResponseAttribute(404, "Aucun plat n'a été trouvé."));
gestionPlats.MapPost("/", CreatePlat)
    .WithMetadata(new SwaggerOperationAttribute(
        summary: "Crée un plat",
        description: "Ajoute un nouveau plat dans la base de données"))
    .WithMetadata(new SwaggerResponseAttribute(201, "Plat bien créé !"))
    .WithMetadata(new SwaggerResponseAttribute(400, "Requête invalide"));
gestionPlats.MapPut("/{id}", UpdatePlat)
    .WithMetadata(new SwaggerOperationAttribute(
        summary: "Modifie un plat",
        description: "Modification des informations d'un plat"))
    .WithMetadata(new SwaggerResponseAttribute(201, "Le plat a bien été modifié !"))
    .WithMetadata(new SwaggerResponseAttribute(400, "La modification du plat a échoué."));
gestionPlats.MapDelete("/{id}", DeletePlat)
    .WithMetadata(new SwaggerOperationAttribute(
        summary: "Supprime un plat",
        description: "Suppression d'un plat de la base de données"))
    .WithMetadata(new SwaggerResponseAttribute(201, "Le plat a bien été supprimé !"))
    .WithMetadata(new SwaggerResponseAttribute(400, "La suppression du plat a échoué."));

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