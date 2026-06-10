using System.ComponentModel.DataAnnotations;

namespace Gestion_CyberCafe.ModelsR
{
    public class Poste
    {
        [Key]
        public int IdPoste { get; set; }
        public string NomPoste { get; set; }
        public string Description { get; set; }
        public string Statut { get; set; } // LIBRE / OCCUPE / MAINTENANCE
    }
}