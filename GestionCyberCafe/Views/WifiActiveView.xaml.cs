using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Gestion_CyberCafe.Data;

namespace Gestion_CyberCafe.Views.Wifi.Pages
{
    public partial class WifiActiveView : UserControl
    {
        private readonly GestionCyberContext _context;
        private DispatcherTimer _timer;

        public WifiActiveView()
        {
            InitializeComponent();

            _context = new GestionCyberContext();

            ChargerActives();
            StartTimer();
        }

        // ================= LOAD ACTIVE SESSIONS =================
        private void ChargerActives()
        {
            // 🔴 AUTO CLOSE EXPIRED SESSIONS
            AutoCloseSessions();

            var data = (from s in _context.SessionWifis
                        join c in _context.Clients on s.IdClient equals c.IdClient
                        where s.Statut == "ACTIVE" && !s.IsDeleted
                        select new
                        {
                            s.IdSessionWifi,
                            NomClient = c.Nom + " " + c.Prenom,
                            s.MarqueAppareil,
                            s.TypeReseau,
                            s.HeureDebut,
                            s.DureeMinutes,
                            s.MontantTotal
                        })
                        .ToList()
                        .Select(x => new
                        {
                            x.IdSessionWifi,
                            x.NomClient,
                            x.MarqueAppareil,
                            x.TypeReseau,
                            HeureDebut = x.HeureDebut.ToString("HH:mm"),

                            TempsRestant = CalculTempsRestant(x.HeureDebut, x.DureeMinutes),

                            MontantTotal = (x.MontantTotal).ToString("N0") + " Ar",
                            Statut = "ACTIVE"
                        })
                        .ToList();

            dgActive.ItemsSource = data;

            txtActiveCount.Text = data.Count.ToString();

            // 🔴 SAFE SUM (fix null crash)
            decimal total = _context.SessionWifis
                .Where(s => s.Statut == "ACTIVE" && !s.IsDeleted)
                .Select(s => (decimal?)s.MontantTotal)
                .Sum() ?? 0m;

            txtTotalMontant.Text = total.ToString("N0") + " Ar";
        }

        // ================= AUTO CLOSE EXPIRED =================
        private void AutoCloseSessions()
        {
            var sessions = _context.SessionWifis
                .Where(s => s.Statut == "ACTIVE" && !s.IsDeleted)
                .ToList();

            foreach (var s in sessions)
            {
                var fin = s.HeureDebut.AddMinutes(s.DureeMinutes);

                if (DateTime.Now >= fin)
                {
                    s.Statut = "TERMINEE";
                    s.HeureFin = DateTime.Now;
                }
            }

            _context.SaveChanges();
        }

        // ================= CALCUL TEMPS RESTANT =================
        private string CalculTempsRestant(DateTime debut, int dureeMinutes)
        {
            var fin = debut.AddMinutes(dureeMinutes);
            var restant = fin - DateTime.Now;

            if (restant.TotalMinutes <= 0)
                return "Expiré";

            return $"{(int)restant.TotalHours}h {(int)restant.Minutes}m";
        }

        // ================= TIMER AUTO REFRESH =================
        private void StartTimer()
        {
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(30)
            };

            _timer.Tick += (s, e) =>
            {
                ChargerActives();
            };

            _timer.Start();
        }

        // ================= TERMINER SESSION =================
        private void BtnTerminate_Click(object sender, RoutedEventArgs e)
        {
            var row = (FrameworkElement)sender;

            dynamic data = row.DataContext;

            int id = data.IdSessionWifi;

            var session = _context.SessionWifis
                .FirstOrDefault(x => x.IdSessionWifi == id);

            if (session == null)
            {
                MessageBox.Show("Session introuvable !");
                return;
            }

            session.Statut = "TERMINEE";
            session.HeureFin = DateTime.Now;

            _context.SaveChanges();

            ChargerActives();

            MessageBox.Show("Session terminée ✔");
        }

        // ================= PROLONGER SESSION =================
        private void BtnExtend_Click(object sender, RoutedEventArgs e)
        {
            var row = (FrameworkElement)sender;

            dynamic data = row.DataContext;

            int id = data.IdSessionWifi;

            var session = _context.SessionWifis
                .FirstOrDefault(x => x.IdSessionWifi == id);

            if (session == null)
            {
                MessageBox.Show("Session introuvable !");
                return;
            }

            session.DureeMinutes += 30;

            _context.SaveChanges();

            ChargerActives();

            MessageBox.Show("+30 min ajoutées ✔");
        }
    }
}