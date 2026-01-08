using DataAccess.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
        // GET: api/<ComunaController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var comuna = await _comunaRepository.GetByIdAsync(id);
            if (comuna == null) return NotFound();
            return Ok(comuna);
        }
    }
}
