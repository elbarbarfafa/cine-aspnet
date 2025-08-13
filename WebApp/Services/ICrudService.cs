using WebApp.Models.ViewModels;

namespace WebApp.Services
{
    /// <summary>
    /// Interface générique pour les services CRUD (Create, Read, Update, Delete).
    /// Fournit les opérations de base pour la gestion d'entités.
    /// </summary>
    /// <typeparam name="T">Type de l'entité géré par le service. Doit être une classe de référence.</typeparam>
    /// <typeparam name="I">Type de l'identifiant unique de l'entité.</typeparam>
    public interface ICrudService<T, I> where T : class
    {
        /// <summary>
        /// Ajoute une nouvelle entité dans le système.
        /// </summary>
        /// <param name="entity">L'entité à ajouter. Ne peut pas être null.</param>
        /// <exception cref="ArgumentException">Lancée si l'entité ne respecte pas les règles de validation.</exception>
        /// <exception cref="InvalidOperationException">Lancée si l'entité existe déjà ou si l'opération échoue.</exception>
        void Add(T entity);

        /// <summary>
        /// Met à jour une entité existante dans le système.
        /// </summary>
        /// <param name="entity">L'entité avec les nouvelles données. Ne peut pas être null.</param>
        /// <exception cref="KeyNotFoundException">Lancée si l'entité à mettre à jour n'existe pas.</exception>
        /// <exception cref="ArgumentException">Lancée si l'entité ne respecte pas les règles de validation.</exception>
        void Update(T entity);

        /// <summary>
        /// Supprime une entité du système en utilisant son identifiant.
        /// </summary>
        /// <param name="entityId">L'identifiant unique de l'entité à supprimer.</param>
        /// <exception cref="KeyNotFoundException">Lancée si l'entité à supprimer n'existe pas.</exception>
        /// <exception cref="InvalidOperationException">Lancée si l'entité ne peut pas être supprimée (par ex. contraintes de référence).</exception>
        void Delete(I entityId);

        /// <summary>
        /// Récupère une entité spécifique par son identifiant unique.
        /// </summary>
        /// <param name="entityId">L'identifiant unique de l'entité recherchée.</param>
        /// <returns>L'entité correspondante si elle existe, sinon null.</returns>
        T? GetOneById(I entityId);

        /// <summary>
        /// Récupère toutes les entités disponibles dans le système.
        /// </summary>
        /// <returns>Une liste contenant toutes les entités. La liste peut être vide si aucune entité n'existe.</returns>
        List<T> GetAll();
    }
}
