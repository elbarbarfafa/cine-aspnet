using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models.Entities;
using WebApp.Models.ViewModels;
using WebApp.Models.ViewModels.Cinema;
using WebApp.Services;

namespace WebApp.Controllers
{
    [Authorize(Roles = "admin")]
    public class CinemasController(CinemaService cinemaService) : Controller
    {
        private readonly CinemaService _cinemaService = cinemaService;

        public IActionResult Index(string searchString, int? pageNumber, int? pageSize)
        {
            ViewData["CurrentFilter"] = searchString;
            int page = pageNumber ?? 1;
            int size = pageSize ?? 5;

            var cinemas = _cinemaService.GetAllPaginatedAndFiltered(new BasePaginationParams(page, size), new CinemaFilterModel(searchString));
            return View(cinemas);
        }

        public IActionResult Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cinema = _cinemaService.GetOneById(id);
            if (cinema == null)
            {
                return NotFound();
            }

            return View(cinema);
        }

        public IActionResult Create()
        {
            return View(new CreateCinemaViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateCinemaViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var cinema = new Cinema
                {
                    Nom = viewModel.Nom,
                    Rue = viewModel.Rue,
                    Numero = viewModel.Numero
                };

                try
                {
                    _cinemaService.Add(cinema);
                    TempData["SuccessMessage"] = "Cinéma créé avec succès.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View(viewModel);
        }

        public IActionResult Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cinema = _cinemaService.GetOneById(id);
            if (cinema == null)
            {
                return NotFound();
            }

            ViewData["CinemaName"] = cinema.Nom;
            var viewModel = EditCinemaViewModel.FromCinema(cinema);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, EditCinemaViewModel viewModel)
        {
            var existingCinema = _cinemaService.GetOneById(id);
            if (existingCinema == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var cinema = new Cinema
                {
                    Nom = id,
                    Rue = viewModel.Rue,
                    Numero = viewModel.Numero
                };

                try
                {
                    _cinemaService.Update(cinema);
                    TempData["SuccessMessage"] = "Cinéma modifié avec succès.";
                    return RedirectToAction(nameof(Index));
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

            ViewData["CinemaName"] = id;
            return View(viewModel);
        }

        public IActionResult Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cinema = _cinemaService.GetOneById(id);
            if (cinema == null)
            {
                return NotFound();
            }

            return View(cinema);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(string id)
        {
            try
            {
                _cinemaService.Delete(id);
                TempData["SuccessMessage"] = "Cinéma supprimé avec succès.";
                return RedirectToAction(nameof(Index));
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
