using System.ComponentModel.DataAnnotations;

namespace WebApp.Models.ViewModels.Cinema
{
    public class CreateCinemaViewModel
    {
        [Required(ErrorMessage = "Le nom du cinéma est obligatoire")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Le nom du cinéma doit contenir entre 2 et 100 caractères")]
        [Display(Name = "Nom du cinéma")]
        public string Nom { get; set; } = string.Empty;

        [StringLength(200, ErrorMessage = "Le nom de la rue ne peut pas dépasser 200 caractères")]
        [Display(Name = "Nom de la rue")]
        public string? Rue { get; set; }

        [Required(ErrorMessage = "Le numéro d'adresse est obligatoire")]
        [Range(1, 9999, ErrorMessage = "Le numéro d'adresse doit être compris entre 1 et 9999")]
        [Display(Name = "Numéro d'adresse")]
        public int Numero { get; set; }
    }
}