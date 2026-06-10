using System;
using System.Collections.Generic;
using System.Linq;
using Gestion_CyberCafe.Data;
using Gestion_CyberCafe.ModelsR;

namespace Gestion_CyberCafe.Services
{
    public class ClientService
    {
        private readonly GestionCyberContext _context;

        public ClientService(GestionCyberContext context)
        {
            _context = context;
        }

        // ==========================
        // AJOUT CLIENT
        // ==========================
        public Client AjouterClient(
            string nom,
            string prenom,
            string telephone,
            string adresse)
        {
            var client = new Client
            {
                Nom = nom,
                Prenom = prenom,
                Telephone = telephone,
                Adresse = adresse,
                Statut = "ACTIF"
            };

            _context.Clients.Add(client);
            _context.SaveChanges();

            return client;
        }

        // ==========================
        // MODIFIER CLIENT
        // ==========================
        public void ModifierClient(
            int idClient,
            string nom,
            string prenom,
            string telephone,
            string adresse)
        {
            var client = _context.Clients
                .Find(idClient);

            if (client == null)
                throw new Exception("Client introuvable");

            client.Nom = nom;
            client.Prenom = prenom;
            client.Telephone = telephone;
            client.Adresse = adresse;

            _context.SaveChanges();
        }

        // ==========================
        // DESACTIVER CLIENT
        // ==========================
        public void DesactiverClient(
            int idClient)
        {
            var client = _context.Clients
                .Find(idClient);

            if (client == null)
                throw new Exception("Client introuvable");

            client.Statut = "INACTIF";

            _context.SaveChanges();
        }

        // ==========================
        // ACTIVER CLIENT
        // ==========================
        public void ActiverClient(
            int idClient)
        {
            var client = _context.Clients
                .Find(idClient);

            if (client == null)
                throw new Exception("Client introuvable");

            client.Statut = "ACTIF";

            _context.SaveChanges();
        }

        // ==========================
        // RECHERCHE
        // ==========================
        public List<Client> Rechercher(
            string texte)
        {
            return _context.Clients
                .Where(c =>
                    c.Nom.Contains(texte)
                    || c.Prenom.Contains(texte)
                    || c.Telephone.Contains(texte))
                .OrderBy(c => c.Nom)
                .ToList();
        }

        // ==========================
        // TOUS LES CLIENTS
        // ==========================
        public List<Client> GetAll()
        {
            return _context.Clients
                .OrderBy(c => c.Nom)
                .ToList();
        }

        // ==========================
        // CLIENT PAR ID
        // ==========================
        public Client GetById(
            int idClient)
        {
            return _context.Clients
                .Find(idClient);
        }

        // ==========================
        // NOMBRE CLIENTS
        // ==========================
        public int NombreClients()
        {
            return _context.Clients.Count();
        }
    }
}