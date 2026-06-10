using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using Gestion_CyberCafe.Data;
using Gestion_CyberCafe.ModelsR;

namespace Gestion_CyberCafe.Services
{
    public class SessionPosteService
    {
        private readonly GestionCyberContext _context;

        public SessionPosteService(GestionCyberContext context)
        {
            _context = context;
        }

        // =====================================
        // DEMARRER SESSION
        // =====================================
        public SessionPoste DemarrerSession(
            int idClient,
            int idPoste,
            int dureeMinutes)
        {
            var poste = _context.Postes.Find(idPoste);

            if (poste == null)
                throw new Exception("Poste introuvable");

            if (poste.Statut != "LIBRE")
                throw new Exception("Poste non disponible");

            var session = new SessionPoste
            {
                IdClient = idClient,
                IdPoste = idPoste,
                HeureDebut = DateTime.Now,
                DureeMinutes = dureeMinutes,
                ProlongationMinutes = 0,
                MontantTotal = 0,
                Statut = "ACTIVE",
                IsDeleted = false
            };

            poste.Statut = "OCCUPE";

            _context.SessionPostes.Add(session);
            _context.SaveChanges();

            return session;
        }

        // =====================================
        // PROLONGER SESSION
        // =====================================
        public void ProlongerSession(
            int idSession,
            int minutes)
        {
            var session = _context.SessionPostes
                .Find(idSession);

            if (session == null)
                throw new Exception("Session introuvable");

            if (session.Statut != "ACTIVE")
                throw new Exception("Session non active");

            session.ProlongationMinutes += minutes;

            _context.SaveChanges();
        }

        // =====================================
        // CALCUL MONTANT
        // =====================================
        public decimal CalculerMontant(
            int minutes)
        {
            var parametre =
                _context.Parametres.FirstOrDefault();

            decimal prixHeure =
                parametre?.PrixHeurePC ?? 1000;

            return (minutes * prixHeure) / 60m;
        }

        // =====================================
        // FIN SESSION
        // =====================================
        public decimal TerminerSession(
            int idSession)
        {
            var session = _context.SessionPostes
                .Include(s => s.Poste)
                .FirstOrDefault(
                    s => s.IdSessionPoste == idSession);

            if (session == null)
                throw new Exception("Session introuvable");

            if (session.Statut != "ACTIVE")
                throw new Exception("Session déjà terminée");

            session.HeureFin = DateTime.Now;

            int minutes =
                (int)Math.Ceiling(
                    (session.HeureFin.Value -
                     session.HeureDebut)
                    .TotalMinutes);

            decimal montant =
                CalculerMontant(minutes);

            session.MontantTotal = montant;

            session.Statut =
                "EN_ATTENTE_PAIEMENT";

            if (session.Poste != null)
            {
                session.Poste.Statut =
                    "LIBRE";
            }

            _context.SaveChanges();

            return montant;
        }

        // =====================================
        // SESSION ACTIVE ?
        // =====================================
        public bool SessionActiveExiste(
            int idClient)
        {
            return _context.SessionPostes
                .Any(s =>
                    s.IdClient == idClient &&
                    s.Statut == "ACTIVE");
        }

        // =====================================
        // LISTE COMPLETE
        // =====================================
        public List<SessionPoste> GetAll()
        {
            return _context.SessionPostes
                .Include(s => s.Client)
                .Include(s => s.Poste)
                .Where(s => !s.IsDeleted)
                .OrderByDescending(
                    s => s.IdSessionPoste)
                .ToList();
        }

        // =====================================
        // SESSION ACTIVE
        // =====================================
        public List<SessionPoste> GetSessionsActives()
        {
            return _context.SessionPostes
                .Include(s => s.Client)
                .Include(s => s.Poste)
                .Where(s =>
                    s.Statut == "ACTIVE" &&
                    !s.IsDeleted)
                .ToList();
        }

        // =====================================
        // SUPPRESSION LOGIQUE
        // =====================================
        public void Supprimer(int idSession)
        {
            var session =
                _context.SessionPostes.Find(idSession);

            if (session == null)
                return;

            session.IsDeleted = true;

            _context.SaveChanges();
        }
    }
}