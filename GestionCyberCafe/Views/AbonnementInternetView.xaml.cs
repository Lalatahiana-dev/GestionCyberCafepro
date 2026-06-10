using System.Windows;
using System.Windows.Controls;

namespace Gestion_CyberCafe.Views.Abonnement.Pages
{
    public partial class AbonnementInternetView : UserControl
    {
        public AbonnementInternetView()
        {
            InitializeComponent();

        // Page par défaut
        MainArea.Content = new ForfaitView();
        }

        private void BtnForfait_Click(object sender, RoutedEventArgs e)
        {
            MainArea.Content = new ForfaitView();
        }

        private void BtnAbonnementLong_Click(object sender, RoutedEventArgs e)
        {
            MainArea.Content = new AbonnementLongView();
        }
    }


}
