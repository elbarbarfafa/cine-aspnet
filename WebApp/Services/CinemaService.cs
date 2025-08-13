using WebApp.Models.Entities;
using WebApp.Models.ViewModels;
using WebApp.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Models.ViewModels.Cinema;

namespace WebApp.Services
{
    public class CinemaService(CinemaRepository cinemaRepository) : ICrudService<Cinema,string>, IPaginatedService<Cinema, BasePaginationParams, CinemaFilterModel>
    {
        private readonly CinemaRepository _cinemaRepository = cinemaRepository;

        /// <summary>
        /// Récupère la liste de tous les cinémas disponibles, ou filtre par nom si un paramètre de recherche est fourni.
        /// </summary>
        /// <param name="search">Nom (ou partie du nom) du cinéma à rechercher (optionnel).</param>
        /// <returns>Liste des cinémas filtrés ou tous les cinémas si search est null ou vide.</returns>
        public List<Cinema> GetAllCinemas(string? search = null)
        {
            var cinemas = _cinemaRepository.GetAll();
            if (!string.IsNullOrWhiteSpace(search))
            {
                cinemas = [.. cinemas.Where(c => c.Nom.Contains(search, System.StringComparison.OrdinalIgnoreCase))];
            }
            return cinemas;
        }


        public List<Cinema> GetAll()
        {
            var cinemas = _cinemaRepository.GetAll();
            return [.. cinemas];
        }

        /// <summary>
        /// Récupère un cinéma par son nom (identifiant).
        /// </summary>
        /// <param name="nom">Le nom du cinéma à rechercher.</param>
        /// <returns>Le cinéma correspondant ou null s'il n'existe pas.</returns>
        public Cinema? GetOneById(string nom)
        {
            return _cinemaRepository.GetById(nom);
        }

        /// <summary>
        /// Ajoute un nouveau cinéma à la base de données.
        /// </summary>
        /// <param name="cinema">L'entité cinéma à ajouter.</param>
        public void Add(Cinema cinema)
        {
            _cinemaRepository.Insert(cinema);
        }

        /// <summary>
        /// Met à jour les informations d'un cinéma existant.
        /// </summary>
        /// <param name="cinema">L'entité cinéma avec les nouvelles valeurs.</param>
        public void Update(Cinema cinema)
        {
            _cinemaRepository.Update(cinema);
        }

        /// <summary>
        /// Supprime un cinéma de la base de données par son nom (identifiant).
        /// </summary>
        /// <param name="nom">Le nom du cinéma à supprimer.</param>
        public void Delete(string nom)
        {
            _cinemaRepository.Delete(nom);
        }

        /// <summary>
        /// Pagine et filtre la liste des cinémas en fonction des paramètres de pagination et des filtres fournis.
        /// </summary>
        /// <param name="paginationParams"></param>
        /// <param name="filters"></param>
        /// <returns></returns>
        public PaginatedList<Cinema> GetAllPaginatedAndFiltered(BasePaginationParams paginationParams, CinemaFilterModel filters)
        {
            var cinemas = _cinemaRepository.GetAllAndSearchFor(filters.CinemaSearch);
            return PaginatedList<Cinema>.CreateFromList(cinemas, paginationParams.PageIndex, paginationParams.PageSize);
        }
    }
}
