using Microsoft.CodeAnalysis.CSharp.Syntax;
using WebApp.Exceptions;
using WebApp.Models.Entities;
using WebApp.Models.ViewModels.Salle;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebApp.Repositories
{
    public class SalleRepository(MyContext context) : ICrudRepository<Salle, (string CinemaNom, int Numero)>
    {
        private readonly MyContext _context = context;

        public bool ExistsById((string CinemaNom, int Numero) entityId)
        {
            return _context.Salles.Find(entityId.CinemaNom, entityId.Numero) != null;
        }

        /// <summary>
        /// Supprime une salle spécifique d'un cinéma.
        /// </summary>
        /// <param name="cinemaNom"></param>
        /// <param name="numero"></param>
        /// <exception cref="KeyNotFoundException"></exception>
        public void Delete((string CinemaNom, int Numero) entityId)
        {
            Salle? salle = _context.Salles.Find(entityId.CinemaNom, entityId.Numero) ?? throw new KeyNotFoundException($"Salle with CinemaNom {entityId.CinemaNom} and Numero {entityId.Numero} not found.");
            _context.Salles.Remove(salle);
            _context.SaveChanges();
        }


        public List<Salle> GetAll()
        {
            return [.. _context.Salles];
        }

        /// <summary>
        /// Récupère toutes les salles d'un cinéma spécifique.
        /// </summary>
        /// <param name="cinemaNom">Nom du cinéma</param>
        /// <returns>Une liste des salles</returns>
        public List<Salle> GetAll(string cinemaNom)
        {
            if (string.IsNullOrWhiteSpace(cinemaNom))
            {
                return [];
            }
            return [.. _context.Salles.Where(s => s.CinemaNom == cinemaNom)];
        }

        public Salle? GetById((string CinemaNom, int Numero) entityId)
        {
            return _context.Salles.Find(entityId.CinemaNom, entityId.Numero);
        }

        public void Insert(Salle entity)
        {
            bool exist = _context.Salles.Find(entity.CinemaNom, entity.Numero) != null;
            if (exist) {
                throw new ExistingException($"Numéro de salle {entity.Numero} déjà existant pour le cinéma {entity.CinemaNom}");
            }
            _context.Salles.Add(entity);
            _context.SaveChanges();
        }

        public void Update(Salle entity)
        {
            var existingSalle = _context.Salles.Find(entity.CinemaNom, entity.Numero) ?? throw new KeyNotFoundException($"Salle with CinemaNom {entity.CinemaNom} and Numero {entity.Numero} not found.");
            _context.Entry(existingSalle).CurrentValues.SetValues(entity);
            _context.SaveChanges();
        }

        public List<Salle> GetAllByCinemaWithFilters(string cinemaNom, SalleFilterModel filters)
        {
            var query = _context.Salles.Where(s => s.CinemaNom == cinemaNom);

            if (filters.Numero.HasValue)
            {
                query = query.Where(s => s.Numero == filters.Numero.Value);
            }

            if (filters.CapaciteMin.HasValue)
            {
                query = query.Where(s => s.Capacite >= filters.CapaciteMin.Value);
            }

            if (filters.CapaciteMax.HasValue)
            {
                query = query.Where(s => s.Capacite <= filters.CapaciteMax.Value);
            }

            if (filters.DateConstructionDebut.HasValue)
            {
                query = query.Where(s => s.DateConstruction >= filters.DateConstructionDebut.Value);
            }

            if (filters.DateConstructionFin.HasValue)
            {
                query = query.Where(s => s.DateConstruction <= filters.DateConstructionFin.Value);
            }

            return [.. query];
        }
    }
}
