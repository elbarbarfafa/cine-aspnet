using WebApp.Models.Entities;
using WebApp.Models.ViewModels;
using WebApp.Repositories;

namespace WebApp.Services
{
    public class FilmService(FilmRepository filmRepository) : ICrudService<Film, int>
    {
        private readonly FilmRepository _filmRepository = filmRepository;

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
            // Vous pourriez ajouter ici des validations métier supplémentaires
            if (string.IsNullOrWhiteSpace(entity.Titre))
            {
                throw new ArgumentException("Le titre du film ne peut pas être vide.");
            }

            if (entity.Annee <= 1800)
            {
                throw new ArgumentException("L'année du film doit être supérieure à 1800.");
            }

            _filmRepository.Insert(entity);
        }

        public void Update(Film entity)
        {
            // Mêmes validations que pour l'insertion
            if (string.IsNullOrWhiteSpace(entity.Titre))
            {
                throw new ArgumentException("Le titre du film ne peut pas être vide.");
            }

            if (entity.Annee <= 1800)
            {
                throw new ArgumentException("L'année du film doit être supérieure à 1800.");
            }
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
