using Gestion_CyberCafe.Data;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Gestion_CyberCafe.Views.Poste
{
    public partial class MonitoringPosteView : UserControl
    {
        private readonly GestionCyberContext _context;
        private readonly DispatcherTimer _timer;

        public MonitoringPosteView()
        {
            InitializeComponent();

            _context = new GestionCyberContext();

            ChargerDonnees();

            // 🔁 AUTO REFRESH (real-time)
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(10);
            _timer.Tick += (s, e) => ChargerDonnees();
            _timer.Start();
        }

        // ================= MAIN LOAD =================
        private void ChargerDonnees()
        {
            // ================= STATISTIQUES =================
            txtLibres.Text =
                _context.Postes.Count(p => p.Statut == "LIBRE").ToString();

            txtOccupes.Text =
                _context.Postes.Count(p => p.Statut == "OCCUPE").ToString();

            txtMaintenance.Text =
                _context.Postes.Count(p => p.Statut == "MAINTENANCE").ToString();

            txtClientsActifs.Text =
                _context.SessionPostes.Count(s => s.Statut == "ACTIVE").ToString();

            // ================= MONITORING JOIN =================
            var data = (from p in _context.Postes
                        join s in _context.SessionPostes
                        on p.IdPoste equals s.IdPoste into ps
                        from s in ps.DefaultIfEmpty()

                        join c in _context.Clients
                        on s.IdClient equals c.IdClient into sc
                        from c in sc.DefaultIfEmpty()

                        select new
                        {
                            p.NomPoste,
                            StatutPoste = p.Statut,

                            NomClient = c != null ? c.Nom + " " + c.Prenom : "--",
                            Telephone = c != null ? c.Telephone : "--",

                            HeureDebut = s != null ? s.HeureDebut : (DateTime?)null,
                            HeureFin = s != null ? s.HeureFin : (DateTime?)null,

                            DureeMinutes = s != null ? s.DureeMinutes : 0,

                            MontantTotal = s != null ? s.MontantTotal : 0,

                            StatutSession = s != null ? s.Statut : "--"
                        })
            .AsEnumerable() // 🔥 IMPORTANT: switch to C#
            .Select(x => new
            {
                x.NomPoste,
                x.StatutPoste,
                x.NomClient,
                x.Telephone,

                HeureDebut = x.HeureDebut.HasValue
                    ? x.HeureDebut.Value.ToString("HH:mm")
                    : "--",

                HeureFin = x.HeureFin.HasValue
                    ? x.HeureFin.Value.ToString("HH:mm")
                    : "--",

                x.DureeMinutes,

                MontantTotal = x.MontantTotal.ToString("N0") + " Ar",

                Statut = x.StatutSession
            })
            .ToList();

            dgMonitoring.ItemsSource = data;
        }

        // ================= ACTIONS =================

        private void BtnTerminer_Click(object sender, RoutedEventArgs e)
        {
            if (!(sender is Button btn))
                return;

            if (btn.Tag == null)
            {
                MessageBox.Show("Tag du bouton introuvable.");
                return;
            }

            string nomPoste = btn.Tag.ToString();

            var poste = _context.Postes
                                .FirstOrDefault(p => p.NomPoste == nomPoste);

            if (poste == null)
            {
                MessageBox.Show("Poste introuvable.");
                return;
            }

            var session = _context.SessionPostes
                                  .FirstOrDefault(s =>
                                      s.IdPoste == poste.IdPoste &&
                                      s.Statut == "ACTIVE");

            if (session == null)
            {
                MessageBox.Show("Aucune session active trouvée.");
                return;
            }

            session.Statut = "TERMINEE";
            session.HeureFin = DateTime.Now;

            poste.Statut = "LIBRE";

            _context.SaveChanges();

            ChargerDonnees();

            MessageBox.Show("Session terminée avec succès.");
        }

        private void BtnMaintenance_Click(object sender, RoutedEventArgs e)
        {
            if (!(sender is Button btn))
                return;

            if (btn.Tag == null)
            {
                MessageBox.Show("Tag du bouton introuvable.");
                return;
            }

            string nomPoste = btn.Tag.ToString();

            var poste = _context.Postes
                                .FirstOrDefault(p => p.NomPoste == nomPoste);

            if (poste == null)
            {
                MessageBox.Show("Poste introuvable.");
                return;
            }

            poste.Statut = "MAINTENANCE";

            _context.SaveChanges();

            ChargerDonnees();

            MessageBox.Show($"Le poste {nomPoste} est maintenant en maintenance.");
        }

        private void BtnLibre_Click(object sender, RoutedEventArgs e)
        {
            if (!(sender is Button btn))
                return;

            if (btn.Tag == null)
            {
                MessageBox.Show("Tag du bouton introuvable.");
                return;
            }

            string nomPoste = btn.Tag.ToString();

            var poste = _context.Postes
                                .FirstOrDefault(p => p.NomPoste == nomPoste);

            if (poste == null)
            {
                MessageBox.Show("Poste introuvable.");
                return;
            }

            poste.Statut = "LIBRE";

            _context.SaveChanges();

            ChargerDonnees();

            MessageBox.Show($"Le poste {nomPoste} est maintenant libre.");
        }
    }
}