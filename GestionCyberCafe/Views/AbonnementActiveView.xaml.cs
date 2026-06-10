using Gestion_CyberCafe.Data;
using Gestion_CyberCafe.ModelsR;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Mapping;
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
            // ✔ STEP 1: check expiration
            CheckExpiration();

            _abonnements = _context.AbonnementInternets
                .Include(a => a.Client)
                .Where(a =>
                    !a.IsDeleted &&
                    a.Statut == "ACTIVE")
                .OrderByDescending(a => a.DateDebut)
                .ToList();

            dgAbonnements.ItemsSource = _abonnements;

            txtTotal.Text = $"{_abonnements.Count} abonnement(s)";
        }

        // ================= RECHERCHE =================

        private void TxtRecherche_TextChanged(object sender, TextChangedEventArgs e)
        {
            string recherche = txtRecherche.Text.ToLower();

            var resultat = _abonnements.Where(a =>
                a.Client != null &&
                (
                    a.Client.Nom.ToLower().Contains(recherche)
                    ||
                    a.Client.Prenom.ToLower().Contains(recherche)
                    ||
                    a.Client.Telephone.ToLower().Contains(recherche)
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

        private AbonnementInternet GetSelectedAbonnement(object sender)
        {
            return (sender as FrameworkElement)?.DataContext as AbonnementInternet;
        }
        //Voir details
        private void BtnVoir_Click(object sender, RoutedEventArgs e)
        {
            var item = GetSelectedAbonnement(sender);
            if (item == null) return;

            MessageBox.Show(
                $"Client: {item.Client?.Nom} {item.Client?.Prenom}\n" +
                $"Accès: {item.TypeAcces}\n" +
                $"Forfait: {item.TypeForfait}\n" +
                $"Montant: {item.Montant} Ar\n" +
                $"Expiration: {item.DateExpiration}"
            );
        }

        //Modification
        private void BtnModifier_Click(object sender, RoutedEventArgs e)
        {
            var abonnement =
                (sender as FrameworkElement)?.DataContext
                as AbonnementInternet;

            if (abonnement == null)
            {
                MessageBox.Show("Aucun abonnement sélectionné !");
                return;
            }

            var win = new ModifierAbonnementWindow(abonnement);
            win.ShowDialog();

            LoadAbonnements();
        }

        //Suspendre
        private void BtnSuspendre_Click(object sender, RoutedEventArgs e)
        {
            var abonnement =
                (sender as FrameworkElement)?.DataContext
                as AbonnementInternet;

            if (abonnement == null)
                return;
            // Vérifie statut
            if (abonnement.Statut != "SUSPENDU")
            {
                MessageBox.Show(
                    "Seuls les abonnements suspendus peuvent être réactivés.");
                return;
            }
            var result = MessageBox.Show(
                "Suspendre cet abonnement ?",
                "Confirmation",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes)
                return;

            abonnement.Statut = "SUSPENDU";

            _context.SaveChanges();

            MessageBox.Show(
                "Abonnement suspendu avec succès.",
                "Succès",
                MessageBoxButton.OK,
                MessageBoxImage.Information);

            LoadAbonnements();
        }

        //Suppression
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
        // Expire automatique
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
        // Renouvellement 
        private void BtnRenouveler_Click(object sender, RoutedEventArgs e)
        {
            var abonnement =
                (sender as FrameworkElement)?.DataContext
                as AbonnementInternet;

            if (abonnement == null)
                return;

            // ✔ confirmation
            var result = MessageBox.Show(
                "Renouveler cet abonnement ?",
                "Confirmation",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes)
                return;

            DateTime debut = DateTime.Now;

            // ✔ calcul base forfait
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

            // ✔ recalcul montant
            abonnement.Montant = prixBase * abonnement.NombreAppareils;

            // ✔ reactivate
            abonnement.Statut = "ACTIVE";

            abonnement.DateDebut = debut;

            _context.SaveChanges();

            MessageBox.Show("Abonnement renouvelé ✔");

            LoadAbonnements();
        }

        private void dgAbonnements_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = dgAbonnements.SelectedItem as AbonnementInternet;

            if (selected == null)
                return;

            // Exemple: affichage simple
            txtRecherche.Text = selected.Client?.Nom;
        }
    }
}