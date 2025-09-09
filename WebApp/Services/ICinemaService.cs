using WebApp.Models.Entities;
using WebApp.Models.ViewModels;
using WebApp.Models.ViewModels.Cinema;

namespace WebApp.Services
{
    /// <summary>
    /// Interface spécialisée pour la gestion des opérations métier des cinémas.
    /// Étend les interfaces génériques ICrudService et IPaginatedService pour les cinémas.
    /// </summary>
    public interface ICinemaService : ICrudService<Cinema, string>, IPaginatedService<Cinema, BasePaginationParams, CinemaFilterModel>
    {
        /// <summary>
        /// Récupère la liste de tous les cinémas disponibles, ou filtre par nom si un paramètre de recherche est fourni.
        /// </summary>
        /// <param name="search">Nom (ou partie du nom) du cinéma à rechercher (optionnel).</param>
        /// <returns>Liste des cinémas filtrés ou tous les cinémas si search est null ou vide.</returns>
        List<Cinema> GetAllCinemas(string? search = null);
    }
}