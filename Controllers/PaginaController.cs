using MemoriaAPI.Models.DTO; // --> ¡Importante! Añadir el using para los DTOs
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

        // --- GET ALL ---
        [HttpGet]
        // --> Devuelve una lista de DTOs de lectura.
        public async Task<ActionResult<IEnumerable<PaginaDTO>>> Get()
        {
            _logger.LogInformation("🔍 [GET] api/pagina llamado.");
            var paginas = await _service.GetAllAsync();

            // --> Mapeamos los modelos de BD a DTOs.
            var paginasDto = paginas.Select(p => new PaginaDTO
            {
                IdPagina = p.IdPagina,
                Nombre = p.Nombre,
                Url = p.Url,
                Orden = p.Orden
            });

            _logger.LogInformation("📄 {Cantidad} páginas recuperadas.", paginasDto.Count());
            return Ok(paginasDto);
        }

        // --- GET BY ID ---
        [HttpGet("{id}")]
        // --> Devuelve un solo DTO de lectura.
        public async Task<ActionResult<PaginaDTO>> Get(int id)
        {
            _logger.LogInformation("🔍 [GET] api/pagina/{Id} llamado.", id);
            var pagina = await _service.GetByIdAsync(id);

            if (pagina == null)
            {
                _logger.LogWarning("⚠️ Página con ID {Id} no encontrada.", id);
                return NotFound();
            }

            // --> Mapeamos el modelo encontrado a un DTO.
            var paginaDto = new PaginaDTO
            {
                IdPagina = pagina.IdPagina,
                Nombre = pagina.Nombre,
                Url = pagina.Url,
                Orden = pagina.Orden
            };

            return Ok(paginaDto);
        }

        // --- POST ---
        [HttpPost]
        // --> Recibe un DTO de escritura y devuelve uno de lectura.
        public async Task<ActionResult<PaginaDTO>> Post([FromBody] PaginaCreateUpdateDTO paginaDto)
        {
            _logger.LogInformation("📥 [POST] api/pagina - Creando nueva página: {Nombre}", paginaDto.Nombre);

            // --> Pasamos el DTO al servicio, que es lo que espera la interfaz.
            var nuevaPagina = await _service.CreateAsync(paginaDto);

            // --> Mapeamos el nuevo modelo creado a un DTO para devolverlo.
            var nuevaPaginaDto = new PaginaDTO
            {
                IdPagina = nuevaPagina.IdPagina,
                Nombre = nuevaPagina.Nombre,
                Url = nuevaPagina.Url,
                Orden = nuevaPagina.Orden
            };

            _logger.LogInformation("✅ Página creada con ID {Id}.", nuevaPaginaDto.IdPagina);
            return CreatedAtAction(nameof(Get), new { id = nuevaPaginaDto.IdPagina }, nuevaPaginaDto);
        }

        // --- PUT ---
        [HttpPut("{id}")]
        // --> Recibe un DTO de escritura.
        public async Task<IActionResult> Put(int id, [FromBody] PaginaCreateUpdateDTO paginaDto)
        {
            _logger.LogInformation("✏️ [PUT] api/pagina/{Id} - Actualizando página.", id);

            // --> Pasamos el DTO al servicio.
            var actualizado = await _service.UpdateAsync(id, paginaDto);

            if (!actualizado)
            {
                _logger.LogWarning("⚠️ No se pudo actualizar. Página con ID {Id} no encontrada.", id);
                return NotFound();
            }

            _logger.LogInformation("✅ Página con ID {Id} actualizada correctamente.", id);
            return NoContent();
        }

        // --- DELETE ---
        // Este método ya estaba perfecto y no necesita cambios.
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