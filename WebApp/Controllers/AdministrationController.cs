using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdministrationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
