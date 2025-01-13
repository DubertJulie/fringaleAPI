using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


namespace fringaleAPI
{
    public class Plat
    {
        [Key]
        public int Id_pl { get; set; } // clé primaire
        public required string Nom_pl { get; set; }
        public required double Prix_pl { get; set; }
        public string? Categorie_pl { get; set; } // vaut "Entrée", "Plat", "Dessert" ou "Boisson".

    }

    public class Commande
    {
        [Key]
        public int Id_co { get; set; } // clé primaire
        public double Montant_co { get; set; }
        public DateTime Date_co { get; set; } = DateTime.Now;
        public required int Id_cl { get; set; } // clé vers client

        //Liste des plats :
        public ICollection<PlatParCommande>? PlatParCommandes { get; set; }

        
    }

    public class Client
    {
        [Key]
        public int Id_cl { get; set; } // clé primaire
        public required string? Nom_cl { get; set; }
        public required string? Prenom_cl { get; set; }
        public string? Adresse_cl { get; set; }
        public string? Telephone_cl { get; set; }

    }

    public class PlatParCommande //table d'association Plat/Commande
    {
        [Key]
        public int Id { get; set; }

        public int Id_co { get; set; }
        public int Id_pl { get; set; }
    }
}