using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gestion_CyberCafe.ModelsR
{
    public class SessionPoste
    {
        [Key]
        public int IdSessionPoste { get; set; }

        public int IdClient { get; set; }

        [ForeignKey("IdClient")]
        public virtual Client Client { get; set; }

        public int IdPoste { get; set; }

        [ForeignKey("IdPoste")]
        public virtual Poste Poste { get; set; }

        public DateTime HeureDebut { get; set; }
        public DateTime? HeureFin { get; set; }

        public int DureeMinutes { get; set; }
        public int ProlongationMinutes { get; set; }

        public decimal MontantTotal { get; set; }

        public string Statut { get; set; }
        // ACTIVE
        // TERMINEE
        // EN_ATTENTE_PAIEMENT

        public bool IsDeleted { get; set; } = false;
    }
}