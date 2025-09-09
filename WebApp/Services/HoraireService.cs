using WebApp.Models.Entities;
using WebApp.Repositories;

namespace WebApp.Services
{
    public class HoraireService(IHoraireRepository horaireRepository) : IHoraireService
    {
        private readonly IHoraireRepository _horaireRepository = horaireRepository;
        public void Add(Horaire entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int entityId)
        {
            throw new NotImplementedException();
        }

        public List<Horaire> GetAll()
        {
            return _horaireRepository.GetAll();
        }

        public Horaire? GetOneById(int entityId)
        {
            return _horaireRepository.GetById(entityId);
            throw new NotImplementedException();
        }

        public void Update(Horaire entity)
        {
            throw new NotImplementedException();
        }
    }
}
