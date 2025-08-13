using WebApp.Models.Entities;
using WebApp.Models.ViewModels;
using WebApp.Models.ViewModels.Salle;
using WebApp.Repositories;

namespace WebApp.Services
{
    public class SalleService(SalleRepository salleRepository) : ICrudService<Salle, (string cinemaNom, int numero)>, IPaginatedService<Salle, BasePaginationParams, SalleFilterModel>
    {
        private readonly SalleRepository _salleRepository = salleRepository;

        public List<Salle> GetAll()
        {
            return _salleRepository.GetAll();
        }
        public Salle? GetOneById((string cinemaNom, int numero) entityId)
        {
            return _salleRepository.GetById(entityId);
        }
        public void Add(Salle salle)
        {
            _salleRepository.Insert(salle);
        }
        public void Update(Salle salle)
        {
            _salleRepository.Update(salle);
        }

        public void Delete((string cinemaNom, int numero) entityId)
        {
            _salleRepository.Delete(entityId);
        }
        
        public PaginatedList<Salle> GetAllPaginatedAndFiltered(
            BasePaginationParams paginableData,
            SalleFilterModel filters)
        {
            var salles = _salleRepository.GetAllByCinemaWithFilters(filters.CinemaNom, filters);
            return PaginatedList<Salle>.CreateFromList(salles, paginableData.PageIndex, paginableData.PageSize);
        }

    }
}
