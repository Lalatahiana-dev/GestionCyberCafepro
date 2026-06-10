using GestionCyberCafe.Views;
using System.Windows;
using System.Windows.Controls;
using Gestion_CyberCafe.Views.Poste;

namespace Gestion_CyberCafe.Views
{
    public partial class PosteView : UserControl
    {
        public PosteView()
        {
            InitializeComponent();
            MainArea.Content = new SessionPosteView();
        }

        private void BtnSession_Click(object sender, RoutedEventArgs e)
        {
            MainArea.Content = new SessionPosteView();
        }

        private void BtnMonitoring_Click(object sender, RoutedEventArgs e)
        {
            MainArea.Content = new MonitoringPosteView();
        }

        private void BtnHistorique_Click(object sender, RoutedEventArgs e)
        {
            MainArea.Content = new HistoriquePosteView();
        }

        private void BtnGestion_Click(object sender, RoutedEventArgs e)
        {
            MainArea.Content = new GestionPosteView();
        }
    }
}