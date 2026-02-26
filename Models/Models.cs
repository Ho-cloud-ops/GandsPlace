using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace GantsPlace.Models
{
    public class Salle
    {
        public int Id { get; set; }
        public string Nom { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Localisation { get; set; } = string.Empty;
        public int Capacite { get; set; }
        public string TypeSalle { get; set; } = string.Empty;
        public List<string> Equipements { get; set; } = new();
        public string? ImagePath { get; set; }
    }

    public class Reservation
    {
        public int Id { get; set; }
        public int SalleId { get; set; }
        public string SalleNom { get; set; } = string.Empty;
        public string UserNom { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public TimeSpan HeureDebut { get; set; }
        public TimeSpan HeureFin { get; set; }
        public string TypeSalle { get; set; } = string.Empty;
        public string Statut { get; set; } = "Confirmée";
        public string DateFormatee => Date.ToString("dd/MM/yyyy");
        public string HeuresFormatees => $"{HeureDebut:hh\\:mm} - {HeureFin:hh\\:mm}";
    }

    public class Utilisateur
    {
        public int Id { get; set; }
        public string NomComplet { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string MotDePasse { get; set; } = string.Empty;
        public ObservableCollection<Reservation> Reservations { get; set; } = new();
    }

    public static class Session
    {
        public static Utilisateur? UtilisateurConnecte { get; set; }
        public static bool EstConnecte => UtilisateurConnecte != null;
    }
}
