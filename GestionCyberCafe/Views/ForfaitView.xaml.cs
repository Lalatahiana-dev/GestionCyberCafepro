using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Gestion_CyberCafe.Data;
using Gestion_CyberCafe.ModelsR;
using ClientModel = Gestion_CyberCafe.ModelsR.Client;

namespace Gestion_CyberCafe.Views.Abonnement.Pages
{
    public partial class ForfaitView : UserControl
    {
        private readonly GestionCyberContext _context;
        private ClientModel selectedClient;


        public ForfaitView()
        {
            InitializeComponent();

            _context = new GestionCyberContext();

            txtDateDebut.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");

            LoadClients();
        }
        private void BtnResetClient_Click(object sender, RoutedEventArgs e)
        {
            ResetClientForm();
        }

        private void ResetClientForm()
        {
            txtNom.Text = "";
            txtPrenom.Text = "";
            txtTelephone.Text = "";
            txtAdresse.Text = "";

            txtClientSelectionne.Text = "";
            selectedClient = null;

            dgClients.SelectedItem = null;
        }

        // ================= LOAD CLIENTS =================
        private void LoadClients()
        {
            dgClients.ItemsSource = _context.Clients.ToList();
        }

        // ================= SEARCH CLIENT =================
        private void TxtSearchClient_TextChanged(object sender, TextChangedEventArgs e)
        {
            string search = txtSearchClient.Text.ToLower();

            dgClients.ItemsSource = _context.Clients
                .Where(c => c.Nom.ToLower().Contains(search)
                         || c.Prenom.ToLower().Contains(search)
                         || c.Telephone.Contains(search))
                .ToList();
        }


        // ================= AJOUT CLIENT =================
        private void BtnAjouterClient_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNom.Text) ||
                string.IsNullOrWhiteSpace(txtTelephone.Text))
            {
                MessageBox.Show("Nom sy Téléphone obligatoire !");
                return;
            }

            var client = new ClientModel
            {
                Nom = txtNom.Text.Trim(),
                Prenom = txtPrenom.Text.Trim(),
                Telephone = txtTelephone.Text.Trim(),
                Adresse = txtAdresse.Text.Trim(),
                Statut = "ACTIF"
            };

            _context.Clients.Add(client);
            _context.SaveChanges();

            LoadClients();

            MessageBox.Show("Client ajouté ✔");

            txtNom.Text = "";
            txtPrenom.Text = "";
            txtTelephone.Text = "";
            txtAdresse.Text = "";
        }
        // ================= Selection Client =================
        private void DgClients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedClient = dgClients.SelectedItem as ClientModel;

            if (selectedClient != null)
            {
                txtNom.Text = selectedClient.Nom;
                txtPrenom.Text = selectedClient.Prenom;
                txtTelephone.Text = selectedClient.Telephone;
                txtAdresse.Text = selectedClient.Adresse;

                txtClientSelectionne.Text =
                    selectedClient.Nom + " " + selectedClient.Prenom;
            }
        }

        // ================= Bouton edit =================
        private void BtnEditClient_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as FrameworkElement;
            var client = btn.DataContext as ClientModel;

            if (client == null) return;

            selectedClient = client;

            txtNom.Text = client.Nom;
            txtPrenom.Text = client.Prenom;
            txtTelephone.Text = client.Telephone;
            txtAdresse.Text = client.Adresse;
        }
        // ================= Bouton update =================
        private void BtnUpdateClient_Click(object sender, RoutedEventArgs e)
        {
            if (selectedClient == null)
            {
                MessageBox.Show("Sélectionnez un client !");
                return;
            }

            var client = _context.Clients.Find(selectedClient.IdClient);

            if (client != null)
            {
                client.Nom = txtNom.Text.Trim();
                client.Prenom = txtPrenom.Text.Trim();
                client.Telephone = txtTelephone.Text.Trim();
                client.Adresse = txtAdresse.Text.Trim();

                _context.SaveChanges();

                LoadClients();
                ResetClientForm();

                MessageBox.Show("Client modifié ✔");
            }
        }
        // ================= Bouton supprimer =================
        private void BtnDeleteClient_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as FrameworkElement;
            var client = btn.DataContext as ClientModel;

            if (client == null) return;

            var result = MessageBox.Show(
                "Voulez-vous supprimer ce client ?",
                "Confirmation",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                var c = _context.Clients.Find(client.IdClient);

                if (c != null)
                {
                    _context.Clients.Remove(c);
                    _context.SaveChanges();

                    LoadClients();
                    ResetClientForm();
                }
            }
        }
        // ================= CALCUL FORFAIT =================
        private void CalculerMontant()
        {
            if (cbForfait.SelectedItem == null)
                return;

            int nbPoste = int.TryParse(txtNbPoste.Text, out var p) ? p : 0;
            int nbWifi = int.TryParse(txtNbWifi.Text, out var w) ? w : 0;

            string type = ((ComboBoxItem)cbForfait.SelectedItem).Content.ToString();

            DateTime debut = DateTime.Now;
            DateTime expiration = debut;

            decimal prixPoste = 8000;
            decimal prixWifi = 5000;

            switch (type)
            {
                case "DEMI_JOURNEE":
                    expiration = debut.AddHours(12);
                    break;

                case "NUIT":
                    expiration = debut.AddHours(10);
                    break;

                case "JOURNEE":
                    expiration = debut.AddHours(24);
                    break;
            }

            decimal montant = (nbPoste * prixPoste) + (nbWifi * prixWifi);

            txtMontant.Text = montant.ToString("N0") + " Ar";
            txtExpiration.Text = expiration.ToString("dd/MM/yyyy HH:mm");
            txtDateDebut.Text = debut.ToString("dd/MM/yyyy HH:mm");
        }
        private void CbForfait_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CalculerMontant();
        }
        private void BtnActiver_Click(object sender, RoutedEventArgs e)
        {
            if (selectedClient == null)
            {
                MessageBox.Show("Sélectionnez un client !");
                return;
            }

            if (cbForfait.SelectedItem == null)
            {
                MessageBox.Show("Choisissez un forfait !");
                return;
            }

            string typeAcces = ((ComboBoxItem)cbTypeAcces.SelectedItem)?.Content.ToString();
            string typeForfait = ((ComboBoxItem)cbForfait.SelectedItem)?.Content.ToString();

            int nbPoste = int.TryParse(txtNbPoste.Text, out var p) ? p : 0;
            int nbWifi = int.TryParse(txtNbWifi.Text, out var w) ? w : 0;

            DateTime debut = DateTime.Now;
            DateTime expiration = debut;

            switch (typeForfait)
            {
                case "DEMI_JOURNEE":
                    expiration = debut.AddHours(12);
                    break;

                case "NUIT":
                    expiration = debut.AddHours(10);
                    break;

                case "JOURNEE":
                    expiration = debut.AddHours(24);
                    break;
            }

            decimal montant = (nbPoste * 8000) + (nbWifi * 5000);

            var abonnement = new AbonnementInternet
            {
                IdClient = selectedClient.IdClient,
                TypeAcces = typeAcces,
                TypeForfait = typeForfait,
                DateDebut = debut,
                DateExpiration = expiration,

                // ✔ PRO FIX
                NombreAppareils = nbPoste + nbWifi,

                Montant = montant,
                Statut = "ACTIVE",
                IsDeleted = false
            };

            _context.AbonnementInternets.Add(abonnement);
            _context.SaveChanges();

            MessageBox.Show("Abonnement activé ✔");

            ResetForm();
        }
        private void TxtNbPoste_TextChanged(object sender, TextChangedEventArgs e)
        {
            CalculerMontant();
        }

        private void TxtNbWifi_TextChanged(object sender, TextChangedEventArgs e)
        {
            CalculerMontant();
        }
        // ================= RESET =================
        private void ResetForm()
        {
            txtClientSelectionne.Text = "";
            selectedClient = null;

            cbForfait.SelectedIndex = -1;
            cbTypeAcces.SelectedIndex = -1;

            txtNbPoste.Text = "0";
            txtNbWifi.Text = "0";

            txtMontant.Text = "0 Ar";
            txtExpiration.Text = "";

            dgClients.SelectedItem = null;
        }
        // ===============Calcul montant=================

    }
}