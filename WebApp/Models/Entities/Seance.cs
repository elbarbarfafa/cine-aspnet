using System.ComponentModel.DataAnnotations;

namespace WebApp.Models.Entities
{
    public class Seance
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Le tarif est obligatoire")]
        [Range(0.01, 999.99, ErrorMessage = "Le tarif doit être compris entre 0,01€ et 999,99€")]
        [Display(Name = "Tarif (€)")]
        public double Tarif { get; set; }
        
        [Required(ErrorMessage = "La date de séance est obligatoire")]
        [DataType(DataType.Date)]
        [Display(Name = "Date de séance")]
        public DateOnly DateSeance { get; set; } // Permet de stocker la date de la séance

        public Salle Salle { get; set; } = null!;
        
        public Film Film { get; set; } = null!;
        
        public Horaire Horaire { get; set; } = null!;
    }
}
