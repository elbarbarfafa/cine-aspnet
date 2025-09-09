using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.Models.Entities;
using WebApp.Models.ViewModels;
using WebApp.Models.ViewModels.Seance;
using WebApp.Services;

namespace WebApp.Controllers
{
    [Route("Films/{filmId}/Seances")]
    [Authorize(Roles = "admin")]
    public class SeancesController(ISeanceService seanceService, IFilmService filmService, ISalleService sallesService, IHoraireService horaireService) : Controller
    {
        private readonly ISeanceService _seanceService = seanceService;
        private readonly IFilmService _filmService = filmService;
        private readonly ISalleService _sallesService = sallesService;
        private readonly IHoraireService _horaireService = horaireService;

        // GET: Films/5/Seances
        [HttpGet("")]
        public IActionResult Index(
            int filmId,
            string? salleCinemaNom,
            int? salleNumero,
            DateOnly? dateSeance,
            TimeSpan? heureDebut,
            TimeSpan? heureFin,
            int? pageNumber,
            int? pageSize)
        {
            // Vérifier que le film existe
            var film = _filmService.GetOneById(filmId);
            if (film == null)
            {
                return NotFound();
            }

            ViewData["CurrentFilm"] = film;
            ViewBag.FilmId = filmId;
            ViewBag.SalleCinemaNom = salleCinemaNom;
            ViewBag.SalleNumero = salleNumero;
            ViewBag.DateSeance = dateSeance;
            ViewBag.HeureDebut = heureDebut;
            ViewBag.HeureFin = heureFin;

            var filters = new SeanceFilterModel
            {
                FilmTitre = film.Titre, // Filtrer par le film spécifique
                SalleCinemaNom = salleCinemaNom,
                SalleNumero = salleNumero ?? 0,
                DateSeance = dateSeance,
                HeureDebut = heureDebut ?? TimeSpan.Zero,
                HeureFin = heureFin ?? TimeSpan.Zero
            };

            var paginationParams = new BasePaginationParams(pageNumber ?? 1, pageSize ?? 10);
            var seances = _seanceService.GetAllPaginatedAndFiltered(paginationParams, filters);
            return View(seances);
        }

        // GET: Films/5/Seances/Details/1
        [HttpGet("Details/{id}")]
        public IActionResult Details(int filmId, int id)
        {
            var film = _filmService.GetOneById(filmId);
            if (film == null)
            {
                return NotFound();
            }

            var seance = _seanceService.GetOneById(id);
            if (seance == null || seance.Film.Id != filmId)
            {
                return NotFound();
            }

            ViewData["CurrentFilm"] = film;
            return View(seance);
        }

        // GET: Films/5/Seances/Create
        [HttpGet("Create")]
        public IActionResult Create(int filmId)
        {
            var film = _filmService.GetOneById(filmId);
            if (film == null)
            {
                return NotFound();
            }

            ViewData["CurrentFilm"] = film;
            PopulateViewBags();
            return View(new CreateSeanceViewModel());
        }

        /// <summary>
        /// Crée une nouvelle séance pour le film spécifié.
        /// Valide les entités liées (salle et horaire) avant la création.
        /// </summary>
        /// <param name="filmId">Identifiant du film pour lequel créer la séance.</param>
        /// <param name="viewModel">Données de la séance à créer.</param>
        /// <returns>Redirection vers l'index en cas de succès, ou retour à la vue avec erreurs.</returns>
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(int filmId, CreateSeanceViewModel viewModel)
        {
            var film = _filmService.GetOneById(filmId);
            if (film == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Récupération et validation des entités liées
                var salle = _sallesService.GetOneById((viewModel.SalleCinemaNom, viewModel.SalleNumero));
                var horaire = _horaireService.GetOneById(viewModel.HoraireId);
                
                // Validation de l'existence de la salle
                if (salle == null)
                {
                    ModelState.AddModelError(nameof(viewModel.SalleCinemaNom), "Salle introuvable.");
                    ModelState.AddModelError(nameof(viewModel.SalleNumero), "Vérifiez le cinéma et le numéro de salle.");
                }
                
                // Validation de l'existence de l'horaire
                if (horaire == null)
                {
                    ModelState.AddModelError(nameof(viewModel.HoraireId), "Horaire introuvable.");
                }

                // Si les entités liées sont trouvées, créer la séance
                if (salle != null && horaire != null)
                {
                    var seance = new WebApp.Models.Entities.Seance
                    {
                        Tarif = viewModel.Tarif,
                        DateSeance = viewModel.DateSeance,
                        Film = film,
                        Salle = salle,
                        Horaire = horaire
                    };

                    try
                    {
                        _seanceService.Add(seance);
                        TempData["SuccessMessage"] = "Séance créée avec succès.";
                        return RedirectToAction(nameof(Index), new { filmId });
                    }
                    catch (Exception ex)
                    {
                        // Capture et affiche les erreurs métier (ex: conflit d'horaires)
                        ModelState.AddModelError("", ex.Message);
                    }
                }
            }

            // En cas d'erreur, retourner la vue avec le modèle et les erreurs
            ViewData["CurrentFilm"] = film;
            PopulateViewBags();
            return View(viewModel);
        }

        // GET: Films/5/Seances/Edit/1
        [HttpGet("Edit/{id}")]
        public IActionResult Edit(int filmId, int id)
        {
            var film = _filmService.GetOneById(filmId);
            if (film == null)
            {
                return NotFound();
            }

            var seance = _seanceService.GetOneById(id);
            if (seance == null || seance.Film.Id != filmId)
            {
                return NotFound();
            }

            ViewData["CurrentFilm"] = film;
            PopulateViewBags();
            
            // Créer le ViewModel à partir de la séance existante
            var viewModel = EditSeanceViewModel.FromSeance(seance);
            return View(viewModel);
        }

        /// <summary>
        /// Met à jour une séance existante pour le film spécifié.
        /// Valide les entités liées et l'existence de la séance avant la mise à jour.
        /// </summary>
        /// <param name="filmId">Identifiant du film auquel appartient la séance.</param>
        /// <param name="id">Identifiant de la séance à modifier.</param>
        /// <param name="viewModel">Nouvelles données de la séance.</param>
        /// <returns>Redirection vers l'index en cas de succès, ou retour à la vue avec erreurs.</returns>
        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int filmId, int id, EditSeanceViewModel viewModel)
        {
            // Vérification de cohérence des identifiants
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            var film = _filmService.GetOneById(filmId);
            if (film == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Récupération et validation des entités liées
                var salle = _sallesService.GetOneById((viewModel.SalleCinemaNom, viewModel.SalleNumero));
                var horaire = _horaireService.GetOneById(viewModel.HoraireId);
                
                // Validation de l'existence de la salle
                if (salle == null)
                {
                    ModelState.AddModelError(nameof(viewModel.SalleCinemaNom), "Salle introuvable.");
                    ModelState.AddModelError(nameof(viewModel.SalleNumero), "Vérifiez le cinéma et le numéro de salle.");
                }
                
                // Validation de l'existence de l'horaire
                if (horaire == null)
                {
                    ModelState.AddModelError(nameof(viewModel.HoraireId), "Horaire introuvable.");
                }

                // Si les entités liées sont trouvées, mettre à jour la séance
                if (salle != null && horaire != null)
                {
                    var seance = new WebApp.Models.Entities.Seance
                    {
                        Id = viewModel.Id,
                        Tarif = viewModel.Tarif,
                        DateSeance = viewModel.DateSeance,
                        Film = film,
                        Salle = salle,
                        Horaire = horaire
                    };

                    try
                    {
                        _seanceService.Update(seance);
                        TempData["SuccessMessage"] = "Séance modifiée avec succès.";
                        return RedirectToAction(nameof(Index), new { filmId });
                    }
                    catch (KeyNotFoundException)
                    {
                        return NotFound();
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", ex.Message);
                    }
                }
            }

            // En cas d'erreur, retourner la vue avec le modèle et les erreurs
            ViewData["CurrentFilm"] = film;
            PopulateViewBags();
            return View(viewModel);
        }

        // GET: Films/5/Seances/Delete/1
        [HttpGet("Delete/{id}")]
        public IActionResult Delete(int filmId, int id)
        {
            var film = _filmService.GetOneById(filmId);
            if (film == null)
            {
                return NotFound();
            }

            var seance = _seanceService.GetOneById(id);
            if (seance == null || seance.Film.Id != filmId)
            {
                return NotFound();
            }

            ViewData["CurrentFilm"] = film;
            return View(seance);
        }

        // POST: Films/5/Seances/Delete/1
        [HttpPost("Delete/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int filmId, int id)
        {
            var film = _filmService.GetOneById(filmId);
            if (film == null)
            {
                return NotFound();
            }

            try
            {
                var seance = _seanceService.GetOneById(id);
                if (seance == null || seance.Film.Id != filmId)
                {
                    return NotFound();
                }

                _seanceService.Delete(id);
                TempData["SuccessMessage"] = "Séance supprimée avec succès.";
                return RedirectToAction(nameof(Index), new { filmId });
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index), new { filmId });
            }
        }

        private void PopulateViewBags()
        {
            ViewBag.Salles = new SelectList(_sallesService.GetAll().Select(s => new
            {
                Value = $"{s.CinemaNom},{s.Numero}",
                Text = $"{s.CinemaNom} - Salle {s.Numero}"
            }), "Value", "Text");


            // Récupération des horaires depuis le service
            var horaires = _horaireService.GetAll();


            ViewBag.Horaires = new SelectList(horaires.Select(h => new
            {
                h.Id,
                Display = $"{h.HeureDebut:hh\\:mm} - {h.HeureFin:hh\\:mm}"
            }), "Id", "Display");
        }
    }
}