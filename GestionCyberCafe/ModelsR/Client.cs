using System.ComponentModel.DataAnnotations;

namespace Gestion_CyberCafe.ModelsR
{
    public class Client
    {
        [Key]
        public int IdClient { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Telephone { get; set; }
        public string Adresse { get; set; }
        public string Statut { get; set; } // Actif / Inactif
    }
}