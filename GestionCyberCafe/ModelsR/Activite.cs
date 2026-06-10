using System;
using System.ComponentModel.DataAnnotations;

public class Activite
{
    [Key]
    public int IdActivite { get; set; }

    public DateTime DateHeure { get; set; }

    public string TypeActivite { get; set; }
    // SESSION, PAIEMENT, WIFI, ABONNEMENT

    public string Description { get; set; }
}