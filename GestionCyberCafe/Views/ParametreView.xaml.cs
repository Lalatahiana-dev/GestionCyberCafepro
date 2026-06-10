using Gestion_CyberCafe.Views.Parametre;
using GestionCyberCafe.Views;
using System.Windows;
using System.Windows.Controls;

namespace Gestion_CyberCafe.Views.Parametre
{
    public partial class ParametreView : UserControl
    {
        public ParametreView()
        {
            InitializeComponent();

            // Vue par défaut
            ParametreContent.Content = new ParametreGeneralView();
        }

        private void BtnGeneral_Click(object sender, RoutedEventArgs e)
        {
            ParametreContent.Content = new ParametreGeneralView();
        }

        private void BtnTarifs_Click(object sender, RoutedEventArgs e)
        {
            ParametreContent.Content = new ParametreTarifView();
        }

        private void BtnUsers_Click(object sender, RoutedEventArgs e)
        {
            ParametreContent.Content = new ParametreUserView();
        }

        private void BtnPostes_Click(object sender, RoutedEventArgs e)
        {
            ParametreContent.Content = new ParametrePosteView();
        }
    }
}