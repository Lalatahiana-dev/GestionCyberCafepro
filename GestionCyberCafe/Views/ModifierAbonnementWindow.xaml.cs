using Gestion_CyberCafe.Data;
using Gestion_CyberCafe.ModelsR;
using System;
using System.Data.Entity;
using System.Windows;

namespace Gestion_CyberCafe.Views
{
    public partial class ModifierAbonnementWindow : Window
    {
        private AbonnementInternet _abonnement;
        private GestionCyberContext _context;

        public ModifierAbonnementWindow(AbonnementInternet abonnement)
        {
            InitializeComponent();

            _context = new GestionCyberContext();
            _abonnement = abonnement;

            LoadData();
        }

        // ================= LOAD DATA =================
        private void LoadData()
        {
            txtPoste.Text = _abonnement.NombreAppareils.ToString();

            txtWifi.Text = "0";

            // Optionnel: afficher info
            this.Title = $"Modifier - {_abonnement.Client?.Nom}";
        }

        // ================= SAVE =================
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            int nbPoste = int.TryParse(txtPoste.Text, out var p) ? p : 0;
            int nbWifi = int.TryParse(txtWifi.Text, out var w) ? w : 0;

            // ✔ validation
            if (nbPoste == 0 && nbWifi == 0)
            {
                MessageBox.Show("Au moins 1 appareil requis !");
                return;
            }

            // ================= PRIX =================
            decimal prixPoste = 10000; // base simple (MOIS)
            decimal prixWifi = 5000;

            int totalAppareils = nbPoste + nbWifi;

            // ================= UPDATE ENTITY =================
            _abonnement.NombreAppareils = totalAppareils;

            _abonnement.Montant = (nbPoste * prixPoste) + (nbWifi * prixWifi);

            _abonnement.DateDebut = DateTime.Now;

            _abonnement.DateExpiration = DateTime.Now.AddMonths(1);

            _abonnement.Statut = "ACTIVE";

            // ================= SAVE DB =================
            var entity = _context.AbonnementInternets.Find(_abonnement.IdAbonnementInternet);

            if (entity != null)
            {
                entity.NombreAppareils = _abonnement.NombreAppareils;
                entity.Montant = _abonnement.Montant;
                entity.DateDebut = _abonnement.DateDebut;
                entity.DateExpiration = _abonnement.DateExpiration;
                entity.Statut = _abonnement.Statut;

                _context.SaveChanges();
            }

            MessageBox.Show("Modification réussie ✔");

            this.DialogResult = true;
            this.Close();
        }
    }
}