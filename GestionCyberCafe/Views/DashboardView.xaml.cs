using System.Linq;
using System.Windows.Controls;
using Gestion_CyberCafe.Data;

namespace Gestion_CyberCafe.Views
{
    public partial class DashboardView : UserControl
    {
        private readonly GestionCyberContext _context;

        public DashboardView()
        {
            InitializeComponent();

            _context = new GestionCyberContext();

            ChargerStatistiques();
            ChargerHistorique();
        }

        // ================= STATISTIQUES =================
        private void ChargerStatistiques()
        {
            // Clients
            txtClients.Text =
                _context.Clients.Count().ToString();

            // Sessions Poste actives
            txtSessionsPoste.Text =
                _context.SessionPostes
                .Count(s => s.Statut == "ACTIVE" && !s.IsDeleted)
                .ToString();

            // Sessions Wifi actives
            txtSessionsWifi.Text =
                _context.SessionWifis
                .Count(s => s.Statut == "ACTIVE" && !s.IsDeleted)
                .ToString();

            // Abonnements actifs
            txtAbonnementsActifs.Text =
                _context.AbonnementInternets
                .Count(a => a.Statut == "ACTIVE" && !a.IsDeleted)
                .ToString();

            // Suspendus
            txtSuspendus.Text =
                _context.AbonnementInternets
                .Count(a => a.Statut == "SUSPENDU" && !a.IsDeleted)
                .ToString();

            // Expirés
            txtExpires.Text =
                _context.AbonnementInternets
                .Count(a => a.Statut == "EXPIRE" && !a.IsDeleted)
                .ToString();

            // Postes libres
            txtPostes.Text =
                _context.Postes
                .Count(p => p.Statut == "LIBRE")
                .ToString();

            // Total abonnements
            txtTotalAbonnements.Text =
                _context.AbonnementInternets
                .Count(a => !a.IsDeleted)
                .ToString();
        }

        // ================= HISTORIQUE =================
        private void ChargerHistorique()
        {
            var historique =
                _context.AbonnementInternets
                .Where(a => !a.IsDeleted)
                .OrderByDescending(a => a.DateDebut)
                .Take(10)
                .Select(a => new
                {
                    a.IdAbonnementInternet,
                    a.TypeAcces,
                    a.TypeForfait,
                    a.DateDebut,
                    a.DateExpiration,
                    a.NombreAppareils,
                    a.Montant,
                    a.Statut
                })
                .ToList();

            dgHistorique.ItemsSource = historique;
        }
    }
}