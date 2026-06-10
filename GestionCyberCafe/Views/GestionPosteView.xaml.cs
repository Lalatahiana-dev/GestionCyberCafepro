using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Gestion_CyberCafe.Data;
using Gestion_CyberCafe.ModelsR;
using Gestion_CyberCafe.Views.Poste;

namespace Gestion_CyberCafe.Views.Poste
{
    public partial class GestionPosteView : UserControl
    {
        private readonly GestionCyberContext _context;

        // ✔ FIX IMPORTANT: alias mba tsy hisy conflit
        private Gestion_CyberCafe.ModelsR.Poste selectedPoste;

        public GestionPosteView()
        {
            InitializeComponent();

            _context = new GestionCyberContext();

            ChargerPostes();
        }

        // =========================
        // CHARGER LISTE
        // =========================
        private void ChargerPostes()
        {
            dgPostes.ItemsSource = _context.Postes.ToList();
        }

        // =========================
        // AJOUT
        // =========================
        private void BtnAjouter_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNomPoste.Text))
            {
                MessageBox.Show("Nom poste obligatoire !");
                return;
            }

            var poste = new Gestion_CyberCafe.ModelsR.Poste
            {
                NomPoste = txtNomPoste.Text,
                Description = txtDescription.Text,
                Statut = ((ComboBoxItem)cbStatut.SelectedItem)?.Content.ToString()
            };

            _context.Postes.Add(poste);
            _context.SaveChanges();

            MessageBox.Show("Poste ajouté !");
            ClearForm();
            ChargerPostes();
        }

        // =========================
        // SELECTION DATAGRID
        // =========================
        private void dgPostes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedPoste = dgPostes.SelectedItem as Gestion_CyberCafe.ModelsR.Poste;

            if (selectedPoste != null)
            {
                txtNomPoste.Text = selectedPoste.NomPoste;
                txtDescription.Text = selectedPoste.Description;

                cbStatut.SelectedItem = cbStatut.Items
                    .Cast<ComboBoxItem>()
                    .FirstOrDefault(i => i.Content.ToString() == selectedPoste.Statut);
            }
        }

        // =========================
        // MODIFIER
        // =========================
        private void BtnModifier_Click(object sender, RoutedEventArgs e)
        {
            if (selectedPoste == null)
            {
                MessageBox.Show("Sélectionner un poste !");
                return;
            }

            var poste = _context.Postes.Find(selectedPoste.IdPoste);

            if (poste != null)
            {
                poste.NomPoste = txtNomPoste.Text;
                poste.Description = txtDescription.Text;
                poste.Statut = ((ComboBoxItem)cbStatut.SelectedItem)?.Content.ToString();

                _context.SaveChanges();

                MessageBox.Show("Poste modifié !");
                ClearForm();
                ChargerPostes();
            }
        }

        // =========================
        // SUPPRIMER
        // =========================
        private void BtnSupprimer_Click(object sender, RoutedEventArgs e)
        {
            if (selectedPoste == null)
            {
                MessageBox.Show("Sélectionner un poste !");
                return;
            }

            var poste = _context.Postes.Find(selectedPoste.IdPoste);

            if (poste != null)
            {
                _context.Postes.Remove(poste);
                _context.SaveChanges();

                MessageBox.Show("Poste supprimé !");
                ClearForm();
                ChargerPostes();
            }
        }

        // =========================
        // CLEAR FORM
        // =========================
        private void ClearForm()
        {
            txtNomPoste.Clear();
            txtDescription.Clear();
            cbStatut.SelectedIndex = -1;
            selectedPoste = null;
        }
    }
}