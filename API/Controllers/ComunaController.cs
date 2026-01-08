using DataAccess.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models.Modelos;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComunaController : ControllerBase
    {
        private readonly IComunaRepo _comunaRepository;
        public ComunaController(IComunaRepo comunaRepository)
        {
            _comunaRepository = comunaRepository;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var comuna = await _comunaRepository.GetByIdAsync(id);
            if (comuna == null) return NotFound();
            return Ok(comuna);
        }

        [HttpPost()]
        public async Task<IActionResult> Update(Comuna comuna)
        {
            var result = await _comunaRepository.UpdateAsync(comuna);
            if (!result) return NotFound();
            return Ok();
        }
    }
}
