using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApp.Models;
using WebApp.Models.Entities;
using WebApp.Models.ViewModels.Home;
using WebApp.Services;

namespace WebApp.Controllers
{
    /// <summary>
    /// Contrôleur principal de l'application gérant la page d'accueil et les fonctionnalités de recherche.
    /// Offre différents types de recherche : par cinéma, par film, et séances du jour.
    /// </summary>
    public class HomeController(CinemaService cinemaService, FilmService filmService, SeanceService seanceService) : Controller
    {
        private readonly CinemaService _cinemaService = cinemaService;
        private readonly FilmService _filmService = filmService;
        private readonly SeanceService _seanceService = seanceService;

        /// <summary>
        /// Action principale de la page d'accueil.
        /// Gère différents types de recherche en fonction du paramètre searchType.
        /// </summary>
        /// <param name="searchType">Type de recherche : "cinema", "film", ou "today". Par défaut : "cinema".</param>
        /// <param name="searchCinema">Terme de recherche pour filtrer les cinémas (optionnel).</param>
        /// <param name="searchFilm">Terme de recherche pour filtrer les films (optionnel).</param>
        /// <param name="cinemaName">Nom spécifique d'un cinéma pour filtrer les séances du jour (optionnel).</param>
        /// <returns>Vue de la page d'accueil avec les données correspondant au type de recherche.</returns>
        public IActionResult Index(string? searchType = "cinema", string? searchCinema = null, string? searchFilm = null, string? cinemaName = null)
        {
            var viewModel = new HomeIndexViewModel
            {
                SearchType = searchType ?? "cinema",
                SearchCinema = searchCinema,
                SearchFilm = searchFilm
            };

            // Dispatching vers la méthode de chargement appropriée selon le type de recherche
            switch (searchType?.ToLower())
            {
                case "film":
                    LoadFilmSeances(viewModel);
                    break;
                case "today":
                    LoadTodaySeances(viewModel, cinemaName);
                    break;
                default:
                    LoadCinemas(viewModel);
                    break;
            }

            return View(viewModel);
        }

        /// <summary>
        /// Charge la liste des cinémas dans le ViewModel.
        /// Applique un filtre de recherche si un terme est fourni.
        /// </summary>
        /// <param name="viewModel">ViewModel à populer avec les données des cinémas.</param>
        private void LoadCinemas(HomeIndexViewModel viewModel)
        {
            viewModel.Cinemas = _cinemaService.GetAllCinemas(viewModel.SearchCinema);
        }

        /// <summary>
        /// Charge les informations sur les films et leurs séances dans le ViewModel.
        /// Si aucun terme de recherche n'est fourni, affiche les 10 premiers films.
        /// Sinon, effectue une recherche et affiche jusqu'à 50 résultats.
        /// </summary>
        /// <param name="viewModel">ViewModel à populer avec les données des films et séances.</param>
        private void LoadFilmSeances(HomeIndexViewModel viewModel)
        {
            if (string.IsNullOrWhiteSpace(viewModel.SearchFilm))
            {
                // Si aucune recherche, afficher quelques films populaires
                var allFilms = _filmService.GetAll().Take(10).ToList();
                foreach (var film in allFilms)
                {
                    var filmSeanceInfo = new FilmSeanceInfo
                    {
                        Film = film,
                        SeancesByCinema = GetSeancesByCinemaForFilm(film.Id)
                    };
                    viewModel.FilmSeances.Add(filmSeanceInfo);
                }
            }
            else
            {
                // Rechercher les films qui correspondent au terme de recherche
                var films = _filmService.GetPaginatedList(viewModel.SearchFilm, null, null, 1, 50);
                foreach (var film in films.Items)
                {
                    var filmSeanceInfo = new FilmSeanceInfo
                    {
                        Film = film,
                        SeancesByCinema = GetSeancesByCinemaForFilm(film.Id)
                    };
                    viewModel.FilmSeances.Add(filmSeanceInfo);
                }
            }
        }

        /// <summary>
        /// Charge les séances programmées pour la date du jour.
        /// Peut être filtrée par un cinéma spécifique si le paramètre cinemaName est fourni.
        /// Les résultats sont groupés par cinéma puis par film, et triés alphabétiquement.
        /// </summary>
        /// <param name="viewModel">ViewModel à populer avec les séances du jour.</param>
        /// <param name="cinemaName">Nom d'un cinéma spécifique pour filtrer les résultats (optionnel).</param>
        private void LoadTodaySeances(HomeIndexViewModel viewModel, string? cinemaName = null)
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            var allSeances = _seanceService.GetAll()
                .Where(s => s.DateSeance == today)
                .ToList();

            // Filtrage par cinéma si spécifié
            if (!string.IsNullOrEmpty(cinemaName))
            {
                allSeances = [.. allSeances.Where(s => s.Salle.CinemaNom.Equals(cinemaName, StringComparison.OrdinalIgnoreCase))];
            }

            // Groupement des séances par cinéma
            var seancesByCinema = allSeances.GroupBy(s => s.Salle.CinemaNom).ToList();

            foreach (var cinemaGroup in seancesByCinema)
            {
                var cinema = _cinemaService.GetOneById(cinemaGroup.Key);
                if (cinema != null)
                {
                    var cinemaSeanceInfo = new CinemaSeanceInfo
                    {
                        Cinema = cinema,
                        // Groupement des séances par film avec tri par horaire
                        FilmSeances = [.. cinemaGroup
                            .GroupBy(s => s.Film.Id)
                            .Select(filmGroup => new FilmSeanceDetail
                            {
                                Film = filmGroup.First().Film,
                                Seances = [.. filmGroup.OrderBy(s => s.Horaire.HeureDebut)]
                            })
                            .OrderBy(f => f.Film.Titre)]
                    };
                    viewModel.TodaySeances.Add(cinemaSeanceInfo);
                }
            }

            // Tri final des cinémas par ordre alphabétique
            viewModel.TodaySeances = [.. viewModel.TodaySeances.OrderBy(c => c.Cinema.Nom)];
        }

        /// <summary>
        /// Récupère toutes les séances d'un film spécifique groupées par cinéma.
        /// Les séances sont triées par date puis par horaire de début.
        /// </summary>
        /// <param name="filmId">Identifiant unique du film.</param>
        /// <returns>Liste des détails de séances groupés par cinéma, triée alphabétiquement par nom de cinéma.</returns>
        private List<CinemaSeanceDetail> GetSeancesByCinemaForFilm(int filmId)
        {
            var seances = _seanceService.GetAll()
                .Where(s => s.Film.Id == filmId)
                .OrderBy(s => s.DateSeance)
                .ThenBy(s => s.Horaire.HeureDebut)
                .ToList();

            var seancesByCinema = seances.GroupBy(s => s.Salle.CinemaNom).ToList();
            var result = new List<CinemaSeanceDetail>();

            foreach (var cinemaGroup in seancesByCinema)
            {
                var cinema = _cinemaService.GetOneById(cinemaGroup.Key);
                if (cinema != null)
                {
                    result.Add(new CinemaSeanceDetail
                    {
                        Cinema = cinema,
                        Seances = [.. cinemaGroup]
                    });
                }
            }

            return [.. result.OrderBy(c => c.Cinema.Nom)];
        }

        /// <summary>
        /// Action de gestion des erreurs de l'application.
        /// Désactive la mise en cache pour s'assurer que les erreurs sont toujours affichées en temps réel.
        /// </summary>
        /// <returns>Vue d'erreur avec les informations de diagnostic.</returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
