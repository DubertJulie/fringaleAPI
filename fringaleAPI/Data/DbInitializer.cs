using System.Text.Json;
using fringaleAPI;

public static class DbInitializer
{

    public class ClientsJson
    {
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Adresse { get; set; }
        public string Phone { get; set; }
    }

    public class ArticlesJson
    {
        public string Nom { get; set; }
        public string Categorie { get; set; }
        public double Prix { get; set; }
    }


    public static void Seed(FringaleAPIDb context)
    {
        string basePath = "C:\\Users\\Utilisateur\\source\\repos\\fringaleAPI\\fringaleAPI\\JsonData\\";
        string clientsFilePath = Path.Combine(basePath, "clients.json");
        string articlesFilePath = Path.Combine(basePath, "articles.json");

        // Charger les clients
        if (!context.Clients.Any())
        {
            string clientsJson = File.ReadAllText(clientsFilePath);
            var jClients = JsonSerializer.Deserialize<List<ClientsJson>>(clientsJson);

            if (jClients != null)
            {
                var clients = jClients.Select(j => new Client
                {
                    Nom_cl = j.Nom,
                    Prenom_cl = j.Prenom,
                    Adresse_cl = j.Adresse,
                    Telephone_cl = j.Phone
                }).ToList();

                context.Clients.AddRange(clients);
                context.SaveChanges();
            }
        }

        // Charger les plats
        if (!context.Plats.Any())
        {
            string platsJson = File.ReadAllText(articlesFilePath);
            var plats = JsonSerializer.Deserialize<List<ArticlesJson>>(platsJson);

            if (plats != null)
            {
                context.Plats.AddRange(plats.Select(plat => new Plat
                {
                    Nom_pl = plat.Nom,
                    Prix_pl = plat.Prix,
                    Categorie_pl = plat.Categorie
                }));
                context.SaveChanges();
            }
        }
    }
}
