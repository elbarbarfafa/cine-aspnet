using System.ComponentModel.DataAnnotations;

namespace WebApp.Models.ViewModels.Salle
{
    public class CreateSalleViewModel
    {

        [Required(ErrorMessage = "Le numéro de salle est obligatoire")]
        [Range(1, 999, ErrorMessage = "Le numéro de salle doit être compris entre 1 et 999")]
        [Display(Name = "Numéro de salle")]
        public int Numero { get; set; }

        [Required(ErrorMessage = "La capacité est obligatoire")]
        [Range(1, 1000, ErrorMessage = "La capacité doit être comprise entre 1 et 1000 places")]
        [Display(Name = "Capacité (nombre de places)")]
        public int Capacite { get; set; }

        [Required(ErrorMessage = "La date de construction est obligatoire")]
        [DataType(DataType.Date)]
        [Display(Name = "Date de construction")]
        public DateOnly DateConstruction { get; set; }

        public CreateSalleViewModel() { }
    }
}