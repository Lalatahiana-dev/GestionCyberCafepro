using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Gestion_CyberCafe.Data;
using Gestion_CyberCafe.ModelsR;

namespace Gestion_CyberCafe.Views.Parametre
{
    public partial class ParametreTarifView : UserControl
    {
        private readonly GestionCyberContext _context;

        public ParametreTarifView()
        {
            InitializeComponent();

            _context = new GestionCyberContext();

            ChargerTarifs();
        }

        // ================= LOAD =================
        private void ChargerTarifs()
        {
            var param = _context.Parametres.FirstOrDefault();

            if (param == null)
                return;

            // POSTE
            txtPrixHeurePC.Text = param.PrixHeurePC.ToString("N0");
            txtPrixMinutePC.Text = param.PrixMinutePC.ToString("N0");

            // WIFI
            txtPrixHeureWifi.Text = param.PrixHeureWifi.ToString("N0");
            txtPrixMinuteWifi.Text = param.PrixMinuteWifi.ToString("N0");

            // ABONNEMENTS
            txtSemaine.Text = param.PrixSemainePoste.ToString("N0");
            txtMois.Text = param.PrixMoisPoste.ToString("N0");
            txtAnnee.Text = param.PrixAnneePoste.ToString("N0");
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

            // POSTE
            decimal.TryParse(txtPrixHeurePC.Text, out decimal prixHeurePC);
            decimal.TryParse(txtPrixMinutePC.Text, out decimal prixMinutePC);

            // WIFI
            decimal.TryParse(txtPrixHeureWifi.Text, out decimal prixHeureWifi);
            decimal.TryParse(txtPrixMinuteWifi.Text, out decimal prixMinuteWifi);

            // ABONNEMENTS
            decimal.TryParse(txtSemaine.Text, out decimal prixSemaine);
            decimal.TryParse(txtMois.Text, out decimal prixMois);
            decimal.TryParse(txtAnnee.Text, out decimal prixAnnee);

            param.PrixHeurePC = prixHeurePC;
            param.PrixMinutePC = prixMinutePC;

            param.PrixHeureWifi = prixHeureWifi;
            param.PrixMinuteWifi = prixMinuteWifi;

            param.PrixSemainePoste = prixSemaine;
            param.PrixMoisPoste = prixMois;
            param.PrixAnneePoste = prixAnnee;

            _context.SaveChanges();

            MessageBox.Show(
                "Tarifs enregistrés avec succès ✔",
                "Paramètres",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
    }
}