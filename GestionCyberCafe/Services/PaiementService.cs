using System;
using System.Linq;
using Gestion_CyberCafe.Data;
using Gestion_CyberCafe.ModelsR;

namespace Gestion_CyberCafe.Services
{
    public class PaiementService
    {
        private readonly GestionCyberContext _context;

        public PaiementService(GestionCyberContext context)
        {
            _context = context;
        }

        // ====================================
        // PAYER SESSION
        // ====================================
        public Paiement PayerSession(
            int idSession,
            string modePaiement)
        {
            var session = _context.SessionPostes
                .FirstOrDefault(s =>
                    s.IdSessionPoste == idSession);

            if (session == null)
                throw new Exception("Session introuvable");

            if (session.Statut != "EN_ATTENTE_PAIEMENT")
                throw new Exception("Session non payable");

            bool dejaPaye = _context.Paiements
                .Any(p =>
                    p.IdSessionPoste == idSession);

            if (dejaPaye)
                throw new Exception("Session déjà payée");

            var paiement = new Paiement
            {
                IdSessionPoste = idSession,
                Montant = session.MontantTotal,
                DatePaiement = DateTime.Now,
                Statut = "PAYE"
            };

            _context.Paiements.Add(paiement);

            session.Statut = "TERMINEE";

            _context.SaveChanges();

            return paiement;
        }

        // ====================================
        // SESSION PAYEE ?
        // ====================================
        public bool SessionEstPayee(
            int idSession)
        {
            return _context.Paiements
                .Any(p =>
                    p.IdSessionPoste == idSession);
        }

        // ====================================
        // TOTAL JOUR
        // ====================================
        public decimal RevenuJour()
        {
            DateTime today = DateTime.Today;

            return _context.Paiements
                .Where(p =>
                    p.DatePaiement >= today)
                .Sum(p => (decimal?)p.Montant)
                ?? 0;
        }

        // ====================================
        // TOTAL MOIS
        // ====================================
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

        // ====================================
        // LISTE PAIEMENTS
        // ====================================
        public System.Collections.Generic.List<Paiement>
            GetAll()
        {
            return _context.Paiements
                .OrderByDescending(p =>
                    p.DatePaiement)
                .ToList();
        }
    }
}