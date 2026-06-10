using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Gestion_CyberCafe.Data;
using Gestion_CyberCafe.ModelsR;

namespace Gestion_CyberCafe.Views.Poste
{
    public partial class SessionPosteView : UserControl
    {
        private readonly GestionCyberContext _context;

        private const decimal PRIX_HEURE = 1000m;

        public SessionPosteView()
        {
            InitializeComponent();

            _context = new GestionCyberContext();

            dpDate.SelectedDate = DateTime.Now;
            txtHeureDebut.Text = DateTime.Now.ToString("HH:mm");

            LoadPostes();

            txtHeure.TextChanged += (s, e) => Calcul();
            txtMinute.TextChanged += (s, e) => Calcul();

            txtMontant.Text = "0 Ar";
            txtHeureFin.Text = "--:--";
        }

        // ================= CHARGER POSTES LIBRES =================
        private void LoadPostes()
        {
            cbPostes.ItemsSource = _context.Postes
                .Where(p => p.Statut == "LIBRE")
                .OrderBy(p => p.NomPoste)
                .ToList();

            cbPostes.SelectedIndex = -1;
        }

        // ================= CALCUL AUTOMATIQUE =================
        private void Calcul()
        {
            int h = Parse(txtHeure.Text);
            int m = Parse(txtMinute.Text);

            DateTime heureDebut = DateTime.Now;

            DateTime.TryParse(txtHeureDebut.Text, out heureDebut);

            DateTime heureFin = heureDebut
                .AddHours(h)
                .AddMinutes(m);

            txtHeureFin.Text = heureFin.ToString("HH:mm");

            int totalMinutes = (h * 60) + m;

            decimal montant =
                (totalMinutes * PRIX_HEURE) / 60m;

            txtMontant.Text = $"{montant:N0} Ar";
        }

        // ================= LANCER SESSION =================
        private void StartSession_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validation client
                if (string.IsNullOrWhiteSpace(txtNom.Text))
                {
                    MessageBox.Show("Le nom est obligatoire.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtPrenom.Text))
                {
                    MessageBox.Show("Le prénom est obligatoire.");
                    return;
                }

                // Validation poste
                if (cbPostes.SelectedValue == null)
                {
                    MessageBox.Show("Veuillez sélectionner un poste.");
                    return;
                }

                int heure = Parse(txtHeure.Text);
                int minute = Parse(txtMinute.Text);

                if (heure == 0 && minute == 0)
                {
                    MessageBox.Show("Veuillez saisir une durée.");
                    return;
                }

                int totalMinutes = (heure * 60) + minute;

                int idPoste = (int)cbPostes.SelectedValue;

                var poste = _context.Postes.Find(idPoste);

                if (poste == null)
                {
                    MessageBox.Show("Poste introuvable.");
                    return;
                }

                if (poste.Statut != "LIBRE")
                {
                    MessageBox.Show("Ce poste n'est plus disponible.");
                    return;
                }

                // ================= CLIENT =================
                var client = new Gestion_CyberCafe.ModelsR.Client
                {
                    Nom = txtNom.Text.Trim(),
                    Prenom = txtPrenom.Text.Trim(),
                    Telephone = txtTelephone.Text.Trim(),
                    Adresse = txtAdresse.Text.Trim(),
                    Statut = "ACTIF"
                };

                _context.Clients.Add(client);
                _context.SaveChanges();

                // ================= SESSION =================
                DateTime debut = DateTime.Now;

                var session = new SessionPoste
                {
                    IdClient = client.IdClient,
                    IdPoste = idPoste,

                    HeureDebut = debut,
                    HeureFin = debut.AddMinutes(totalMinutes),

                    DureeMinutes = totalMinutes,
                    ProlongationMinutes = 0,

                    MontantTotal =
                        (totalMinutes * PRIX_HEURE) / 60m,

                    Statut = "ACTIVE",
                    IsDeleted = false
                };

                _context.SessionPostes.Add(session);

                // ================= UPDATE POSTE =================
                poste.Statut = "OCCUPE";

                _context.SaveChanges();

                MessageBox.Show(
                    $"Session démarrée sur {poste.NomPoste} ✔",
                    "Succès",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                LoadPostes();
                Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Erreur",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        // ================= PARSE =================
        private int Parse(string value)
        {
            return int.TryParse(value, out int result)
                ? result
                : 0;
        }

        // ================= RESET FORM =================
        private void Clear()
        {
            txtNom.Clear();
            txtPrenom.Clear();
            txtTelephone.Clear();
            txtAdresse.Clear();

            txtHeure.Clear();
            txtMinute.Clear();

            txtMontant.Text = "0 Ar";
            txtHeureFin.Text = "--:--";

            txtHeureDebut.Text =
                DateTime.Now.ToString("HH:mm");

            cbPostes.SelectedIndex = -1;
        }
    }
}