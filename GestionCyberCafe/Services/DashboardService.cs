using System;
using System.Linq;
using Gestion_CyberCafe.Data;

namespace Gestion_CyberCafe.Services
{
    public class DashboardService
    {
        private readonly GestionCyberContext _context;

        public DashboardService(GestionCyberContext context)
        {
            _context = context;
        }

        // ==========================
        // SESSIONS ACTIVES
        // ==========================
        public int NombreSessionsActives()
        {
            return _context.SessionPostes
                .Count(s => s.Statut == "ACTIVE");
        }

        // ==========================
        // EN ATTENTE PAIEMENT
        // ==========================
        public int NombreEnAttentePaiement()
        {
            return _context.SessionPostes
                .Count(s => s.Statut == "EN_ATTENTE_PAIEMENT");
        }

        // ==========================
        // SESSIONS TERMINEES
        // ==========================
        public int NombreSessionsTerminees()
        {
            return _context.SessionPostes
                .Count(s => s.Statut == "TERMINEE");
        }

        // ==========================
        // POSTES LIBRES
        // ==========================
        public int NombrePostesLibres()
        {
            return _context.Postes
                .Count(p => p.Statut == "LIBRE");
        }

        // ==========================
        // POSTES OCCUPES
        // ==========================
        public int NombrePostesOccupes()
        {
            return _context.Postes
                .Count(p => p.Statut == "OCCUPE");
        }

        // ==========================
        // POSTES MAINTENANCE
        // ==========================
        public int NombrePostesMaintenance()
        {
            return _context.Postes
                .Count(p => p.Statut == "MAINTENANCE");
        }

        // ==========================
        // CLIENTS JOUR
        // ==========================
        public int NombreClientsJour()
        {
            DateTime today = DateTime.Today;

            return _context.SessionPostes
                .Where(s => s.HeureDebut >= today)
                .Select(s => s.IdClient)
                .Distinct()
                .Count();
        }

        // ==========================
        // REVENU JOUR
        // ==========================
        public decimal RevenuJour()
        {
            DateTime today = DateTime.Today;

            return _context.Paiements
                .Where(p => p.DatePaiement >= today)
                .Sum(p => (decimal?)p.Montant)
                ?? 0;
        }

        // ==========================
        // REVENU MOIS
        // ==========================
        public decimal RevenuMois()
        {
            int mois = DateTime.Now.Month;
            int annee = DateTime.Now.Year;

            return _context.Paiements
                .Where(p =>
                    p.DatePaiement.Month == mois &&
                    p.DatePaiement.Year == annee)
                .Sum(p => (decimal?)p.Montant)
                ?? 0;
        }
    }
}