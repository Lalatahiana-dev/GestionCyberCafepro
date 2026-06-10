using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Gestion_CyberCafe.Data;

namespace Gestion_CyberCafe.Views.Wifi
{
    public partial class WifiHistoryView : UserControl
    {
        private readonly GestionCyberContext _context;

        public WifiHistoryView()
        {
            InitializeComponent();

            _context = new GestionCyberContext();

            cbStatut.SelectedIndex = 0;

            LoadData();
        }

        // ================= LOAD =================
        private void LoadData()
        {
            var data = _context.SessionWifis
                .Where(s => !s.IsDeleted)
                .Select(s => new
                {
                    s.IdSessionWifi,
                    NomClient = s.Client.Nom + " " + s.Client.Prenom,
                    s.MarqueAppareil,
                    s.TypeReseau,
                    s.HeureDebut,
                    s.HeureFin,
                    s.DureeMinutes,
                    MontantTotal = s.MontantTotal + " Ar",
                    s.Statut
                })
                .ToList();

            dgWifiHistory.ItemsSource = data;
        }

        // ================= SEARCH =================
        private void BtnRechercher_Click(object sender, RoutedEventArgs e)
        {
            string key = txtSearch.Text?.ToLower();

            var query = _context.SessionWifis
                .Where(s => !s.IsDeleted)
                .Select(s => new
                {
                    s.IdSessionWifi,
                    NomClient = s.Client.Nom + " " + s.Client.Prenom,
                    s.MarqueAppareil,
                    s.TypeReseau,
                    s.HeureDebut,
                    s.HeureFin,
                    s.DureeMinutes,
                    s.MontantTotal,
                    s.Statut
                });

            if (!string.IsNullOrWhiteSpace(key))
            {
                query = query.Where(x =>
                    x.NomClient.ToLower().Contains(key) ||
                    x.MarqueAppareil.ToLower().Contains(key) ||
                    x.TypeReseau.ToLower().Contains(key) ||
                    x.Statut.ToLower().Contains(key)
                );
            }

            if (dpDebut.SelectedDate != null)
                query = query.Where(x => x.HeureDebut >= dpDebut.SelectedDate);

            if (dpFin.SelectedDate != null)
                query = query.Where(x => x.HeureDebut <= dpFin.SelectedDate);

            if (cbStatut.SelectedIndex > 0)
            {
                string statut = ((ComboBoxItem)cbStatut.SelectedItem).Content.ToString();
                query = query.Where(x => x.Statut == statut);
            }

            dgWifiHistory.ItemsSource = query.ToList()
                .Select(x => new
                {
                    x.IdSessionWifi,
                    x.NomClient,
                    x.MarqueAppareil,
                    x.TypeReseau,
                    x.HeureDebut,
                    x.HeureFin,
                    x.DureeMinutes,
                    MontantTotal = x.MontantTotal + " Ar",
                    x.Statut
                });
        }

        // ================= RESET =================
        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            txtSearch.Clear();
            dpDebut.SelectedDate = null;
            dpFin.SelectedDate = null;
            cbStatut.SelectedIndex = 0;

            LoadData();
        }

        // ================= DELETE SINGLE =================
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var row = (dynamic)((FrameworkElement)sender).DataContext;

            int id = row.IdSessionWifi;

            var item = _context.SessionWifis
                .FirstOrDefault(x => x.IdSessionWifi == id);

            if (item == null) return;

            item.IsDeleted = true;
            _context.SaveChanges();

            LoadData();
        }

        // ================= DELETE ALL =================
        private void BtnDeleteAll_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Supprimer tout l'historique ?",
                "Confirmation",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                var all = _context.SessionWifis.Where(x => !x.IsDeleted).ToList();

                foreach (var s in all)
                    s.IsDeleted = true;

                _context.SaveChanges();

                LoadData();
            }
        }
    }
}