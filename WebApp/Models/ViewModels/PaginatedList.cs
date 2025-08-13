using Microsoft.EntityFrameworkCore;

namespace WebApp.Models.ViewModels
{
    /// <summary>
    /// Classe générique représentant une liste paginée d'éléments.
    /// Utilisée pour diviser les grandes collections de données en pages plus petites et gérer la navigation.
    /// Implémente le pattern de pagination côté serveur pour optimiser les performances et l'expérience utilisateur.
    /// </summary>
    /// <typeparam name="T">Type des éléments contenus dans la liste paginée.</typeparam>
    /// <param name="items">Liste des éléments de la page courante.</param>
    /// <param name="count">Nombre total d'éléments dans l'ensemble de données complet.</param>
    /// <param name="pageIndex">Index de la page courante (commence à 1).</param>
    /// <param name="pageSize">Nombre d'éléments par page.</param>
    public class PaginatedList<T>(List<T> items, int count, int pageIndex, int pageSize)
    {
        /// <summary>
        /// Obtient la liste des éléments présents sur la page courante.
        /// </summary>
        public List<T> Items { get; } = items;

        /// <summary>
        /// Obtient l'index de la page courante. La première page a l'index 1.
        /// </summary>
        public int PageIndex { get; } = pageIndex;

        /// <summary>
        /// Obtient le nombre total de pages disponibles.
        /// Calculé automatiquement en fonction du nombre total d'éléments et de la taille de page.
        /// </summary>
        public int TotalPages { get; } = (int)Math.Ceiling(count / (double)pageSize);

        /// <summary>
        /// Obtient le nombre d'éléments par page.
        /// </summary>
        public int PageSize { get; } = pageSize;

        /// <summary>
        /// Indique s'il existe une page précédente.
        /// Retourne true si la page courante n'est pas la première page.
        /// </summary>
        public bool HasPreviousPage => PageIndex > 1;

        /// <summary>
        /// Indique s'il existe une page suivante.
        /// Retourne true si la page courante n'est pas la dernière page.
        /// </summary>
        public bool HasNextPage => PageIndex < TotalPages;

        /// <summary>
        /// Crée une instance de PaginatedList à partir d'une liste source complète.
        /// Cette méthode effectue la pagination côté application.
        /// </summary>
        /// <param name="source">Liste complète des éléments à paginer.</param>
        /// <param name="pageIndex">Index de la page désirée (commence à 1).</param>
        /// <param name="pageSize">Nombre d'éléments par page.</param>
        /// <returns>Une nouvelle instance de PaginatedList contenant les éléments de la page demandée.</returns>
        /// <remarks>
        /// Cette méthode est recommandée pour des petites collections déjà chargées en mémoire.
        /// Pour de grandes collections, préférez la pagination côté base de données.
        /// </remarks>
        public static PaginatedList<T> CreateFromList(List<T> source, int pageIndex, int pageSize)
        {
            var count = source.Count;
            var items = source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }
    }
}