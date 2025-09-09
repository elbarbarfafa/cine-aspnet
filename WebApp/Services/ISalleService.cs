using WebApp.Models.Entities;
using WebApp.Models.ViewModels;
using WebApp.Models.ViewModels.Salle;

namespace WebApp.Services
{
    /// <summary>
    /// Interface spécialisée pour la gestion des opérations métier des salles.
    /// Étend les interfaces génériques ICrudService et IPaginatedService pour les salles.
    /// </summary>
    public interface ISalleService : ICrudService<Salle, (string cinemaNom, int numero)>, IPaginatedService<Salle, BasePaginationParams, SalleFilterModel>
    {
        // Pour l'instant, cette interface n'ajoute pas de méthodes spécifiques au-delà de celles héritées
        // mais elle peut être étendue avec des opérations spécifiques aux salles si nécessaire
    }
}