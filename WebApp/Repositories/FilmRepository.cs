using Microsoft.EntityFrameworkCore;
using WebApp.Models.Entities;

namespace WebApp.Repositories
{
    public class FilmRepository(MyContext dbContext) : ICrudRepository<Film, int>
    {
        private readonly MyContext _dbContext = dbContext;

        public void Delete(int entityId)
        {
            var film = _dbContext.Films.Find(entityId) ?? throw new KeyNotFoundException($"Film with ID {entityId} not found.");
            _dbContext.Films.Remove(film);
            _dbContext.SaveChanges();
        }

        public List<Film> GetAll()
        {
            return [.. _dbContext.Films];
        }

        public List<Film> GetAllByNameOrTypeOrYear(
            string? searchName = null,
            string? searchType = null,
            int? searchYear = null)
        {
            IQueryable<Film> query = _dbContext.Films.AsQueryable();
            if (!string.IsNullOrEmpty(searchName))
            {
                query = query.Where(f => f.Titre.Contains(searchName));
            }
            if (!string.IsNullOrEmpty(searchType))
            {
                query = query.Where(f => f.Genre.Contains(searchType));
            }
            if (searchYear.HasValue)
            {
                query = query.Where(f => f.Annee == searchYear.Value);
            }
            return [.. query];
        }

        public Film? GetById(int entityId)
        {
            return  _dbContext.Films.Find(entityId);
        }

        public void Insert(Film entity)
        {
            _dbContext.Films.Add(entity);
            _dbContext.SaveChanges();
        }

        public void Update(Film entity)
        {
            var existingFilm = _dbContext.Films.Find(entity.Id) ?? throw new KeyNotFoundException($"Film with ID {entity.Id} not found.");
            _dbContext.Entry(existingFilm).CurrentValues.SetValues(entity);
            _dbContext.SaveChanges();
        }
    }
}
