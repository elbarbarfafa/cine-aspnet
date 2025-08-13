using WebApp.Models.ViewModels;

namespace WebApp.Services
{
    public interface IPaginatedService<TEntity, TPaginationParams, TFilter>
        where TEntity : class
        where TPaginationParams : IPaginationParams
        where TFilter : IFilter
    {
        /// <summary>
        /// Représente une méthode pour obtenir une liste paginée d'entités.
        /// </summary>
        /// <param name="paginableData">Paramètres de pagination</param>
        /// <param name="filters">(Optionnel) permet de définir un ensemble de filtres à appliquer</param>
        /// <returns>Une liste paginée d'entités</returns>
        PaginatedList<TEntity> GetAllPaginatedAndFiltered(
            TPaginationParams paginationParams,
            TFilter filters);
    }
}
