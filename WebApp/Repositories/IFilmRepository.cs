using WebApp.Models.Entities;

namespace WebApp.Repositories
{
    /// <summary>
    /// Interface spécialisée pour la gestion des opérations CRUD des films.
    /// Étend l'interface générique ICrudRepository et ajoute des opérations spécifiques aux films.
    /// </summary>
    public interface IFilmRepository : ICrudRepository<Film, int>
    {
        /// <summary>
        /// Récupère tous les films en fonction de critères de recherche optionnels.
        /// </summary>
        /// <param name="searchName">Nom/titre à rechercher dans les films (optionnel).</param>
        /// <param name="searchType">Genre à rechercher dans les films (optionnel).</param>
        /// <param name="searchYear">Année à rechercher dans les films (optionnel).</param>
        /// <returns>Liste des films correspondant aux critères de recherche.</returns>
        List<Film> GetAllByNameOrTypeOrYear(string? searchName = null, string? searchType = null, int? searchYear = null);
    }
}