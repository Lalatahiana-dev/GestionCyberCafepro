using Gestion_CyberCafe.Data;
using GestionCyberCafe;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Gestion_CyberCafe
{
    public partial class LoginWindow : Window
    {
        private readonly GestionCyberContext _context;
        private bool _passwordVisible = false;

        public LoginWindow()
        {
            InitializeComponent();
            _context = new GestionCyberContext();
        }

        // ================= PASSWORD TOGGLE =================
        private void TogglePassword_Click(object sender, RoutedEventArgs e)
        {
            _passwordVisible = !_passwordVisible;

            if (_passwordVisible)
            {
                txtPasswordVisible.Text = txtPassword.Password;

                txtPassword.Visibility = Visibility.Collapsed;
                txtPasswordVisible.Visibility = Visibility.Visible;

                eyeIcon.Icon = FontAwesome.WPF.FontAwesomeIcon.EyeSlash;
            }
            else
            {
                txtPassword.Password = txtPasswordVisible.Text;

                txtPassword.Visibility = Visibility.Visible;
                txtPasswordVisible.Visibility = Visibility.Collapsed;

                eyeIcon.Icon = FontAwesome.WPF.FontAwesomeIcon.Eye;
            }
        }

        // ================= LOGIN =================
        private async void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string username = txtUsername.Text?.Trim();
                string password = _passwordVisible
                    ? txtPasswordVisible.Text?.Trim()
                    : txtPassword.Password?.Trim();

                if (string.IsNullOrWhiteSpace(username) ||
                    string.IsNullOrWhiteSpace(password))
                {
                    MessageBox.Show("Veuillez remplir tous les champs !");
                    return;
                }

                // LOADING ON
                btnLogin.IsEnabled = false;
                loadingPanel.Visibility = Visibility.Visible;

                await Task.Delay(1000);

                var user = _context.Users
                    .FirstOrDefault(u => u.Username == username && u.Password == password);

                if (user == null)
                {
                    MessageBox.Show("Login ou mot de passe incorrect !");
                    return;
                }


                new MainWindow().Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur : " + ex.Message);
            }
            finally
            {
                btnLogin.IsEnabled = true;
                loadingPanel.Visibility = Visibility.Collapsed;
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            _context?.Dispose();
        }
    }
}