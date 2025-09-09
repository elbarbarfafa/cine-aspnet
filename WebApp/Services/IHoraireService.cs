using WebApp.Models.Entities;

namespace WebApp.Services
{
    /// <summary>
    /// Interface spécialisée pour la gestion des opérations métier des horaires.
    /// Étend l'interface générique ICrudService pour les horaires.
    /// </summary>
    public interface IHoraireService : ICrudService<Horaire, int>
    {
        // Pour l'instant, cette interface n'ajoute pas de méthodes spécifiques au-delà de celles héritées
        // mais elle peut être étendue avec des opérations spécifiques aux horaires si nécessaire
    }
}