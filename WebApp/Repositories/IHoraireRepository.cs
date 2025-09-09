using WebApp.Models.Entities;

namespace WebApp.Repositories
{
    /// <summary>
    /// Interface spécialisée pour la gestion des opérations CRUD des horaires.
    /// Étend l'interface générique ICrudRepository pour les horaires.
    /// </summary>
    public interface IHoraireRepository : ICrudRepository<Horaire, int>
    {
        // Pour l'instant, cette interface n'ajoute pas de méthodes spécifiques
        // mais elle peut être étendue avec des opérations spécifiques aux horaires si nécessaire
    }
}