using System.Windows;
using System.Windows.Controls;
using Gestion_CyberCafe.Views.Wifi.Pages;

namespace Gestion_CyberCafe.Views.Wifi
{
    public partial class SessionWifiView : UserControl
    {
        public SessionWifiView()
        {
            InitializeComponent();

            // default page = Client + Connexion
            WifiContent.Content = new WifiClientView();
        }

        // ================= PAGE 1 =================
        private void BtnClient_Click(object sender, RoutedEventArgs e)
        {
            WifiContent.Content = new WifiClientView();
        }

        //// ================= PAGE 2 =================
        private void BtnActive_Click(object sender, RoutedEventArgs e)
        {
            WifiContent.Content = new WifiActiveView();
        }

        //// ================= PAGE 3 =================
        private void BtnHistory_Click(object sender, RoutedEventArgs e)
        {
            WifiContent.Content = new WifiHistoryView();
        }
    }
}