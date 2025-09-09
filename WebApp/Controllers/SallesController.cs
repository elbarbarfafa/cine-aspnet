using AspNetCoreGeneratedDocument;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Models.Entities;
using WebApp.Models.ViewModels;
using WebApp.Models.ViewModels.Salle;
using WebApp.Repositories;
using WebApp.Services;

namespace WebApp.Controllers
{
    [Authorize(Roles = "admin")]
    public class SallesController(ISalleService sallesService) : Controller
    {
        private readonly ISalleService _sallesService = sallesService;

        public IActionResult Index(string cinemaNom,
            int? pageNumber,
            int? pageSize,
            int? capaciteMin,
            int? capaciteMax,
            DateTime? dateConstructionDebut,
            DateTime? dateConstructionFin)
        {
            if (string.IsNullOrEmpty(cinemaNom))
            {
                return NotFound();
            }

            ViewData["CurrentCinema"] = cinemaNom;
            ViewBag.CapaciteMin = capaciteMin;
            ViewBag.CapaciteMax = capaciteMax;
            ViewBag.DateConstructionDebut = dateConstructionDebut;
            ViewBag.DateConstructionFin = dateConstructionFin;

            var salles = _sallesService.GetAllPaginatedAndFiltered(new BasePaginationParams(pageNumber, pageSize), new SalleFilterModel(cinemaNom, null, capaciteMin, capaciteMax, dateConstructionDebut.HasValue ? DateOnly.FromDateTime(dateConstructionDebut.Value) : null, dateConstructionFin.HasValue ? DateOnly.FromDateTime(dateConstructionFin.Value) : null));
            return View(salles);
        }

        public IActionResult Details(string cinemaNom, int numero)
        {
            if (string.IsNullOrEmpty(cinemaNom))
            {
                return NotFound();
            }
            ViewData["CurrentCinema"] = cinemaNom;

            var salle = _sallesService.GetOneById((cinemaNom, numero));
            if (salle == null)
            {
                return NotFound();
            }

            return View(salle);
        }

        public IActionResult Create(string cinemaNom)
        {
            if (string.IsNullOrEmpty(cinemaNom))
            {
                return NotFound();
            }

            ViewData["CurrentCinema"] = cinemaNom;
            return View(new CreateSalleViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(string cinemaNom, CreateSalleViewModel viewModel)
        {
            if (string.IsNullOrEmpty(cinemaNom))
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var salle = new Salle
                {
                    CinemaNom = cinemaNom,
                    Numero = viewModel.Numero,
                    Capacite = viewModel.Capacite,
                    DateConstruction = viewModel.DateConstruction
                };

                try
                {
                    _sallesService.Add(salle);
                    TempData["SuccessMessage"] = "Salle créée avec succès.";
                    return RedirectToAction(nameof(Index), new { cinemaNom });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Une erreur s'est produite lors de la création de la salle : " + ex.Message);
                }
            }

            ViewData["CurrentCinema"] = cinemaNom;
            return View(viewModel);
        }

        public IActionResult Edit(string cinemaNom, int numero)
        {
            if (string.IsNullOrEmpty(cinemaNom))
            {
                return NotFound();
            }

            var salle = _sallesService.GetOneById((cinemaNom, numero));
            if (salle == null)
            {
                return NotFound();
            }

            ViewData["CurrentCinema"] = cinemaNom;
            ViewBag.SalleNumero = numero; // Ajouter le numéro de salle pour la vue
            var viewModel = EditSalleViewModel.FromSalle(salle);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string cinemaNom, int numero, EditSalleViewModel viewModel)
        {
            if (string.IsNullOrEmpty(cinemaNom))
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var salle = new Salle
                {
                    CinemaNom = cinemaNom,
                    Numero = numero,
                    Capacite = viewModel.Capacite,
                    DateConstruction = viewModel.DateConstruction
                };

                try
                {
                    _sallesService.Update(salle);
                    TempData["SuccessMessage"] = "Salle modifiée avec succès.";
                    return RedirectToAction(nameof(Index), new { cinemaNom });
                }
                catch (KeyNotFoundException)
                {
                    return NotFound();
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Une erreur s'est produite lors de la modification de la salle : " + ex.Message);
                }
            }

            ViewData["CurrentCinema"] = cinemaNom;
            ViewBag.SalleNumero = numero; // Maintenir le numéro de salle en cas d'erreur
            return View(viewModel);
        }

        public IActionResult Delete(string cinemaNom, int numero)
        {
            if (string.IsNullOrEmpty(cinemaNom))
            {
                return NotFound();
            }

            var salle = _sallesService.GetOneById((cinemaNom, numero));
            if (salle == null)
            {
                return NotFound();
            }
            ViewData["CurrentCinema"] = cinemaNom;
            return View(salle);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(string cinemaNom, int numero)
        {
            try
            {
                _sallesService.Delete((cinemaNom, numero));
                TempData["SuccessMessage"] = "Salle supprimée avec succès.";
                return RedirectToAction(nameof(Index), new { cinemaNom });
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Une erreur s'est produite lors de la suppression de la salle : " + ex.Message;
                return RedirectToAction(nameof(Index), new { cinemaNom });
            }
        }
    }
}
