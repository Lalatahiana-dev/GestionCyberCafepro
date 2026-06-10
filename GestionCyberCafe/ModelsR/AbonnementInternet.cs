using System;
using System.ComponentModel.DataAnnotations;

namespace Gestion_CyberCafe.ModelsR
{
    public class AbonnementInternet
    {
        [Key]
        public int IdAbonnementInternet { get; set; }

        // Client
        public int IdClient { get; set; }

        // WIFI / POSTE / WIFI+POSTE
        public string TypeAcces { get; set; }

        // DEMI_JOURNEE / NUIT / JOURNEE / MOIS / ANNEE
        public string TypeForfait { get; set; }

        // Début abonnement
        public DateTime DateDebut { get; set; }

        // Expiration calculée automatiquement
        public DateTime DateExpiration { get; set; }

        // Nombre appareils autorisés
        public int NombreAppareils { get; set; }

        // Durée personnalisée
        public int DureeMois { get; set; }

        public int DureeAnnees { get; set; }

        // Montant
        public decimal Montant { get; set; }

        // ACTIF / SUSPENDU / EXPIRE
        public string Statut { get; set; }

        // Soft Delete
        public bool IsDeleted { get; set; }
        public virtual Client Client { get; set; }
    }
}