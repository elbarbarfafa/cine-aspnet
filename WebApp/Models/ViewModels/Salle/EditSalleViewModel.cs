using System.ComponentModel.DataAnnotations;

namespace WebApp.Models.ViewModels.Salle
{
    public class EditSalleViewModel
    {
        [Required(ErrorMessage = "La capacité est obligatoire")]
        [Range(1, 1000, ErrorMessage = "La capacité doit être comprise entre 1 et 1000 places")]
        [Display(Name = "Capacité (nombre de places)")]
        public int Capacite { get; set; }

        [Required(ErrorMessage = "La date de construction est obligatoire")]
        [DataType(DataType.Date)]
        [Display(Name = "Date de construction")]
        public DateOnly DateConstruction { get; set; }

        public EditSalleViewModel()
        {
            DateConstruction = DateOnly.FromDateTime(DateTime.Now);
        }

        public static EditSalleViewModel FromSalle(WebApp.Models.Entities.Salle salle)
        {
            return new EditSalleViewModel
            {
                Capacite = salle.Capacite,
                DateConstruction = salle.DateConstruction
            };
        }
    }
}