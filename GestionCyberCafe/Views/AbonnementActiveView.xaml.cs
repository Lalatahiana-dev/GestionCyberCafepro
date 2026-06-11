using Gestion_CyberCafe.Data;
using Gestion_CyberCafe.ModelsR;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Gestion_CyberCafe.Views.Abonnement.Pages
{
    public partial class AbonnementActiveView : UserControl
    {
        private readonly GestionCyberContext _context;
        private List<AbonnementInternet> _abonnements;

        public AbonnementActiveView()
        {
            InitializeComponent();
            _context = new GestionCyberContext();
            LoadAbonnements();
        }

        // ================= LOAD =================
        private void LoadAbonnements()
        {
            CheckExpiration();

            _abonnements = _context.AbonnementInternets
                .Include(a => a.Client)
                .Where(a => !a.IsDeleted && a.Statut == "ACTIVE")
                .OrderByDescending(a => a.DateDebut)
                .ToList();

            dgAbonnements.ItemsSource = _abonnements;
            txtTotal.Text = $"{_abonnements.Count} abonnement(s)";
        }

        // ================= RECHERCHE =================
        private void BtnRechercher_Click(object sender, RoutedEventArgs e)
        {
            ApplySearch();
        }

        private void TxtRecherche_TextChanged(object sender, TextChangedEventArgs e)
        {
            // optional live search (si tianao instantané)
            // ApplySearch();
        }

        private void ApplySearch()
        {
            if (_abonnements == null) return;

            string recherche = (txtRecherche.Text ?? "").ToLower().Trim();

            var resultat = _abonnements.Where(a =>
                a.Client != null &&
                (
                    (a.Client.Nom ?? "").ToLower().Contains(recherche) ||
                    (a.Client.Prenom ?? "").ToLower().Contains(recherche) ||
                    (a.Client.Telephone ?? "").ToLower().Contains(recherche) ||
                    (a.TypeForfait ?? "").ToLower().Contains(recherche)
                ))
                .ToList();

            dgAbonnements.ItemsSource = resultat;
            txtTotal.Text = $"{resultat.Count} abonnement(s)";
        }

        // ================= ACTUALISER =================
        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            txtRecherche.Text = "";
            LoadAbonnements();
        }

        // ================= HELPERS =================
        private AbonnementInternet GetSelectedAbonnement(object sender)
        {
            return (sender as FrameworkElement)?.DataContext as AbonnementInternet;
        }

        // ================= VOIR =================
        private void BtnVoir_Click(object sender, RoutedEventArgs e)
        {
            var item = GetSelectedAbonnement(sender);
            if (item == null) return;

            MessageBox.Show(
                $"Client: {item.Client?.Nom} {item.Client?.Prenom}\n" +
                $"Téléphone: {item.Client?.Telephone}\n" +
                $"Accès: {item.TypeAcces}\n" +
                $"Forfait: {item.TypeForfait}\n" +
                $"Appareils: {item.NombreAppareils}\n" +
                $"Montant: {item.Montant} Ar\n" +
                $"Début: {item.DateDebut}\n" +
                $"Expiration: {item.DateExpiration}\n" +
                $"Statut: {item.Statut}"
            );
        }

        // ================= MODIFIER =================
        private void BtnModifier_Click(object sender, RoutedEventArgs e)
        {
            var abonnement = GetSelectedAbonnement(sender);

            if (abonnement == null)
            {
                MessageBox.Show("Aucun abonnement sélectionné !");
                return;
            }

            var win = new ModifierAbonnementWindow(abonnement);
            win.ShowDialog();

            LoadAbonnements();
        }

        // ================= SUSPENDRE (FIXED) =================
        private void BtnSuspendre_Click(object sender, RoutedEventArgs e)
        {
            var abonnement = GetSelectedAbonnement(sender);
            if (abonnement == null) return;

            var result = MessageBox.Show(
                "Voulez-vous suspendre cet abonnement ?",
                "Confirmation",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes)
                return;

            abonnement.Statut = "SUSPENDU";

            _context.SaveChanges();

            MessageBox.Show("Abonnement suspendu ✔");

            LoadAbonnements();
        }

        // ================= DELETE =================
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var item = GetSelectedAbonnement(sender);
            if (item == null) return;

            if (MessageBox.Show("Supprimer cet abonnement ?", "Confirm",
                MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                item.IsDeleted = true;
                _context.SaveChanges();
                LoadAbonnements();
            }
        }

        // ================= EXPIRATION AUTO =================
        private void CheckExpiration()
        {
            var abonnements = _context.AbonnementInternets
                .Where(a => a.Statut == "ACTIVE")
                .ToList();

            bool updated = false;

            foreach (var a in abonnements)
            {
                if (a.DateExpiration < DateTime.Now)
                {
                    a.Statut = "EXPIRE";
                    updated = true;
                }
            }

            if (updated)
                _context.SaveChanges();
        }

        // ================= RENOUVELER =================
        private void BtnRenouveler_Click(object sender, RoutedEventArgs e)
        {
            var abonnement = GetSelectedAbonnement(sender);
            if (abonnement == null) return;

            var result = MessageBox.Show(
                "Renouveler cet abonnement ?",
                "Confirmation",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes)
                return;

            DateTime debut = DateTime.Now;
            decimal prixBase = 0;

            switch (abonnement.TypeForfait)
            {
                case "DEMI_JOURNEE":
                    prixBase = 5000;
                    abonnement.DateExpiration = debut.AddHours(12);
                    break;

                case "NUIT":
                    prixBase = 8000;
                    abonnement.DateExpiration = debut.AddHours(18);
                    break;

                case "JOURNEE":
                    prixBase = 12000;
                    abonnement.DateExpiration = debut.AddHours(24);
                    break;

                case "SEMAINE":
                    prixBase = 10000;
                    abonnement.DateExpiration = debut.AddDays(7);
                    break;

                case "MOIS":
                    prixBase = 30000;
                    abonnement.DateExpiration = debut.AddMonths(1);
                    break;

                case "ANNEE":
                    prixBase = 300000;
                    abonnement.DateExpiration = debut.AddYears(1);
                    break;
            }

            abonnement.Montant = prixBase * abonnement.NombreAppareils;
            abonnement.DateDebut = debut;
            abonnement.Statut = "ACTIVE";

            _context.SaveChanges();

            MessageBox.Show("Abonnement renouvelé ✔");

            LoadAbonnements();
        }

        // ================= OPTIONAL UI =================
        private void dgAbonnements_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = dgAbonnements.SelectedItem as AbonnementInternet;

            if (selected != null && !string.IsNullOrEmpty(selected.Client?.Nom))
            {
                txtRecherche.Text = selected.Client.Nom;
            }
        }
    }
}