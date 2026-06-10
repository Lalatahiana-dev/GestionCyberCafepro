using System;
using System.ComponentModel.DataAnnotations;

namespace Gestion_CyberCafe.ModelsR
{
    public class Paiement
    {
        [Key]
        public int IdPaiement { get; set; }

        public int IdSessionPoste { get; set; }

        public decimal Montant { get; set; }

        public DateTime DatePaiement { get; set; }

        public string Statut { get; set; } // PAYE / NON PAYE
    }
}