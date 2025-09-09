using WebApp.Models.Entities;
using WebApp.Models.ViewModels;
using WebApp.Repositories;

namespace WebApp.Services
{
    public class FilmService(IFilmRepository filmRepository) : IFilmService
    {
        private readonly IFilmRepository _filmRepository = filmRepository;

        public void Delete(int entityId)
        {
            _filmRepository.Delete(entityId);
        }

        public List<Film> GetAll()
        {
            return _filmRepository.GetAll();
        }

        public Film? GetOneById(int entityId)
        {
            return _filmRepository.GetById(entityId);
        }

        public void Add(Film entity)
        {
            _filmRepository.Insert(entity);
        }

        public void Update(Film entity)
        {
            _filmRepository.Update(entity);
        }

        public PaginatedList<Film> GetPaginatedList(
            string? searchName,
            string? searchType,
            int? searchYear,
            int pageIndex = 1,
            int pageSize = 10)
        {
            var films = _filmRepository.GetAllByNameOrTypeOrYear(searchName, searchType, searchYear);
            return PaginatedList<Film>.CreateFromList(films, pageIndex, pageSize);
        }
    }
}
