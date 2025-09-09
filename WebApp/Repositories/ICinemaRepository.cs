using WebApp.Models.Entities;

namespace WebApp.Repositories
{
    /// <summary>
    /// Interface spécialisée pour la gestion des opérations CRUD des cinémas.
    /// Étend l'interface générique ICrudRepository et ajoute des opérations spécifiques aux cinémas.
    /// </summary>
    public interface ICinemaRepository : ICrudRepository<Cinema, string>
    {
        /// <summary>
        /// Récupère tous les cinémas dont le nom ou la rue contient la chaîne de caractères spécifiée.
        /// </summary>
        /// <param name="name">Chaîne de caractères à rechercher dans le nom ou la rue du cinéma.</param>
        /// <returns>Liste des cinémas correspondant aux critères de recherche.</returns>
        List<Cinema> GetAllAndSearchFor(string? name);
    }
}