using Gestion_CyberCafe.Data;
using Gestion_CyberCafe.ModelsR;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace GestionCyberCafe.Views
{
    public partial class AbonnementHistoryView : UserControl
    {
        private readonly GestionCyberContext _context;
        private List<AbonnementInternet> _abonnements;

        public AbonnementHistoryView()
        {
            InitializeComponent();
            _context = new GestionCyberContext();
            LoadHistorique();
        }

        // ================= LOAD =================
        private void LoadHistorique()
        {
            _abonnements = _context.AbonnementInternets
                .Include(a => a.Client)
                .OrderByDescending(a => a.DateDebut)
                .ToList();

            dgHistorique.ItemsSource = _abonnements;
            txtTotal.Text = $"{_abonnements.Count} abonnement(s)";
        }

        // ================= RECHERCHE TEXT =================
        private void txtRecherche_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
        }

        // ================= RECHERCHE DATE =================
        private void dpDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        // ================= FILTRE GLOBAL =================
        private void ApplyFilters()
        {
            if (_abonnements == null) return;

            string recherche = (txtRecherche.Text ?? "").ToLower().Trim();
            DateTime? date = dpDate.SelectedDate;

            var resultat = _abonnements.Where(a =>
            {
                bool matchText =
                    string.IsNullOrEmpty(recherche) ||
                    (a.Client?.Nom ?? "").ToLower().Contains(recherche) ||
                    (a.Client?.Prenom ?? "").ToLower().Contains(recherche) ||
                    (a.Client?.Telephone ?? "").ToLower().Contains(recherche) ||
                    (a.TypeForfait ?? "").ToLower().Contains(recherche) ||
                    (a.TypeAcces ?? "").ToLower().Contains(recherche) ||
                    (a.Statut ?? "").ToLower().Contains(recherche);

                bool matchDate =
                    !date.HasValue ||
                    (a.DateDebut.Date == date.Value.Date);

                return matchText && matchDate;
            })
            .ToList();

            dgHistorique.ItemsSource = resultat;
            txtTotal.Text = $"{resultat.Count} abonnement(s)";
        }

        // ================= REFRESH =================
        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            txtRecherche.Text = "";
            dpDate.SelectedDate = null;
            LoadHistorique();
        }

        // ================= VOIR =================
        private void BtnVoir_Click(object sender, RoutedEventArgs e)
        {
            var abonnement = GetItem(sender);
            if (abonnement == null) return;

            MessageBox.Show(
                $"Client : {abonnement.Client?.Nom} {abonnement.Client?.Prenom}\n" +
                $"Téléphone : {abonnement.Client?.Telephone}\n" +
                $"Accès : {abonnement.TypeAcces}\n" +
                $"Forfait : {abonnement.TypeForfait}\n" +
                $"Appareils : {abonnement.NombreAppareils}\n" +
                $"Début : {abonnement.DateDebut:dd/MM/yyyy HH:mm}\n" +
                $"Expiration : {abonnement.DateExpiration:dd/MM/yyyy HH:mm}\n" +
                $"Montant : {abonnement.Montant:N0} Ar\n" +
                $"Statut : {abonnement.Statut}",
                "Détails",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        // ================= DELETE =================
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var abonnement = GetItem(sender);
            if (abonnement == null) return;

            if (MessageBox.Show("Supprimer cet abonnement ?", "Confirm",
                MessageBoxButton.YesNo) != MessageBoxResult.Yes)
                return;

            abonnement.IsDeleted = true;
            abonnement.Statut = "SUPPRIME";

            _context.SaveChanges();
            LoadHistorique();
        }

        // ================= REACTIVER =================
        private void BtnReactiver_Click(object sender, RoutedEventArgs e)
        {
            var abonnement = GetItem(sender);
            if (abonnement == null) return;

            if (abonnement.Statut != "SUSPENDU")
            {
                MessageBox.Show("Seuls les abonnements suspendus peuvent être réactivés.");
                return;
            }

            if (MessageBox.Show("Réactiver cet abonnement ?", "Confirm",
                MessageBoxButton.YesNo) != MessageBoxResult.Yes)
                return;

            abonnement.Statut = "ACTIVE";

            _context.SaveChanges();
            LoadHistorique();
        }
        private void BtnRechercher_Click(object sender, RoutedEventArgs e)
        {
            ApplyFilters(); // na ApplySearch raha io no anaran'ny method-nao
        }
        // ================= HELPER =================
        private AbonnementInternet GetItem(object sender)
        {
            return (sender as FrameworkElement)?.DataContext as AbonnementInternet;
        }
    }
}