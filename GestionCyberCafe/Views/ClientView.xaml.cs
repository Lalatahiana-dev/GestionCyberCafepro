using Gestion_CyberCafe.Data;
using Gestion_CyberCafe.ModelsR;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Gestion_CyberCafe.Views.Client
{
    public partial class ClientView : UserControl
    {
        private readonly GestionCyberContext _context;

        public ClientView()
        {
            InitializeComponent();
            _context = new GestionCyberContext();

            LoadClients();
        }

        // ================= LOAD =================
        private void LoadClients()
        {
            dgClients.ItemsSource = _context.Clients.ToList();
        }

        // ================= ADD CLIENT ONLY =================
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNom.Text))
            {
                MessageBox.Show("Nom obligatoire");
                return;
            }

            var client = new Gestion_CyberCafe.ModelsR.Client
            {
                Nom = txtNom.Text,
                Prenom = txtPrenom.Text,
                Telephone = txtTelephone.Text,
                Adresse = txtAdresse.Text,
                Statut = "Actif"
            };

            _context.Clients.Add(client);
            _context.SaveChanges();

            MessageBox.Show("Client ajouté ✔");

            ClearForm();
            LoadClients();
        }

        // ================= RESET =================
        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            txtNom.Clear();
            txtPrenom.Clear();
            txtTelephone.Clear();
            txtAdresse.Clear();
        }
    }
}