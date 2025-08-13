using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Models.Entities;
using WebApp.Models.ViewModels.Film;
using WebApp.Repositories;
using WebApp.Services;

namespace WebApp.Controllers
{
    [Authorize(Roles = "admin")]
    public class FilmsController(FilmService filmService) : Controller
    {
        private readonly FilmService _filmService = filmService;

        public IActionResult Index(
            string? searchName,
            string? searchType,
            int? searchYear,
            int? pageNumber, int? pageSize
            )
        {
            ViewBag.SearchName = searchName;
            ViewBag.SearchType = searchType;
            ViewBag.SearchYear = searchYear;

            var films = _filmService.GetPaginatedList(searchName, searchType, searchYear, pageNumber ?? 1, pageSize ?? 10);

            return View(films);
        }

        // GET: Films/Details/:id
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var film = _filmService.GetOneById(id.Value);
            if (film == null)
            {
                return NotFound();
            }
            return View(film);
        }

        // GET: Films/Create
        public IActionResult Create()
        {
            return View(new CreateFilmViewModel());
        }

        // POST: Films/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateFilmViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var film = new Film
                {
                    Titre = viewModel.Titre,
                    Annee = viewModel.Annee,
                    Genre = viewModel.Genre
                };

                try
                {
                    _filmService.Add(film);
                    TempData["SuccessMessage"] = "Film créé avec succès.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View(viewModel);
        }

        // GET: Films/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var film = _filmService.GetOneById(id.Value);
            if (film == null)
            {
                return NotFound();
            }
            var viewModel = EditFilmViewModel.FromFilm(film);
            return View(viewModel);
        }

        // POST: Films/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, EditFilmViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var film = new Film
                {
                    Id = viewModel.Id,
                    Titre = viewModel.Titre,
                    Annee = viewModel.Annee,
                    Genre = viewModel.Genre
                };

                try
                {
                    _filmService.Update(film);
                    TempData["SuccessMessage"] = "Film modifié avec succès.";
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
            return View(viewModel);
        }

        // GET: Films/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var film = _filmService.GetOneById(id.Value);
            if (film == null)
            {
                return NotFound();
            }
            return View(film);
        }

        // POST: Films/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                _filmService.Delete(id);
                TempData["SuccessMessage"] = "Film supprimé avec succès.";
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
