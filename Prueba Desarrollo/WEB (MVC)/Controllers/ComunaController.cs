using Microsoft.AspNetCore.Mvc;
using Models.Modelos;
using System.Net.Http;
using System.Text.Json;

namespace WEB__MVC_.Controllers
{
    public class ComunaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        // GET: /Comunas/Details/5
        // Corresponde a: GET http://.../region/IdRegion
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("API");
                var response = await client.GetAsync($"api/region/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    };
                    var region = JsonSerializer.Deserialize<Region>(json, options);

                    if (region != null)
                    {
                        return Json(region);
                    }
                }

                _logger.LogWarning("Región no encontrada: {RegionId}", id);
                TempData["Error"] = "No se encontró la región solicitada.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener detalles de región {RegionId}", id);
                TempData["Error"] = "Error al cargar los detalles de la región.";
                return RedirectToAction(nameof(Index));
            }
        }

    }
}
