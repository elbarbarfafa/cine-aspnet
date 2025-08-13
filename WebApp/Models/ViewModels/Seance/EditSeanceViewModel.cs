using System.ComponentModel.DataAnnotations;

namespace WebApp.Models.ViewModels.Seance
{
    public class EditSeanceViewModel
    {
        [Required]
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Le tarif est obligatoire")]
        [Range(0.01, 999.99, ErrorMessage = "Le tarif doit être compris entre 0,01€ et 999,99€")]
        [Display(Name = "Tarif (€)")]
        public double Tarif { get; set; }
        
        [Required(ErrorMessage = "La date de séance est obligatoire")]
        [DataType(DataType.Date)]
        [Display(Name = "Date de séance")]
        public DateOnly DateSeance { get; set; }
        
        [Required(ErrorMessage = "Le nom du cinéma est obligatoire")]
        [StringLength(100, ErrorMessage = "Le nom du cinéma ne peut pas dépasser 100 caractères")]
        [Display(Name = "Nom du cinéma")]
        public string SalleCinemaNom { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Le numéro de salle est obligatoire")]
        [Range(1, 999, ErrorMessage = "Le numéro de salle doit être compris entre 1 et 999")]
        [Display(Name = "Numéro de salle")]
        public int SalleNumero { get; set; }
        
        [Required(ErrorMessage = "L'horaire est obligatoire")]
        [Range(1, int.MaxValue, ErrorMessage = "Veuillez sélectionner un horaire")]
        [Display(Name = "Horaire")]
        public int HoraireId { get; set; }

        // Méthode statique pour créer le ViewModel à partir d'une entité Seance
        public static EditSeanceViewModel FromSeance(WebApp.Models.Entities.Seance seance)
        {
            return new EditSeanceViewModel
            {
                Id = seance.Id,
                Tarif = seance.Tarif,
                DateSeance = seance.DateSeance,
                SalleCinemaNom = seance.Salle.CinemaNom,
                SalleNumero = seance.Salle.Numero,
                HoraireId = seance.Horaire.Id
            };
        }
    }
}