using System.Linq;
using System.Windows;
using GestionCyberCafe;
using Gestion_CyberCafe.Data;

namespace Gestion_CyberCafe
{
    public partial class LoginWindow : Window
    {
        private GestionCyberContext _context;

        public LoginWindow()
        {
            InitializeComponent();

            _context = new GestionCyberContext();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Password;

            var user = _context.Users
                .FirstOrDefault(u =>
                    u.Username == username &&
                    u.Password == password);

            if (user == null)
            {
                MessageBox.Show("Login ou mot de passe incorrect !");
                return;
            }

            //// Open MainWindow
            MainWindow main = new MainWindow();
            main.Show();

            this.Close();
        }
    }
}