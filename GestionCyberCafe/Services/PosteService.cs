using System;
using System.Collections.Generic;
using System.Linq;
using Gestion_CyberCafe.Data;
using Gestion_CyberCafe.ModelsR;

namespace Gestion_CyberCafe.Services
{
    public class PosteService
    {
        private readonly GestionCyberContext _context;

        public PosteService(GestionCyberContext context)
        {
            _context = context;
        }

        // ==========================
        // AJOUT POSTE
        // ==========================
        public Poste AjouterPoste(
            string nomPoste,
            string description)
        {
            bool existe = _context.Postes
                .Any(p => p.NomPoste == nomPoste);

            if (existe)
                throw new Exception("Ce poste existe déjà");

            var poste = new Poste
            {
                NomPoste = nomPoste,
                Description = description,
                Statut = "LIBRE"
            };

            _context.Postes.Add(poste);
            _context.SaveChanges();

            return poste;
        }

        // ==========================
        // MODIFIER POSTE
        // ==========================
        public void ModifierPoste(
            int idPoste,
            string nomPoste,
            string description)
        {
            var poste = _context.Postes.Find(idPoste);

            if (poste == null)
                throw new Exception("Poste introuvable");

            poste.NomPoste = nomPoste;
            poste.Description = description;

            _context.SaveChanges();
        }

        // ==========================
        // MAINTENANCE
        // ==========================
        public void MettreEnMaintenance(
            int idPoste)
        {
            var poste = _context.Postes.Find(idPoste);

            if (poste == null)
                throw new Exception("Poste introuvable");

            if (poste.Statut == "OCCUPE")
                throw new Exception("Poste actuellement occupé");

            poste.Statut = "MAINTENANCE";

            _context.SaveChanges();
        }

        // ==========================
        // RETOUR LIBRE
        // ==========================
        public void RemettreLibre(
            int idPoste)
        {
            var poste = _context.Postes.Find(idPoste);

            if (poste == null)
                throw new Exception("Poste introuvable");

            poste.Statut = "LIBRE";

            _context.SaveChanges();
        }

        // ==========================
        // RECHERCHE
        // ==========================
        public List<Poste> Rechercher(
            string texte)
        {
            return _context.Postes
                .Where(p =>
                    p.NomPoste.Contains(texte)
                    || p.Description.Contains(texte))
                .OrderBy(p => p.NomPoste)
                .ToList();
        }

        // ==========================
        // TOUS LES POSTES
        // ==========================
        public List<Poste> GetAll()
        {
            return _context.Postes
                .OrderBy(p => p.NomPoste)
                .ToList();
        }

        // ==========================
        // POSTES LIBRES
        // ==========================
        public List<Poste> GetPostesLibres()
        {
            return _context.Postes
                .Where(p => p.Statut == "LIBRE")
                .ToList();
        }

        // ==========================
        // COMPTEURS DASHBOARD
        // ==========================
        public int NombreLibres()
        {
            return _context.Postes
                .Count(p => p.Statut == "LIBRE");
        }

        public int NombreOccupes()
        {
            return _context.Postes
                .Count(p => p.Statut == "OCCUPE");
        }

        public int NombreMaintenance()
        {
            return _context.Postes
                .Count(p => p.Statut == "MAINTENANCE");
        }
    }
}