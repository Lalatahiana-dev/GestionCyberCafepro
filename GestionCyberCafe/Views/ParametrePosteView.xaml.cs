using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Gestion_CyberCafe.Data;
using Gestion_CyberCafe.ModelsR;
using PosteModel = Gestion_CyberCafe.ModelsR.Poste;

namespace Gestion_CyberCafe.Views.Parametre
{
    public partial class ParametrePosteView : UserControl
    {
        private readonly GestionCyberContext _context;
        private PosteModel selectedPoste;

        public ParametrePosteView()
        {
            InitializeComponent();

            _context = new GestionCyberContext();

            ChargerPostes();
        }

        // ================= LOAD =================
        private void ChargerPostes()
        {
            dgPostes.ItemsSource =
                _context.Postes
                .OrderBy(p => p.NomPoste)
                .ToList();
        }

        // ================= ADD =================
        private void BtnAjouter_Click(object sender, RoutedEventArgs e)
        {
            PosteModel poste = new PosteModel
            {
                NomPoste = txtNomPoste.Text,
                Description = txtDescription.Text,
                Statut = ((ComboBoxItem)cbStatut.SelectedItem)?.Content.ToString()
            };

            _context.Postes.Add(poste);
            _context.SaveChanges();

            MessageBox.Show("Poste ajouté ✔");

            ChargerPostes();
            Clear();
        }

        // ================= UPDATE =================
        private void BtnModifier_Click(object sender, RoutedEventArgs e)
        {
            if (selectedPoste == null)
            {
                MessageBox.Show("Sélectionnez un poste.");
                return;
            }

            selectedPoste.NomPoste = txtNomPoste.Text;
            selectedPoste.Description = txtDescription.Text;
            selectedPoste.Statut = ((ComboBoxItem)cbStatut.SelectedItem)?.Content.ToString();

            _context.SaveChanges();

            MessageBox.Show("Poste modifié ✔");

            ChargerPostes();
            Clear();
        }

        // ================= DELETE =================
        private void BtnSupprimer_Click(object sender, RoutedEventArgs e)
        {
            if (selectedPoste == null)
            {
                MessageBox.Show("Sélectionnez un poste.");
                return;
            }

            _context.Postes.Remove(selectedPoste);
            _context.SaveChanges();

            MessageBox.Show("Poste supprimé ✔");

            ChargerPostes();
            Clear();
        }

        // ================= SELECT =================
        private void DgPostes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgPostes.SelectedItem is PosteModel poste)
            {
                selectedPoste = poste;

                txtNomPoste.Text = poste.NomPoste;
                txtDescription.Text = poste.Description;

                foreach (ComboBoxItem item in cbStatut.Items)
                {
                    if (item.Content.ToString() == poste.Statut)
                    {
                        cbStatut.SelectedItem = item;
                        break;
                    }
                }
            }
        }

        // ================= CLEAR =================
        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            Clear();
        }

        private void Clear()
        {
            selectedPoste = null;

            txtNomPoste.Clear();
            txtDescription.Clear();
            cbStatut.SelectedIndex = -1;

            dgPostes.SelectedItem = null;
        }
    }
}