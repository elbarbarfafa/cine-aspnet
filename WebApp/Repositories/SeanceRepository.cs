using Microsoft.EntityFrameworkCore;
using WebApp.Models.Entities;
using WebApp.Models.ViewModels.Seance;

namespace WebApp.Repositories
{
    public class SeanceRepository(MyContext myContext) : ISeanceRepository
    {
        private readonly MyContext _myContext = myContext;

        public void Delete(int entityId)
        {
            var seance = _myContext.Seances.Find(entityId) ?? throw new KeyNotFoundException($"Séance with ID {entityId} not found.");
            _myContext.Seances.Remove(seance);
            _myContext.SaveChanges();
        }

        public List<Seance> GetAll()
        {
            return [.. _myContext.Seances
                .Include(s => s.Film)
                .Include(s => s.Salle)
                .Include(s => s.Horaire)];
        }

        public List<Seance> GetAllWithFilters(SeanceFilterModel filters)
        {
            var query = _myContext.Seances
                .Include(s => s.Film)
                .Include(s => s.Salle)
                .Include(s => s.Horaire)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filters.SalleCinemaNom))
            {
                query = query.Where(s => s.Salle.CinemaNom == filters.SalleCinemaNom);
            }

            if (filters.SalleNumero > 0)
            {
                query = query.Where(s => s.Salle.Numero == filters.SalleNumero);
            }

            if (!string.IsNullOrWhiteSpace(filters.FilmTitre))
            {
                query = query.Where(s => s.Film.Titre.Contains(filters.FilmTitre));
            }

            if (filters.DateSeance.HasValue)
            {
                query = query.Where(s => s.DateSeance == filters.DateSeance.Value);
            }

            if (filters.HeureDebut != TimeSpan.Zero)
            {
                query = query.Where(s => s.Horaire.HeureDebut >= filters.HeureDebut);
            }

            if (filters.HeureFin != TimeSpan.Zero)
            {
                query = query.Where(s => s.Horaire.HeureFin <= filters.HeureFin);
            }

            return [.. query];
        }

        public Seance? GetById(int entityId)
        {
            return _myContext.Seances
                .Include(s => s.Film)
                .Include(s => s.Salle)
                .Include(s => s.Horaire)
                .FirstOrDefault(s => s.Id == entityId);
        }

        public void Insert(Seance entity)
        {
            // Vérification que le film existe
            var film = _myContext.Films.Find(entity.Film.Id);
            if (film == null)
            {
                throw new KeyNotFoundException($"Film with ID {entity.Film.Id} not found.");
            }

            // Vérification que la salle existe
            var salle = _myContext.Salles.Find(entity.Salle.CinemaNom, entity.Salle.Numero);
            if (salle == null)
            {
                throw new KeyNotFoundException($"Salle not found for cinema {entity.Salle.CinemaNom} and number {entity.Salle.Numero}.");
            }

            // Vérification que l'horaire existe
            var horaire = _myContext.Horaires.Find(entity.Horaire.Id);
            if (horaire == null)
            {
                throw new KeyNotFoundException($"Horaire with ID {entity.Horaire.Id} not found.");
            }

            // Vérification des chevauchements de séances pour la même salle
            var existingSeance = _myContext.Seances
                .Any(s => s.Salle.CinemaNom == entity.Salle.CinemaNom 
                      && s.Salle.Numero == entity.Salle.Numero
                      && s.DateSeance == entity.DateSeance
                      && s.Horaire.Id == entity.Horaire.Id);

            if (existingSeance)
            {
                throw new InvalidOperationException("Une séance existe déjà dans cette salle à cet horaire.");
            }

            _myContext.Seances.Add(entity);
            _myContext.SaveChanges();
        }

        public void Update(Seance entity)
        {
            var existingSeance = _myContext.Seances
                .Include(s => s.Film)
                .Include(s => s.Salle)
                .Include(s => s.Horaire)
                .FirstOrDefault(s => s.Id == entity.Id) ?? throw new KeyNotFoundException($"Séance with ID {entity.Id} not found.");

            // Vérification des chevauchements (seulement si la salle ou l'horaire change)
            if (existingSeance.Salle.CinemaNom != entity.Salle.CinemaNom 
                || existingSeance.Salle.Numero != entity.Salle.Numero
                || existingSeance.Horaire.Id != entity.Horaire.Id
                || existingSeance.DateSeance != entity.DateSeance)
            {
                var overlap = _myContext.Seances
                    .Any(s => s.Id != entity.Id
                          && s.Salle.CinemaNom == entity.Salle.CinemaNom 
                          && s.Salle.Numero == entity.Salle.Numero
                          && s.DateSeance == entity.DateSeance
                          && s.Horaire.Id == entity.Horaire.Id);

                if (overlap)
                {
                    throw new InvalidOperationException("Une séance existe déjà dans cette salle à cet horaire.");
                }
            }

            // Mise à jour des propriétés
            existingSeance.Tarif = entity.Tarif;
            existingSeance.DateSeance = entity.DateSeance;
            existingSeance.Film = _myContext.Films.Find(entity.Film.Id) 
                ?? throw new KeyNotFoundException($"Film with ID {entity.Film.Id} not found.");
            existingSeance.Salle = _myContext.Salles.Find(entity.Salle.CinemaNom, entity.Salle.Numero) 
                ?? throw new KeyNotFoundException($"Salle not found.");
            existingSeance.Horaire = _myContext.Horaires.Find(entity.Horaire.Id) 
                ?? throw new KeyNotFoundException($"Horaire with ID {entity.Horaire.Id} not found.");

            _myContext.SaveChanges();
        }
    }
}
