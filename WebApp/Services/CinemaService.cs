using WebApp.Models.Entities;
using WebApp.Models.ViewModels;
using WebApp.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Models.ViewModels.Cinema;

namespace WebApp.Services
{
    public class CinemaService(ICinemaRepository cinemaRepository) : ICinemaService
    {
        private readonly ICinemaRepository _cinemaRepository = cinemaRepository;

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

        public Cinema? GetOneById(string nom)
        {
            return _cinemaRepository.GetById(nom);
        }

        public void Add(Cinema cinema)
        {
            _cinemaRepository.Insert(cinema);
        }

        public void Update(Cinema cinema)
        {
            _cinemaRepository.Update(cinema);
        }

        public void Delete(string nom)
        {
            _cinemaRepository.Delete(nom);
        }

        public PaginatedList<Cinema> GetAllPaginatedAndFiltered(BasePaginationParams paginationParams, CinemaFilterModel filters)
        {
            var cinemas = _cinemaRepository.GetAllAndSearchFor(filters.CinemaSearch);
            return PaginatedList<Cinema>.CreateFromList(cinemas, paginationParams.PageIndex, paginationParams.PageSize);
        }
    }
}
