using DataAccess.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Modelos;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegionController : ControllerBase
    {
        private readonly IRegionRepo _regionRepository;

        public RegionController(IRegionRepo regionRepository)
        {
            _regionRepository = regionRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var regiones = await _regionRepository.GetAllAsync();
            return Ok(regiones);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var region = await _regionRepository.GetByIdAsync(id);
            if (region == null) return NotFound();
            return Ok(region);
        }

        [HttpGet("{id}/comunas")]
        public async Task<IActionResult> GetComunasByRegion(int id)
        {
            var comunas = await _regionRepository.GetComunasByRegionIdAsync(id);
            return Ok(comunas);
        }

        [HttpPost()]
        public async Task<IActionResult> Update(Region region)
        {
            var result = await _regionRepository.UpdateAsync(region);
            if (!result) return NotFound();
            return Ok();
        }
    }
}
