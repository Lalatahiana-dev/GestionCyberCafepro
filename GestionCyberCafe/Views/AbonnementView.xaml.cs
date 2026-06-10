using Gestion_CyberCafe.Views.Abonnement.Pages;
using GestionCyberCafe.Views;
using System.Windows.Controls;

namespace Gestion_CyberCafe.Views.Abonnement
{
    public partial class AbonnementView : UserControl
    {
        public AbonnementView()
        {
            InitializeComponent();

            MainArea.Content = new AbonnementInternetView();
        }

        private void BtnActivation_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            MainArea.Content = new AbonnementInternetView();
        }

        private void BtnActifs_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            MainArea.Content = new AbonnementActiveView();
        }

        private void BtnHistorique_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            MainArea.Content = new AbonnementHistoryView();
        }
    }
}