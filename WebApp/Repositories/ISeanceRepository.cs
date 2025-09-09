using WebApp.Models.Entities;
using WebApp.Models.ViewModels.Seance;

namespace WebApp.Repositories
{
    /// <summary>
    /// Interface spécialisée pour la gestion des opérations CRUD des séances.
    /// Étend l'interface générique ICrudRepository et ajoute des opérations spécifiques aux séances.
    /// </summary>
    public interface ISeanceRepository : ICrudRepository<Seance, int>
    {
        /// <summary>
        /// Récupère toutes les séances avec application de filtres.
        /// </summary>
        /// <param name="filters">Critères de filtrage pour les séances.</param>
        /// <returns>Liste des séances correspondant aux critères de filtrage.</returns>
        List<Seance> GetAllWithFilters(SeanceFilterModel filters);
    }
}