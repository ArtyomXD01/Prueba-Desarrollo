using Microsoft.AspNetCore.Mvc;
using Models.Modelos;
using System.Text;
using System.Text.Json;


namespace WEB__MVC_.Controllers
{
    public class RegionController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<RegionController> _logger;

        public RegionController(
            IHttpClientFactory httpClientFactory,
            ILogger<RegionController> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        // GET: /Regiones
        // Corresponde a: GET http://.../region
        public async Task<IActionResult> Index()
        {
            return View(new List<Region>());
        }

        // GET: /Regiones
        // Corresponde a: GET http://.../region
        public async Task<IActionResult> GetAllRegiones()
        {
            try
            {
                var client = _httpClientFactory.CreateClient("API");
                var response = await client.GetAsync("api/region");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    };
                    var regiones = JsonSerializer.Deserialize<List<Region>>(json, options) ?? new List<Region>();
                    //return Json(regiones);
                    return Json(regiones);
                }

                _logger.LogWarning("Error al obtener regiones: {StatusCode}", response.StatusCode);
                return Json(new List<Region>());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al conectar con la API");
                TempData["Error"] = "Error al cargar las regiones. Por favor, intente más tarde.";
                return Json(new List<Region>());
            }
        }

        // GET: /Regiones/Details/5
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

        // GET: /Regiones/Comunas/5
        // Corresponde a: GET http://.../region/IdRegion/comuna
        public async Task<IActionResult> Comunas(int id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("API");
                // Obtener comunas de la región
                var comunasResponse = await client.GetAsync($"api/region/{id}/comunas");

                if (comunasResponse.IsSuccessStatusCode)
                {
                    var json = await comunasResponse.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    };
                    var comunas = JsonSerializer.Deserialize<List<Comuna>>(json, options) ?? new List<Comuna>();

                    ViewBag.RegionId = id;

                    return Json(comunas);
                }

                _logger.LogWarning("Error al obtener comunas de región {RegionId}: {StatusCode}",
                    id, comunasResponse.StatusCode);

                ViewBag.RegionId = id;
                return Json(new List<Comuna>());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener comunas de región {RegionId}", id);
                TempData["Error"] = "Error al cargar las comunas de la región.";
                return RedirectToAction(nameof(Index));
            }
        }
        // POST: /Region/Upsert (hace todo menos borrar XD)
        [HttpPost]
        public async Task<IActionResult> UpdSert([FromBody] Region region)
        {
            //if (!ModelState.IsValid)
            //{
            //    return Json(region);
            //}

            try
            {
                var client = _httpClientFactory.CreateClient("API");

                var jsonContent = JsonSerializer.Serialize(region, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("api/region", content);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Región creada exitosamente: {RegionNombre}", region.NombreRegion);
                    TempData["Success"] = $"La región '{region.NombreRegion}' se creó correctamente.";
                    return RedirectToAction(nameof(Index));
                }

                _logger.LogWarning("Error al crear región: {StatusCode}", response.StatusCode);
                ModelState.AddModelError("", "Error al crear la región en el servidor.");
                return Json(region);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error de conexión al crear región");
                ModelState.AddModelError("", "Error de conexión con el servidor.");
                return Json(region);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al crear región");
                ModelState.AddModelError("", "Ocurrió un error inesperado.");
                return Json(region);
            }
        }
    }
}
