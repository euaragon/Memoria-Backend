using MemoriaAPI.Models;
using MemoriaAPI.Service;
using Microsoft.AspNetCore.Mvc;

namespace MemoriaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaginaController : ControllerBase
    {
        private readonly IPaginaService _service;
        private readonly ILogger<PaginaController> _logger;

        public PaginaController(IPaginaService service, ILogger<PaginaController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pagina>>> Get()
        {
            _logger.LogInformation("🔍 [GET] api/pagina llamado.");

            var paginas = await _service.GetAllAsync();

            _logger.LogInformation("📄 {Cantidad} páginas recuperadas.", paginas.Count());

            return Ok(paginas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Pagina>> Get(int id)
        {
            _logger.LogInformation("🔍 [GET] api/pagina/{Id} llamado.", id);

            var pagina = await _service.GetByIdAsync(id);

            if (pagina == null)
            {
                _logger.LogWarning("⚠️ Página con ID {Id} no encontrada.", id);
                return NotFound();
            }

            return Ok(pagina);
        }

        [HttpPost]
        public async Task<ActionResult<Pagina>> Post(Pagina pagina)
        {
            _logger.LogInformation("📥 [POST] api/pagina - Creando nueva página: {Nombre}", pagina.Nombre);

            var nueva = await _service.CreateAsync(pagina);

            _logger.LogInformation("✅ Página creada con ID {Id}.", nueva.IdPagina);

            return CreatedAtAction(nameof(Get), new { id = nueva.IdPagina }, nueva);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Pagina pagina)
        {
            _logger.LogInformation("✏️ [PUT] api/pagina/{Id} - Actualizando página.", id);

            var actualizado = await _service.UpdateAsync(id, pagina);

            if (!actualizado)
            {
                _logger.LogWarning("⚠️ No se pudo actualizar. Página con ID {Id} no encontrada.", id);
                return NotFound();
            }

            _logger.LogInformation("✅ Página con ID {Id} actualizada correctamente.", id);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("🗑️ [DELETE] api/pagina/{Id} - Intentando eliminar página.", id);

            var eliminado = await _service.DeleteAsync(id);

            if (!eliminado)
            {
                _logger.LogWarning("⚠️ No se pudo eliminar. Página con ID {Id} no encontrada.", id);
                return NotFound();
            }

            _logger.LogInformation("✅ Página con ID {Id} eliminada correctamente.", id);
            return NoContent();
        }
    }
}
