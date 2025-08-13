namespace WebApp.Repositories
{
    /// <summary>
    /// Interface générique pour les dépôts CRUD (Create, Read, Update, Delete).
    /// Définit les opérations de base pour l'accès aux données au niveau de la persistance.
    /// Cette interface fait partie du pattern Repository et abstrait l'accès aux données.
    /// </summary>
    /// <typeparam name="Entity">Type de l'entité manipulée par le dépôt. Doit être une classe de référence.</typeparam>
    /// <typeparam name="EntityId">Type de l'identifiant unique de l'entité.</typeparam>
    public interface ICrudRepository<Entity, EntityId> where Entity : class
    {
        /// <summary>
        /// Insère une nouvelle entité dans la source de données.
        /// </summary>
        /// <param name="entity">L'entité à insérer dans la base de données.</param>
        /// <exception cref="InvalidOperationException">Lancée si l'insertion échoue (ex: violation de contrainte d'unicité).</exception>
        public void Insert(Entity entity);

        /// <summary>
        /// Met à jour une entité existante dans la source de données.
        /// </summary>
        /// <param name="entity">L'entité avec les données mises à jour.</param>
        /// <exception cref="KeyNotFoundException">Lancée si l'entité à mettre à jour n'existe pas.</exception>
        /// <exception cref="InvalidOperationException">Lancée si la mise à jour échoue.</exception>
        public void Update(Entity entity);

        /// <summary>
        /// Supprime une entité de la source de données en utilisant son identifiant.
        /// </summary>
        /// <param name="entityId">L'identifiant unique de l'entité à supprimer.</param>
        /// <exception cref="KeyNotFoundException">Lancée si l'entité à supprimer n'existe pas.</exception>
        /// <exception cref="InvalidOperationException">Lancée si la suppression échoue (ex: contraintes de clé étrangère).</exception>
        public void Delete(EntityId entityId);

        /// <summary>
        /// Récupère toutes les entités disponibles dans la source de données.
        /// </summary>
        /// <returns>Une liste de toutes les entités. Peut être vide si aucune entité n'existe.</returns>
        public List<Entity> GetAll();

        /// <summary>
        /// Récupère une entité spécifique par son identifiant unique.
        /// </summary>
        /// <param name="entityId">L'identifiant unique de l'entité recherchée.</param>
        /// <returns>L'entité correspondante si elle existe, sinon null.</returns>
        public Entity? GetById(EntityId entityId);
    }
}
