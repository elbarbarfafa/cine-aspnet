using System.ComponentModel.DataAnnotations;

namespace WebApp.Models.ViewModels.Film
{
    /// <summary>
    /// ViewModel pour l'édition d'un film existant.
    /// Contient les données nécessaires pour modifier un film et les règles de validation associées.
    /// Implémente le pattern ViewModel pour séparer la logique de présentation des entités métier.
    /// </summary>
    public class EditFilmViewModel
    {
        /// <summary>
        /// Identifiant unique du film à modifier.
        /// Utilisé pour assurer la cohérence lors de la mise à jour.
        /// </summary>
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// Titre du film. Doit être unique et descriptif.
        /// </summary>
        [Required(ErrorMessage = "Le titre est obligatoire")]
        [StringLength(100, ErrorMessage = "Le titre ne peut pas dépasser 100 caractères")]
        [Display(Name = "Titre")]
        public string Titre { get; set; } = string.Empty;

        /// <summary>
        /// Année de sortie du film.
        /// Limitée à une plage raisonnable pour éviter les erreurs de saisie.
        /// </summary>
        [Required(ErrorMessage = "L'année est obligatoire")]
        [Range(1800, 2100, ErrorMessage = "L'année doit être comprise entre 1800 et 2100")]
        [Display(Name = "Année")]
        public int Annee { get; set; }

        /// <summary>
        /// Genre cinématographique du film (ex: Action, Comédie, Drame).
        /// </summary>
        [Required(ErrorMessage = "Le genre est obligatoire")]
        [StringLength(50, ErrorMessage = "Le genre ne peut pas dépasser 50 caractères")]
        [Display(Name = "Genre")]
        public string Genre { get; set; } = string.Empty;

        /// <summary>
        /// Méthode factory pour créer un EditFilmViewModel à partir d'une entité Film.
        /// Facilite la conversion entre l'entité métier et le ViewModel de présentation.
        /// </summary>
        /// <param name="film">Entité Film source contenant les données à éditer.</param>
        /// <returns>Nouvelle instance d'EditFilmViewModel avec les données du film.</returns>
        /// <exception cref="ArgumentNullException">Lancée si le paramètre film est null.</exception>
        public static EditFilmViewModel FromFilm(WebApp.Models.Entities.Film film)
        {
            if (film == null)
                throw new ArgumentNullException(nameof(film), "Le film ne peut pas être null");

            return new EditFilmViewModel
            {
                Id = film.Id,
                Titre = film.Titre,
                Annee = film.Annee,
                Genre = film.Genre
            };
        }
    }
}