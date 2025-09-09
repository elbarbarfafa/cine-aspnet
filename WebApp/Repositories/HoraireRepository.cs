using WebApp.Models.Entities;

namespace WebApp.Repositories
{
    public class HoraireRepository(MyContext myContext) : IHoraireRepository
    {
        private readonly MyContext _myContext = myContext;

        public void Delete(int entityId)
        {
            throw new NotImplementedException();
        }

        public List<Horaire> GetAll()
        {
            return [.. _myContext.Horaires];
        }

        public Horaire? GetById(int entityId)
        {
            var horaire = _myContext.Horaires.Find(entityId);
            return horaire;
        }

        public void Insert(Horaire entity)
        {
            throw new NotImplementedException();
        }

        public void Update(Horaire entity)
        {
            throw new NotImplementedException();
        }
    }
}
