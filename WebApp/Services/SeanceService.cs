using WebApp.Models.Entities;
using WebApp.Models.ViewModels;
using WebApp.Models.ViewModels.Seance;
using WebApp.Repositories;

namespace WebApp.Services
{
    public class SeanceService(SeanceRepository seanceRepository) : ICrudService<Seance, int>, IPaginatedService<Seance, BasePaginationParams, SeanceFilterModel>
    {
        private readonly SeanceRepository _seanceRepository = seanceRepository;

        public void Add(Seance entity)
        {
            // Validations métier
            if (entity.Tarif <= 0)
            {
                throw new ArgumentException("Le tarif doit être supérieur à 0.");
            }

            if (entity.DateSeance < DateOnly.FromDateTime(DateTime.Today))
            {
                throw new ArgumentException("La date de séance ne peut pas être dans le passé.");
            }
            _seanceRepository.Insert(entity);
        }

        public void Delete(int entityId)
        {
            _seanceRepository.Delete(entityId);
        }

        public List<Seance> GetAll()
        {
            return _seanceRepository.GetAll();
        }

        public PaginatedList<Seance> GetAllPaginatedAndFiltered(BasePaginationParams paginationParams, SeanceFilterModel filters)
        {
            var seances = _seanceRepository.GetAllWithFilters(filters);
            return PaginatedList<Seance>.CreateFromList(seances, paginationParams.PageIndex, paginationParams.PageSize);
        }

        public Seance? GetOneById(int entityId)
        {
            return _seanceRepository.GetById(entityId);
        }

        public void Update(Seance entity)
        {
            // Mêmes validations que pour l'insertion
            if (entity.Tarif <= 0)
            {
                throw new ArgumentException("Le tarif doit être supérieur à 0.");
            }

            if (entity.DateSeance < DateOnly.FromDateTime(DateTime.Today))
            {
                throw new ArgumentException("La date de séance ne peut pas être dans le passé.");
            }
            _seanceRepository.Update(entity);
        }
    }
}
