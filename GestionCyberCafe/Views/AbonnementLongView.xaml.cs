using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Gestion_CyberCafe.Data;
using Gestion_CyberCafe.ModelsR;
using ClientModel = Gestion_CyberCafe.ModelsR.Client;

namespace Gestion_CyberCafe.Views.Abonnement.Pages
{
    public partial class AbonnementLongView : UserControl
    {
        private readonly GestionCyberContext _context;
        private ClientModel selectedClient;

        public AbonnementLongView()
        {
            InitializeComponent();
            Loaded += AbonnementLongView_Loaded;

            _context = new GestionCyberContext();

            LoadClients();
        }

        // ================= LOAD CLIENTS =================
        private void LoadClients()
        {
            dgClients.ItemsSource = _context.Clients.ToList();
        }
       
        private void BtnEditClient_Click(object sender, RoutedEventArgs e)
        {
            BtnUpdateClient_Click(sender, e);
        }
        // ================= SEARCH =================
        private void TxtSearchClient_TextChanged(object sender, TextChangedEventArgs e)
        {
            string search = txtSearchClient.Text.ToLower();

            dgClients.ItemsSource = _context.Clients
                .Where(c => c.Nom.ToLower().Contains(search)
                         || c.Prenom.ToLower().Contains(search)
                         || c.Telephone.Contains(search))
                .ToList();
        }

        // ================= SELECT CLIENT =================
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

        // ================= ADD CLIENT =================
        private void BtnAjouterClient_Click(object sender, RoutedEventArgs e)
        {
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
            ClearClientForm();

            MessageBox.Show("Client ajouté ✔");
        }

        // ================= UPDATE CLIENT =================
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
                ClearClientForm();

                MessageBox.Show("Client modifié ✔");
            }
        }

        // ================= DELETE CLIENT =================
        private void BtnDeleteClient_Click(object sender, RoutedEventArgs e)
        {
            var client = dgClients.SelectedItem as ClientModel;

            if (client == null)
            {
                MessageBox.Show("Sélectionnez un client !");
                return;
            }

            var result = MessageBox.Show(
                "Supprimer ce client ?",
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
                    ClearClientForm();
                }
            }
        }

       
        // ================= FORFAIT CHANGE =================
        private void CbForfait_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CalculerMontant();
        }
        private void TxtNbPoste_TextChanged(object sender, TextChangedEventArgs e)
        {
            CalculerMontant();
        }
        private void TxtNbWifi_TextChanged(object sender, TextChangedEventArgs e)
        {
            CalculerMontant();
        }
        private void AbonnementLongView_Loaded(object sender, RoutedEventArgs e)
        {
            CalculerMontant();
        }

        // ================= CALCUL CENTRALISE =================
        private void CalculerMontant()
        {
            if (cbForfait == null || cbForfait.SelectedItem == null)
                return;

            int nbPoste = int.TryParse(txtNbPoste.Text, out var p) ? p : 0;
            int nbWifi = int.TryParse(txtNbWifi.Text, out var w) ? w : 0;

            string forfait =
                ((ComboBoxItem)cbForfait.SelectedItem).Content.ToString();

            decimal prixPoste = 0;
            decimal prixWifi = 0;

            DateTime debut = DateTime.Now;
            DateTime expiration = debut;

            var param = _context.Parametres.FirstOrDefault();

switch (forfait)
{
    case "SEMAINE":
        prixPoste = param.PrixSemainePoste;
        prixWifi = param.PrixSemaineWifi;
        expiration = debut.AddDays(7);
        break;

    case "MOIS":
        prixPoste = param.PrixMoisPoste;
        prixWifi = param.PrixMoisWifi;
        expiration = debut.AddMonths(1);
        break;

    case "ANNEE":
        prixPoste = param.PrixAnneePoste;
        prixWifi = param.PrixAnneeWifi;
        expiration = debut.AddYears(1);
        break;
}

            decimal montant =
                (nbPoste * prixPoste) +
                (nbWifi * prixWifi);

            txtMontant.Text = montant.ToString("N0") + " Ar";
            txtExpiration.Text = expiration.ToString("dd/MM/yyyy");
        }
        // ================= ACTIVER ABONNEMENT =================
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

            int nbPoste = int.TryParse(txtNbPoste.Text, out var p) ? p : 0;
            int nbWifi = int.TryParse(txtNbWifi.Text, out var w) ? w : 0;

            // ✔ VALIDATION
            if (nbPoste == 0 && nbWifi == 0)
            {
                MessageBox.Show("Entrez au moins 1 poste ou 1 wifi !");
                return;
            }
            string typeAcces =
                ((ComboBoxItem)cbTypeAcces.SelectedItem)?.Content.ToString();

            string typeForfait =
                ((ComboBoxItem)cbForfait.SelectedItem)?.Content.ToString();

            DateTime debut = DateTime.Now;
            DateTime expiration = debut;

            decimal prixPoste = 0;
            decimal prixWifi = 0;

            var param = _context.Parametres.FirstOrDefault();

            switch (typeForfait)
            {
                case "SEMAINE":
                    prixPoste = param.PrixSemainePoste;
                    prixWifi = param.PrixSemaineWifi;
                    expiration = debut.AddDays(7);
                    break;

                case "MOIS":
                    prixPoste = param.PrixMoisPoste;
                    prixWifi = param.PrixMoisWifi;
                    expiration = debut.AddMonths(1);
                    break;

                case "ANNEE":
                    prixPoste = param.PrixAnneePoste;
                    prixWifi = param.PrixAnneeWifi;
                    expiration = debut.AddYears(1);
                    break;
            }

            decimal montant =
                (nbPoste * prixPoste) +
                (nbWifi * prixWifi);

            var abonnement = new AbonnementInternet
            {
                IdClient = selectedClient.IdClient,
                TypeAcces = typeAcces,
                TypeForfait = typeForfait,

                DateDebut = debut,
                DateExpiration = expiration,

                NombreAppareils = nbPoste + nbWifi,

                DureeMois = 0,
                DureeAnnees = 0,

                Montant = montant,

                Statut = "ACTIVE",
                IsDeleted = false
            };

            _context.AbonnementInternets.Add(abonnement);
            _context.SaveChanges();

            MessageBox.Show("Abonnement long activé ✔");

            ClearClientForm();
        }

        // ================= CLEAR FORM =================
        private void ClearClientForm()
        {
            txtNom.Text = "";
            txtPrenom.Text = "";
            txtTelephone.Text = "";
            txtAdresse.Text = "";

            txtNbPoste.Text = "0";
            txtNbWifi.Text = "0";

            txtClientSelectionne.Text = "";
            txtMontant.Text = "0 Ar";
            txtExpiration.Text = "";

            cbForfait.SelectedIndex = -1;
            cbTypeAcces.SelectedIndex = -1;

            selectedClient = null;
        }
    }
}