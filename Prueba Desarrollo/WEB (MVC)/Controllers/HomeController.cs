using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WEB__MVC_.Models;

namespace WEB__MVC_.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            // Redirigir directamente a la vista de regiones
            return RedirectToAction("Index", "Region");
        }
    }
}
