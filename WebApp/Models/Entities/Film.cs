using System.ComponentModel.DataAnnotations;

namespace WebApp.Models.Entities
{
    public class Film
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Le titre est obligatoire")]
        [StringLength(100, ErrorMessage = "Le titre ne peut pas dépasser 100 caractères")]
        public string Titre { get; set; } = string.Empty;

        [Required(ErrorMessage = "L'année est obligatoire")]
        [Range(1800, 2100, ErrorMessage = "L'année doit être comprise entre 1800 et 2100")]
        public int Annee { get; set; } = 0;

        [Required(ErrorMessage = "Le genre est obligatoire")]
        [StringLength(50, ErrorMessage = "Le genre ne peut pas dépasser 50 caractères")]
        public string Genre { get; set; } = string.Empty;

        public List<Seance>? Seances { get; set; } = [];
    }
}
