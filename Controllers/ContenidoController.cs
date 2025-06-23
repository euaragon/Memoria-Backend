using MemoriaAPI.Models;
using MemoriaAPI.Service;
using Microsoft.AspNetCore.Mvc;

namespace MemoriaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContenidoController : ControllerBase
    {
        private readonly IContenidoService _service;
        private readonly ILogger<ContenidoController> _logger;

        public ContenidoController(IContenidoService service, ILogger<ContenidoController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Contenido>>> Get()
        {
            _logger.LogInformation("🔍 [GET] api/contenido llamado.");

            var contenidos = await _service.GetAllAsync();

            _logger.LogInformation("📦 Se recuperaron {Cantidad} contenidos.", contenidos.Count());

            return Ok(contenidos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Contenido>> Get(int id)
        {
            _logger.LogInformation("🔍 [GET] api/contenido/{Id} llamado.", id);

            var contenido = await _service.GetByIdAsync(id);

            if (contenido == null)
            {
                _logger.LogWarning("⚠️ Contenido con ID {Id} no encontrado.", id);
                return NotFound();
            }

            return Ok(contenido);
        }

        [HttpPost]
        public async Task<ActionResult<Contenido>> Post(Contenido contenido)
        {
            _logger.LogInformation("📥 [POST] api/contenido - Creando nuevo contenido en sección {IdSeccion}.", contenido.IdSeccion);

            var nuevo = await _service.CreateAsync(contenido);

            _logger.LogInformation("✅ Contenido creado con ID {IdContenido}.", nuevo.IdContenido);

            return CreatedAtAction(nameof(Get), new { id = nuevo.IdContenido }, nuevo);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Contenido contenido)
        {
            _logger.LogInformation("✏️ [PUT] api/contenido/{Id} - Actualizando contenido.", id);

            var actualizado = await _service.UpdateAsync(id, contenido);

            if (!actualizado)
            {
                _logger.LogWarning("⚠️ No se pudo actualizar. Contenido con ID {Id} no encontrado.", id);
                return NotFound();
            }

            _logger.LogInformation("✅ Contenido con ID {Id} actualizado correctamente.", id);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("🗑️ [DELETE] api/contenido/{Id} - Intentando eliminar contenido.", id);

            var eliminado = await _service.DeleteAsync(id);

            if (!eliminado)
            {
                _logger.LogWarning("⚠️ No se pudo eliminar. Contenido con ID {Id} no encontrado.", id);
                return NotFound();
            }

            _logger.LogInformation("✅ Contenido con ID {Id} eliminado correctamente.", id);
            return NoContent();
        }
    }
}
