using MemoriaAPI.Models.DTO; // --> ¡Importante! Añadir el using para los DTOs
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

        // --- GET ALL ---
        [HttpGet]
        // --> Devuelve una lista de DTOs, no el modelo de la BD.
        public async Task<ActionResult<IEnumerable<ContenidoDTO>>> Get()
        {
            _logger.LogInformation("🔍 [GET] api/contenido llamado.");
            var contenidos = await _service.GetAllAsync();

            // --> Mapeamos la lista de modelos a una lista de DTOs.
            var contenidosDto = contenidos.Select(c => new ContenidoDTO
            {
                IdContenido = c.IdContenido,
                Titulo = c.Titulo,
                Texto = c.Texto,
                FechaPublicacion = c.FechaPublicacion
                // Nota: Si quisieras info de la página aquí, tendrías que añadirla al ContenidoDTO.
            });

            _logger.LogInformation("📦 Se recuperaron {Cantidad} contenidos.", contenidosDto.Count());
            return Ok(contenidosDto);
        }

        // --- GET BY ID ---
        [HttpGet("{id}")]
        // --> Devuelve un solo DTO.
        public async Task<ActionResult<ContenidoDTO>> Get(int id)
        {
            _logger.LogInformation("🔍 [GET] api/contenido/{Id} llamado.", id);
            var contenido = await _service.GetByIdAsync(id);

            if (contenido == null)
            {
                _logger.LogWarning("⚠️ Contenido con ID {Id} no encontrado.", id);
                return NotFound();
            }

            // --> Mapeamos el modelo encontrado a un DTO.
            var contenidoDto = new ContenidoDTO
            {
                IdContenido = contenido.IdContenido,
                Titulo = contenido.Titulo,
                Texto = contenido.Texto,
                FechaPublicacion = contenido.FechaPublicacion
            };

            return Ok(contenidoDto);
        }

        // --- POST ---
        [HttpPost]
        // --> Recibe un DTO de escritura y devuelve uno de lectura.
        public async Task<ActionResult<ContenidoDTO>> Post([FromBody] ContenidoCreateUpdateDTO contenidoDto)
        {
            // --> Corregimos el log para usar el PaginaId del DTO.
            _logger.LogInformation("📥 [POST] api/contenido - Creando nuevo contenido para la página {PaginaId}.", contenidoDto.PaginaId);

            // --> Pasamos el DTO al servicio, que es lo que espera la interfaz.
            var nuevoContenido = await _service.CreateAsync(contenidoDto);

            // --> Mapeamos la nueva entidad creada a un DTO para devolverla al cliente.
            var nuevoContenidoDto = new ContenidoDTO
            {
                IdContenido = nuevoContenido.IdContenido,
                Titulo = nuevoContenido.Titulo,
                Texto = nuevoContenido.Texto,
                FechaPublicacion = nuevoContenido.FechaPublicacion
            };

            _logger.LogInformation("✅ Contenido creado con ID {IdContenido}.", nuevoContenidoDto.IdContenido);
            return CreatedAtAction(nameof(Get), new { id = nuevoContenidoDto.IdContenido }, nuevoContenidoDto);
        }

        // --- PUT ---
        [HttpPut("{id}")]
        // --> Recibe un DTO de escritura.
        public async Task<IActionResult> Put(int id, [FromBody] ContenidoCreateUpdateDTO contenidoDto)
        {
            _logger.LogInformation("✏️ [PUT] api/contenido/{Id} - Actualizando contenido.", id);

            // --> Pasamos el DTO al servicio.
            var actualizado = await _service.UpdateAsync(id, contenidoDto);

            if (!actualizado)
            {
                _logger.LogWarning("⚠️ No se pudo actualizar. Contenido con ID {Id} no encontrado.", id);
                return NotFound();
            }

            _logger.LogInformation("✅ Contenido con ID {Id} actualizado correctamente.", id);
            return NoContent();
        }

        // --- DELETE ---
        // Este método estaba perfecto y no necesita cambios.
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