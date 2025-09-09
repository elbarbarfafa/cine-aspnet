using WebApp.Models.Entities;
using WebApp.Models.ViewModels.Salle;

namespace WebApp.Repositories
{
    /// <summary>
    /// Interface spécialisée pour la gestion des opérations CRUD des salles.
    /// Étend l'interface générique ICrudRepository et ajoute des opérations spécifiques aux salles.
    /// </summary>
    public interface ISalleRepository : ICrudRepository<Salle, (string CinemaNom, int Numero)>
    {
        /// <summary>
        /// Vérifie si une salle existe avec l'identifiant composite spécifié.
        /// </summary>
        /// <param name="entityId">Identifiant composite (nom du cinéma, numéro de salle).</param>
        /// <returns>True si la salle existe, false sinon.</returns>
        bool ExistsById((string CinemaNom, int Numero) entityId);

        /// <summary>
        /// Récupère toutes les salles d'un cinéma spécifique.
        /// </summary>
        /// <param name="cinemaNom">Nom du cinéma.</param>
        /// <returns>Liste des salles du cinéma spécifié.</returns>
        List<Salle> GetAll(string cinemaNom);

        /// <summary>
        /// Récupère toutes les salles d'un cinéma avec application de filtres.
        /// </summary>
        /// <param name="cinemaNom">Nom du cinéma.</param>
        /// <param name="filters">Critères de filtrage.</param>
        /// <returns>Liste des salles filtrées du cinéma spécifié.</returns>
        List<Salle> GetAllByCinemaWithFilters(string cinemaNom, SalleFilterModel filters);
    }
}