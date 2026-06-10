using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Gestion_CyberCafe.Data;

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

            if (!string.IsNullOrWhiteSpace(key))
            {
                query = query.Where(x =>
                    x.Client.ToLower().Contains(key) ||
                    x.Poste.ToLower().Contains(key) ||
                    x.Statut.ToLower().Contains(key) ||
                    x.Montant.ToString().Contains(key)
                );
            }

            dgHistorique.ItemsSource = query.Select(x => new
            {
                IdSessionPoste = x.IdSessionPoste,

                NomClient = x.Client,
                NomPoste = x.Poste,

                HeureDebut = x.HeureDebut,
                HeureFin = x.HeureFin,

                DureeMinutes = x.DureeMinutes,

                MontantTotal = x.Montant + " Ar",

                Statut = x.Statut
            }).ToList();
        }

        // ================= RESET =================
        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            txtSearch.Clear();
            ChargerHistorique();
        }

        // ================= DELETE ALL =================
        private void BtnSupprimerTous_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Voulez-vous supprimer tout l'historique ?",
                "Confirmation",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                var all = _context.SessionPostes.ToList();

                _context.SessionPostes.RemoveRange(all);
                _context.SaveChanges();

                ChargerHistorique();

                MessageBox.Show("Historique supprimé ✔");
            }
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