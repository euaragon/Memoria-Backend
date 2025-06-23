using MemoriaAPI.Models;
using MemoriaAPI.Service;
using Microsoft.AspNetCore.Mvc;


namespace MemoriaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SeccionController : ControllerBase
    {
        private readonly ISeccionService _service;
        private readonly ILogger<SeccionController> _logger;

        public SeccionController(ISeccionService service, ILogger<SeccionController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Seccion>>> Get()
        {
            _logger.LogInformation("🔍 [GET] api/seccion llamado.");

            var secciones = await _service.GetAllAsync();

            _logger.LogInformation("📦 {Cantidad} secciones recuperadas.", secciones.Count());

            return Ok(secciones);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Seccion>> Get(int id)
        {
            _logger.LogInformation("🔍 [GET] api/seccion/{Id} llamado.", id);

            var seccion = await _service.GetByIdAsync(id);

            if (seccion == null)
            {
                _logger.LogWarning("⚠️ Sección con ID {Id} no encontrada.", id);
                return NotFound();
            }

            return Ok(seccion);
        }

        [HttpPost]
        public async Task<ActionResult<Seccion>> Post(Seccion seccion)
        {
            _logger.LogInformation("📥 [POST] api/seccion - Creando nueva sección: {Nombre}", seccion.Nombre);

            var nueva = await _service.CreateAsync(seccion);

            _logger.LogInformation("✅ Sección creada con ID {IdSeccion}.", nueva.IdSeccion);

            return CreatedAtAction(nameof(Get), new { id = nueva.IdSeccion }, nueva);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Seccion seccion)
        {
            _logger.LogInformation("✏️ [PUT] api/seccion/{Id} - Actualizando sección.", id);

            var actualizado = await _service.UpdateAsync(id, seccion);

            if (!actualizado)
            {
                _logger.LogWarning("⚠️ No se pudo actualizar. Sección con ID {Id} no encontrada.", id);
                return NotFound();
            }

            _logger.LogInformation("✅ Sección con ID {Id} actualizada correctamente.", id);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("🗑️ [DELETE] api/seccion/{Id} - Intentando eliminar sección.", id);

            var eliminado = await _service.DeleteAsync(id);

            if (!eliminado)
            {
                _logger.LogWarning("⚠️ No se pudo eliminar. Sección con ID {Id} no encontrada.", id);
                return NotFound();
            }

            _logger.LogInformation("✅ Sección con ID {Id} eliminada correctamente.", id);
            return NoContent();
        }
    }
}
