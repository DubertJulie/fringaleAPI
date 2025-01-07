using Microsoft.EntityFrameworkCore;


namespace fringaleAPI
{
    public class Plat
    {
        public int id_me { get; set; }
        public required string? Nom_me { get; set; }
        public required int Prix_me { get; set; }
        public string? Categorie_me { get; set; }

    }    

    public class Commande
    {
        public int id_co { get; set; }
        public int Montant_co { get; set; }
        public DateTime Date_co = DateTime.Now;

    }   
    
    public class Client
    {
        public int? id_cl { get; set; }
        public required string? Nom_cl { get; set; }
        public required string? Prenom_cl { get; set; }
        public string? Adresse_cl { get; set; }
        public string? Telephone_cl { get; set; }

    }
}
