using System.ComponentModel.DataAnnotations;

namespace WebApp.Models.Entities
{
    public class Horaire
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "L'heure de début est obligatoire")]
        [DataType(DataType.Time)]
        [Display(Name = "Heure de début")]
        public TimeSpan HeureDebut { get; set; }
        
        [Required(ErrorMessage = "L'heure de fin est obligatoire")]
        [DataType(DataType.Time)]
        [Display(Name = "Heure de fin")]
        public TimeSpan HeureFin { get; set; }
        
        public List<Seance>? Seances { get; set; } = [];
    }
}
