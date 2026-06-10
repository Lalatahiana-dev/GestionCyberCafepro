using Gestion_CyberCafe;
using Gestion_CyberCafe.Views;
using Gestion_CyberCafe.Views.Abonnement;
using Gestion_CyberCafe.Views.Poste;
using Gestion_CyberCafe.Views.Wifi;
using Gestion_CyberCafe.Views.Parametre;
using GestionCyberCafe.Views;
using System.Windows;

namespace GestionCyberCafe
{
    public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        MainContent.Content = new DashboardView();
    }

    private void BtnDashboard_Click(object sender, RoutedEventArgs e)
    {
        MainContent.Content = new DashboardView();
    }

    private void BtnPoste_Click(object sender, RoutedEventArgs e)
    {
        MainContent.Content = new PosteView();
    }

    private void BtnWifi_Click(object sender, RoutedEventArgs e)
    {
        MainContent.Content = new SessionWifiView();
    }
     private void BtnParametre_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new ParametreView();
        }

        private void BtnAbonnement_Click(object sender, RoutedEventArgs e)
    {
        MainContent.Content = new AbonnementView();
    }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow login = new LoginWindow();
            login.Show();

            this.Close();
        }
    }
}