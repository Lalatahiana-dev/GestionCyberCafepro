using System;
using System.ComponentModel.DataAnnotations;
using Gestion_CyberCafe.ModelsR;

namespace Gestion_CyberCafe.ModelsR
{
    public class SessionWifi
    {
        [Key]
        public int IdSessionWifi { get; set; }

        public int IdClient { get; set; }
        public Client Client { get; set; }

        public string MarqueAppareil { get; set; }
        public string TypeReseau { get; set; }

        public DateTime HeureDebut { get; set; }
        public DateTime? HeureFin { get; set; }

        public int DureeMinutes { get; set; }
        public decimal MontantTotal { get; set; }

        public bool IsDeleted { get; set; } = false;
        public int? TempsRestantMinutes { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string Statut { get; set; }
    }
}