using WebApp.Models.Entities;
using WebApp.Models.ViewModels;
using WebApp.Models.ViewModels.Seance;

namespace WebApp.Services
{
    /// <summary>
    /// Interface spécialisée pour la gestion des opérations métier des séances.
    /// Étend les interfaces génériques ICrudService et IPaginatedService pour les séances.
    /// </summary>
    public interface ISeanceService : ICrudService<Seance, int>, IPaginatedService<Seance, BasePaginationParams, SeanceFilterModel>
    {
        // Pour l'instant, cette interface n'ajoute pas de méthodes spécifiques au-delà de celles héritées
        // mais elle peut être étendue avec des opérations spécifiques aux séances si nécessaire
    }
}