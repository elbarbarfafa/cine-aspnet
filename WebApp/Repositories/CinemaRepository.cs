using Microsoft.EntityFrameworkCore;
using WebApp.Models.Entities;

namespace WebApp.Repositories
{
    public class CinemaRepository(MyContext context) : ICrudRepository<Cinema, string>
    {
        private readonly MyContext _context = context;

        public void Delete(string entityId)
        {
            Cinema? cinema = _context.Cinemas.Find(entityId) ?? throw new KeyNotFoundException($"Cinema with ID {entityId} not found.");
            _context.Cinemas.Remove(cinema);
            _context.SaveChanges();
        }

        public List<Cinema> GetAll()
        {
            return [.. _context.Cinemas];
        }

        /// <summary>
        /// Récupère tous les cinémas dont le nom ou la rue contient la chaîne de caractères spécifiée.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<Cinema> GetAllAndSearchFor(string? name) {
            if(string.IsNullOrWhiteSpace(name))
            {
                return [.. _context.Cinemas];
            }
            return [.. _context.Cinemas.Where(s => s.Nom.Contains(name, System.StringComparison.OrdinalIgnoreCase) ||
                                           s.Rue != null && s.Rue.Contains(name, System.StringComparison.OrdinalIgnoreCase))];
        }

        public Cinema? GetById(string entityId)
        {
            return _context.Cinemas.FirstOrDefault(c => c.Nom == entityId);
        }

        public void Insert(Cinema entity)
        {
            _context.Cinemas.Add(entity);
            _context.SaveChanges();
        }

        public void Update(Cinema entity)
        {
            var existingCinema = _context.Cinemas.Find(entity.Nom) ?? throw new KeyNotFoundException($"Cinema with name {entity.Nom} not found.");
            _context.Entry(existingCinema).CurrentValues.SetValues(entity);
            _context.SaveChanges();
        }
    }
}
