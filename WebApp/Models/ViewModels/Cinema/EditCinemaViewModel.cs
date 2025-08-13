using System.ComponentModel.DataAnnotations;

namespace WebApp.Models.ViewModels.Cinema
{
    public class EditCinemaViewModel
    {
        [StringLength(200, ErrorMessage = "Le nom de la rue ne peut pas dépasser 200 caractères")]
        [Display(Name = "Nom de la rue")]
        public string? Rue { get; set; }

        [Required(ErrorMessage = "Le numéro d'adresse est obligatoire")]
        [Range(1, 9999, ErrorMessage = "Le numéro d'adresse doit être compris entre 1 et 9999")]
        [Display(Name = "Numéro d'adresse")]
        public int Numero { get; set; }

        public static EditCinemaViewModel FromCinema(WebApp.Models.Entities.Cinema cinema)
        {
            return new EditCinemaViewModel
            {
                Rue = cinema.Rue,
                Numero = cinema.Numero
            };
        }
    }
}