using System.ComponentModel.DataAnnotations;

namespace WebApp.Models.Entities
{
    public class Salle
    {
        [Required(ErrorMessage = "Le nom du cinéma est obligatoire")]
        [StringLength(100, ErrorMessage = "Le nom du cinéma ne peut pas dépasser 100 caractères")]
        [Display(Name = "Nom du cinéma")]
        public string CinemaNom { get; set; } = null!;
        
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
        
        [Required(ErrorMessage = "Le cinéma est obligatoire")]
        public Cinema Cinema { get; set; } = null!;
        
        public List<Seance>? Seances { get; set; } = [];
    }
}
