using System;
using System.Windows;
using System.Windows.Controls;
using Gestion_CyberCafe.Data;
using Gestion_CyberCafe.ModelsR;
using ClientModel = Gestion_CyberCafe.ModelsR.Client;

namespace Gestion_CyberCafe.Views.Wifi.Pages
{
    public partial class WifiClientView : UserControl
    {
        private readonly GestionCyberContext _context;
        private const decimal PRIX_HEURE = 1000m;

        public WifiClientView()
        {
            InitializeComponent();

            _context = new GestionCyberContext();

            txtHeure.TextChanged += Calcul;
            txtMinute.TextChanged += Calcul;

            txtMontant.Text = "0 Ar";
            txtHeureFin.Text = "--:--";
            txtDateDebut.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
        }

        // ================= CALCUL AUTO =================
        private void Calcul(object sender, TextChangedEventArgs e)
        {
            int heure = Parse(txtHeure.Text);
            int minute = Parse(txtMinute.Text);

            int totalMinutes = (heure * 60) + minute;

            // ================= MONTANT =================
            decimal montant = (totalMinutes * PRIX_HEURE) / 60m;
            txtMontant.Text = $"{montant:N0} Ar";

            // ================= HEURE FIN =================
            if (totalMinutes <= 0)
            {
                txtHeureFin.Text = "--:--";
                return;
            }

            DateTime heureFin = DateTime.Now.AddMinutes(totalMinutes);
            txtHeureFin.Text = heureFin.ToString("HH:mm");
        }

        // ================= ACTIVER CONNEXION =================
        private void BtnActiver_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // VALIDATION SIMPLE
                if (string.IsNullOrWhiteSpace(txtNom.Text))
                {
                    MessageBox.Show("Nom client obligatoire !");
                    return;
                }

                int heure = Parse(txtHeure.Text);
                int minute = Parse(txtMinute.Text);
                int totalMinutes = (heure * 60) + minute;

                if (totalMinutes <= 0)
                {
                    MessageBox.Show("Durée invalide !");
                    return;
                }

                // ================= CREATE CLIENT =================
                var client = new ClientModel
                {
                    Nom = txtNom.Text,
                    Telephone = txtTelephone.Text,
                    Statut = cbStatutClient.Text
                };

                _context.Clients.Add(client);
                _context.SaveChanges();

                // ================= CREATE SESSION WIFI =================
                var session = new SessionWifi
                {
                    IdClient = client.IdClient,
                    MarqueAppareil = txtMarque.Text,
                    TypeReseau = cbTypeReseau.Text,

                    HeureDebut = DateTime.Now,
                    DureeMinutes = totalMinutes,

                    MontantTotal = (totalMinutes * PRIX_HEURE) / 60m,

                    Statut = "ACTIVE",
                    IsDeleted = false
                };

                _context.SessionWifis.Add(session);
                _context.SaveChanges();

                MessageBox.Show("Connexion activée ✔");

                ResetForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur: " + ex.Message);
            }
        }

        // ================= RESET =================
        private void ResetForm()
        {
            txtNom.Clear();
            txtTelephone.Clear();
            txtMarque.Clear();
            txtHeure.Clear();
            txtMinute.Clear();

            txtMontant.Text = "0 Ar";
            txtHeureFin.Text = "--:--";
            txtDateDebut.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");

            cbStatutClient.SelectedIndex = -1;
            cbTypeReseau.SelectedIndex = -1;
        }

        // ================= SAFE PARSE =================
        private int Parse(string value)
        {
            return int.TryParse(value, out int r) ? r : 0;
        }
    }
}