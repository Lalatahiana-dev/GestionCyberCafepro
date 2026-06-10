using System.ComponentModel.DataAnnotations;

namespace Gestion_CyberCafe.ModelsR
{
    public class ParametreConfig
    {
        [Key]
        public int IdParametre { get; set; }

        public string NomCyber { get; set; }
        public string Adresse { get; set; }
        public string Telephone { get; set; }

        public decimal PrixHeurePC { get; set; }
        public decimal PrixMinutePC { get; set; }

        public decimal PrixHeureWifi { get; set; }
        public decimal PrixMinuteWifi { get; set; }
        public decimal PrixSemainePoste { get; set; }
        public decimal PrixMoisPoste { get; set; }
        public decimal PrixAnneePoste { get; set; }
        public decimal PrixSemaineWifi { get; set; }
        public decimal PrixMoisWifi { get; set; }
        public decimal PrixAnneeWifi { get; set; }
    }
}