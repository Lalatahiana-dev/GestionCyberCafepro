using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Gestion_CyberCafe.Data;
using Gestion_CyberCafe.ModelsR;

namespace Gestion_CyberCafe.Views.Parametre
{
    public partial class ParametreGeneralView : UserControl
    {
        private readonly GestionCyberContext _context;

        public ParametreGeneralView()
        {
            InitializeComponent();

            _context = new GestionCyberContext();

            LoadParametre();
        }

        // ================= LOAD =================
        private void LoadParametre()
        {
            var param = _context.Parametres.FirstOrDefault();

            if (param == null) return;

            txtNomCyber.Text = param.NomCyber;
            txtAdresse.Text = param.Adresse;
            txtTelephone.Text = param.Telephone;
        }

        // ================= SAVE =================
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            var param = _context.Parametres.FirstOrDefault();

            if (param == null)
            {
                param = new ParametreConfig();
                _context.Parametres.Add(param);
            }

            param.NomCyber = txtNomCyber.Text;
            param.Adresse = txtAdresse.Text;
            param.Telephone = txtTelephone.Text;

            _context.SaveChanges();

            MessageBox.Show("Paramètres enregistrés ✔");
        }
    }
}