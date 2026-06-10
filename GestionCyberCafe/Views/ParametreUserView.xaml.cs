using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Gestion_CyberCafe.Data;
using Gestion_CyberCafe.ModelsR;

namespace Gestion_CyberCafe.Views.Parametre
{
    public partial class ParametreUserView : UserControl
    {
        private readonly GestionCyberContext _context;

        private User selectedUser;

        public ParametreUserView()
        {
            InitializeComponent();

            _context = new GestionCyberContext();

            ChargerUsers();
        }

        // ================= CHARGER =================

        private void ChargerUsers()
        {
            dgUsers.ItemsSource = null;
            dgUsers.ItemsSource =
                _context.Users
                .OrderBy(u => u.Username)
                .ToList();
        }

        // ================= AJOUTER =================

        private void BtnAjouter_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                MessageBox.Show("Nom utilisateur obligatoire.");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Password))
            {
                MessageBox.Show("Mot de passe obligatoire.");
                return;
            }

            if (cbRole.SelectedItem == null)
            {
                MessageBox.Show("Sélectionnez un rôle.");
                return;
            }

            string role =
                ((ComboBoxItem)cbRole.SelectedItem)
                .Content
                .ToString();

            User user = new User
            {
                Username = txtUsername.Text.Trim(),
                Password = txtPassword.Password,
                Role = role
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            MessageBox.Show("Utilisateur ajouté ✔");

            ChargerUsers();
            ClearForm();
        }

        // ================= MODIFIER =================

        private void BtnModifier_Click(object sender, RoutedEventArgs e)
        {
            if (selectedUser == null)
            {
                MessageBox.Show("Sélectionnez un utilisateur.");
                return;
            }

            selectedUser.Username = txtUsername.Text.Trim();

            if (!string.IsNullOrWhiteSpace(txtPassword.Password))
            {
                selectedUser.Password = txtPassword.Password;
            }

            selectedUser.Role =
                ((ComboBoxItem)cbRole.SelectedItem)
                .Content
                .ToString();

            _context.SaveChanges();

            MessageBox.Show("Utilisateur modifié ✔");

            ChargerUsers();
            ClearForm();
        }

        // ================= SUPPRIMER =================

        private void BtnSupprimer_Click(object sender, RoutedEventArgs e)
        {
            if (selectedUser == null)
            {
                MessageBox.Show("Sélectionnez un utilisateur.");
                return;
            }

            var result = MessageBox.Show(
                $"Supprimer {selectedUser.Username} ?",
                "Confirmation",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes)
                return;

            _context.Users.Remove(selectedUser);

            _context.SaveChanges();

            MessageBox.Show("Utilisateur supprimé ✔");

            ChargerUsers();
            ClearForm();
        }

        // ================= SELECTION =================

        private void DgUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            User user = dgUsers.SelectedItem as User;

            if (user == null)
                return;

            selectedUser = user;

            txtUsername.Text = user.Username;

            txtPassword.Password = "";

            foreach (ComboBoxItem item in cbRole.Items)
            {
                if (item.Content.ToString() == user.Role)
                {
                    cbRole.SelectedItem = item;
                    break;
                }
            }
        }

        // ================= CLEAR =================

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            selectedUser = null;

            txtUsername.Clear();
            txtPassword.Clear();

            cbRole.SelectedIndex = -1;

            dgUsers.SelectedItem = null;
        }
    }
}