using System.ComponentModel.DataAnnotations;

namespace Gestion_CyberCafe.ModelsR
{
    public class Abonnement
    {
        [Key]
        public int IdAbonnement { get; set; }
        public string Nom { get; set; }
        public decimal Prix { get; set; }
        public int DureeMois { get; set; }
    }
}