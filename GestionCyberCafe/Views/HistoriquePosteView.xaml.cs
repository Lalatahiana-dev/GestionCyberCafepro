using Gestion_CyberCafe.Data;
using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Data.Entity;
using System.Windows.Controls;

namespace Gestion_CyberCafe.Views.Poste
{
    public partial class HistoriquePosteView : UserControl
    {
        private readonly GestionCyberContext _context;

        public HistoriquePosteView()
        {
            InitializeComponent();

            _context = new GestionCyberContext();

            ChargerHistorique();
        }

        // ================= LOAD =================
        private void ChargerHistorique()
        {
            var data = from s in _context.SessionPostes
                       join p in _context.Postes on s.IdPoste equals p.IdPoste
                       join c in _context.Clients on s.IdClient equals c.IdClient

                       select new
                       {
                           IdSessionPoste = s.IdSessionPoste,
                           NomClient = c.Nom + " " + c.Prenom,
                           NomPoste = p.NomPoste,

                           HeureDebut = s.HeureDebut,
                           HeureFin = s.HeureFin,

                           DureeMinutes = s.DureeMinutes,
                           MontantTotal = s.MontantTotal + " Ar",

                           Statut = s.Statut
                       };

            dgHistorique.ItemsSource = data.ToList();
        }

        // ================= SEARCH =================
        private void BtnRechercher_Click(object sender, RoutedEventArgs e)
        {
            string key = txtSearch.Text?.Trim().ToLower();

            DateTime? dateDebut = dpDebut.SelectedDate;
            DateTime? dateFin = dpFin.SelectedDate;

            var query = from s in _context.SessionPostes
                        join p in _context.Postes on s.IdPoste equals p.IdPoste
                        join c in _context.Clients on s.IdClient equals c.IdClient
                        select new
                        {
                            s.IdSessionPoste,
                            Client = c.Nom + " " + c.Prenom,
                            Poste = p.NomPoste,
                            Statut = s.Statut,
                            Montant = s.MontantTotal,
                            s.HeureDebut,
                            s.HeureFin,
                            s.DureeMinutes
                        };

            // Recherche texte
            if (!string.IsNullOrWhiteSpace(key))
            {
                query = query.Where(x =>
                    x.Client.ToLower().Contains(key) ||
                    x.Poste.ToLower().Contains(key) ||
                    x.Statut.ToLower().Contains(key) ||
                    x.Montant.ToString().Contains(key));
            }

            // Date début uniquement
            if (dateDebut.HasValue && !dateFin.HasValue)
            {
                DateTime debut = dateDebut.Value.Date;

                query = query.Where(x =>
                    DbFunctions.TruncateTime(x.HeureDebut) >= debut);
            }

            // Date fin uniquement
            if (!dateDebut.HasValue && dateFin.HasValue)
            {
                DateTime fin = dateFin.Value.Date;

                query = query.Where(x =>
                    DbFunctions.TruncateTime(x.HeureDebut) <= fin);
            }

            // Date début + date fin
            if (dateDebut.HasValue && dateFin.HasValue)
            {
                DateTime debut = dateDebut.Value.Date;
                DateTime fin = dateFin.Value.Date;

                if (debut > fin)
                {
                    MessageBox.Show(
                        "La date début doit être inférieure à la date fin.",
                        "Erreur",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);

                    return;
                }

                query = query.Where(x =>
                    DbFunctions.TruncateTime(x.HeureDebut) >= debut &&
                    DbFunctions.TruncateTime(x.HeureDebut) <= fin);
            }

            dgHistorique.ItemsSource = query
                .ToList()
                .Select(x => new
                {
                    IdSessionPoste = x.IdSessionPoste,
                    NomClient = x.Client,
                    NomPoste = x.Poste,
                    HeureDebut = x.HeureDebut,
                    HeureFin = x.HeureFin,
                    DureeMinutes = x.DureeMinutes,
                    MontantTotal = x.Montant + " Ar",
                    Statut = x.Statut
                });
        }

        // ================= RESET =================
        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            txtSearch.Clear();

            dpDebut.SelectedDate = null;
            dpFin.SelectedDate = null;

            ChargerHistorique();
        }
        

        // ================= DELETE SINGLE =================
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as FrameworkElement;
            var row = btn.DataContext;

            dynamic item = row;

            int id = item.IdSessionPoste;

            var session = _context.SessionPostes
                .FirstOrDefault(s => s.IdSessionPoste == id);

            if (session == null)
            {
                MessageBox.Show("Session introuvable !");
                return;
            }

            if (MessageBox.Show("Supprimer cette session ?",
                "Confirmation",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                _context.SessionPostes.Remove(session);
                _context.SaveChanges();

                ChargerHistorique();

                MessageBox.Show("Supprimé ✔");
            }
        }
    }
}