using Gestion_CyberCafe.Data;
using Gestion_CyberCafe.ModelsR;
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

        // ================= RECHERCHE =================

        private void txtRecherche_TextChanged(object sender, TextChangedEventArgs e)
        {
            string recherche = txtRecherche.Text.ToLower();

            var resultat = _abonnements
                .Where(a =>
                    a.Client != null &&
                    (
                        a.Client.Nom.ToLower().Contains(recherche)
                        ||
                        a.Client.Prenom.ToLower().Contains(recherche)
                        ||
                        a.Client.Telephone.ToLower().Contains(recherche)
                        ||
                        a.TypeForfait.ToLower().Contains(recherche)
                        ||
                        a.Statut.ToLower().Contains(recherche)
                    ))
                .ToList();

            dgHistorique.ItemsSource = resultat;

            txtTotal.Text = $"{resultat.Count} abonnement(s)";
        }

        // ================= REFRESH =================

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            txtRecherche.Text = "";

            LoadHistorique();
        }

        // ================= VOIR DETAILS =================

        private void BtnVoir_Click(object sender, RoutedEventArgs e)
        {
            var abonnement =
                (sender as FrameworkElement)?.DataContext
                as AbonnementInternet;

            if (abonnement == null)
                return;

            MessageBox.Show(
                $"Client : {abonnement.Client?.Nom} {abonnement.Client?.Prenom}\n\n" +
                $"Téléphone : {abonnement.Client?.Telephone}\n\n" +
                $"Type Accès : {abonnement.TypeAcces}\n" +
                $"Forfait : {abonnement.TypeForfait}\n" +
                $"Appareils : {abonnement.NombreAppareils}\n\n" +
                $"Début : {abonnement.DateDebut:dd/MM/yyyy HH:mm}\n" +
                $"Expiration : {abonnement.DateExpiration:dd/MM/yyyy HH:mm}\n\n" +
                $"Montant : {abonnement.Montant:N0} Ar\n" +
                $"Statut : {abonnement.Statut}\n" +
                $"Supprimé : {(abonnement.IsDeleted ? "Oui" : "Non")}",
                "Détails abonnement",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
        // Bouton delete
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var abonnement =
                (sender as FrameworkElement)?.DataContext
                as AbonnementInternet;

            if (abonnement == null)
            {
                MessageBox.Show("Aucun abonnement sélectionné !");
                return;
            }

            var result = MessageBox.Show(
                "Voulez-vous supprimer cet abonnement ?",
                "Confirmation",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes)
                return;

            // ✔ Soft delete
            abonnement.IsDeleted = true;
            abonnement.Statut = "SUPPRIME";

            _context.SaveChanges();

            MessageBox.Show("Abonnement supprimé ✔");

            LoadHistorique();
        }
        // Reactivité
        private void BtnReactiver_Click(object sender, RoutedEventArgs e)
        {
            var abonnement =
                (sender as FrameworkElement)?.DataContext
                as AbonnementInternet;

            if (abonnement == null)
                return;

            if (abonnement.Statut != "SUSPENDU")
            {
                MessageBox.Show(
                    "Seuls les abonnements suspendus peuvent être réactivés.");
                return;
            }

            var result = MessageBox.Show(
                "Réactiver cet abonnement ?",
                "Confirmation",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes)
                return;

            abonnement.Statut = "ACTIVE";

            _context.SaveChanges();

            MessageBox.Show(
                "Abonnement réactivé avec succès.",
                "Succès",
                MessageBoxButton.OK,
                MessageBoxImage.Information);

            LoadHistorique();
        }
    }
}