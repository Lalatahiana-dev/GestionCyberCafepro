using System.Data.Entity;
using Gestion_CyberCafe.ModelsR;

namespace Gestion_CyberCafe.Data
{
    public class GestionCyberContext : DbContext
    {
        public GestionCyberContext()
            : base("name=CyberDB")
        {
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Poste> Postes { get; set; }
        public DbSet<SessionPoste> SessionPostes { get; set; }
        public DbSet<Paiement> Paiements { get; set; }
        public DbSet<ParametreConfig> Parametres { get; set; }

        // WIFI + ABONNEMENT (apetraka manaraka)
        public DbSet<SessionWifi> SessionWifis { get; set; }
        public DbSet<Abonnement> Abonnements { get; set; }
        public DbSet<Activite> Activites { get; set; }
        public DbSet<AbonnementInternet> AbonnementInternets { get; set; }
        public DbSet<User> Users { get; set; }
    }
}