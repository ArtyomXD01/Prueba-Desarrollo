using Microsoft.AspNetCore.Mvc;
using Models.Modelos;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace WEB__MVC_.Controllers
{
    public class ComunaController : Controller
    {

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<ComunaController> _logger;

        public ComunaController(
            IHttpClientFactory httpClientFactory,
            ILogger<ComunaController> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }

        // GET: /Comuna/Details/5
        // Corresponde a: GET http://.../comuna/IdRegion
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("API");
                var response = await client.GetAsync($"api/comuna/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    };
                    var region = JsonSerializer.Deserialize<Comuna>(json, options);

                    if (region != null)
                    {
                        return Json(region);
                    }
                }

                _logger.LogWarning("Comuna no encontrada: {IdComuna}", id);
                TempData["Error"] = "No se encontró la comuna solicitada.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener detalles de comuna {IdComuna}", id);
                TempData["Error"] = "Error al cargar los detalles de la comuna.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: /Comuna/Details/5
        // Corresponde a: Post http://.../comuna/
        public async Task<IActionResult> UpdSert([FromBody] Comuna comuna)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("API");

                var jsonContent = JsonSerializer.Serialize(comuna, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("api/comuna", content);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Comuna guardada exitosamente: {NombreComuna}", comuna.NombreComuna);
                    TempData["Success"] = $"La comuna '{comuna.NombreComuna}' se guardó correctamente.";
                    return RedirectToAction(nameof(Index));
                }

                _logger.LogWarning("Error al crear comuna: {StatusCode}", response.StatusCode);
                ModelState.AddModelError("", "Error al crear la comuna en el servidor.");
                return Json(comuna);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error de conexión al crear comuna");
                ModelState.AddModelError("", "Error de conexión con el servidor.");
                return Json(comuna);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al crear comuna");
                ModelState.AddModelError("", "Ocurrió un error inesperado.");
                return Json(comuna);
            }
        }

    }
}
