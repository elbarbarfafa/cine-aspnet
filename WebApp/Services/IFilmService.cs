using WebApp.Models.Entities;
using WebApp.Models.ViewModels;

namespace WebApp.Services
{
    /// <summary>
    /// Interface spécialisée pour la gestion des opérations métier des films.
    /// Étend l'interface générique ICrudService pour les films.
    /// </summary>
    public interface IFilmService : ICrudService<Film, int>
    {
        /// <summary>
        /// Récupère une liste paginée de films en fonction de critères de recherche.
        /// </summary>
        /// <param name="searchName">Nom/titre à rechercher dans les films (optionnel).</param>
        /// <param name="searchType">Genre à rechercher dans les films (optionnel).</param>
        /// <param name="searchYear">Année à rechercher dans les films (optionnel).</param>
        /// <param name="pageIndex">Index de la page (commence à 1).</param>
        /// <param name="pageSize">Nombre d'éléments par page.</param>
        /// <returns>Liste paginée des films correspondant aux critères de recherche.</returns>
        PaginatedList<Film> GetPaginatedList(string? searchName, string? searchType, int? searchYear, int pageIndex = 1, int pageSize = 10);
    }
}