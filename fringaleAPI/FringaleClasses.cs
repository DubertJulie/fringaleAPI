namespace fringaleAPI
{
    public class Plat
    {
        public int Id_pl { get; set; }
        public required string Nom_pl { get; set; }
        public required double Prix_pl { get; set; }
        public string? Categorie_pl { get; set; } //vaut "Entrée", "Plat", "Dessert" ou "Boisson".
    }

    public class Client
    {
        public int Id_cl { get; set; }
        public required string Nom_cl { get; set; }
        public required string Prenom_cl { get; set; }
        public string? Adresse_cl { get; set; }
        public string? Telephone_cl { get; set; }
    }

    public class Commande
    {
        public int Id_co { get; set; }
        public required DateTime Date_co = DateTime.Now;
        public double Montant_co { get; set; }
    }
}
