using System.ComponentModel.DataAnnotations;

namespace Gestion_CyberCafe.ModelsR
{
    public class User
    {
        [Key]
        public int IdUser { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Role { get; set; } // ADMIN / CAISSIER
    }
}