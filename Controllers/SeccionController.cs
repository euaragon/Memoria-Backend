using MemoriaAPI.Models.DTO; // --> ¡Importante! Añadir el using para los DTOs
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

        // --- GET ALL ---
        [HttpGet]
        // --> El método ahora devuelve una lista de DTOs, no el modelo de BD.
        public async Task<ActionResult<IEnumerable<SeccionDTO>>> Get()
        {
            _logger.LogInformation("🔍 [GET] api/seccion llamado.");
            var secciones = await _service.GetAllAsync();

            // --> Mapeamos la lista de modelos a una lista de DTOs antes de enviarla.
            var seccionesDto = secciones.Select(s => new SeccionDTO
            {
                IdSeccion = s.IdSeccion,
                Nombre = s.Nombre,
                Url = s.Url,
                Orden = s.Orden,
                IconoCss = s.IconoCss,
                NombreEnsamblado = s.NombreEnsamblado,
                Paginas = s.Paginas.Select(p => new PaginaDTO
                {
                    IdPagina = p.IdPagina,
                    Nombre = p.Nombre,
                    Url = p.Url,
                    Orden = p.Orden
                }).ToList()
            });

            _logger.LogInformation("📦 {Cantidad} secciones recuperadas.", seccionesDto.Count());
            return Ok(seccionesDto);
        }

        // --- GET BY ID ---
        [HttpGet("{id}")]
        // --> El método ahora devuelve un solo DTO.
        public async Task<ActionResult<SeccionDTO>> Get(int id)
        {
            _logger.LogInformation("🔍 [GET] api/seccion/{Id} llamado.", id);
            var seccion = await _service.GetByIdAsync(id);

            if (seccion == null)
            {
                _logger.LogWarning("⚠️ Sección con ID {Id} no encontrada.", id);
                return NotFound();
            }

            // --> Mapeamos la entidad encontrada a un DTO.
            var seccionDto = new SeccionDTO
            {
                IdSeccion = seccion.IdSeccion,
                Nombre = seccion.Nombre,
                Url = seccion.Url,
                Orden = seccion.Orden,
                IconoCss = seccion.IconoCss,
                NombreEnsamblado = seccion.NombreEnsamblado,
                Paginas = seccion.Paginas.Select(p => new PaginaDTO
                {
                    IdPagina = p.IdPagina,
                    Nombre = p.Nombre,
                    Url = p.Url,
                    Orden = p.Orden
                }).ToList()
            };

            return Ok(seccionDto);
        }

        // --- POST ---
        [HttpPost]
        // --> El método ahora devuelve el DTO de lectura y recibe el DTO de escritura.
        public async Task<ActionResult<SeccionDTO>> Post([FromBody] SeccionCreateUpdateDTO seccionDto)
        {
            _logger.LogInformation("📥 [POST] api/seccion - Creando nueva sección: {Nombre}", seccionDto.Nombre);

            // --> Pasamos el DTO al servicio, cumpliendo con el contrato de la interfaz.
            var nuevaSeccion = await _service.CreateAsync(seccionDto);

            // --> Mapeamos la respuesta del servicio (que es un modelo) a un DTO para el cliente.
            var nuevaSeccionDto = new SeccionDTO
            {
                IdSeccion = nuevaSeccion.IdSeccion,
                Nombre = nuevaSeccion.Nombre,
                Url = nuevaSeccion.Url,
                Orden = nuevaSeccion.Orden,
                IconoCss = nuevaSeccion.IconoCss,
                NombreEnsamblado = nuevaSeccion.NombreEnsamblado
            };

            _logger.LogInformation("✅ Sección creada con ID {IdSeccion}.", nuevaSeccionDto.IdSeccion);
            return CreatedAtAction(nameof(Get), new { id = nuevaSeccionDto.IdSeccion }, nuevaSeccionDto);
        }

        // --- PUT ---
        [HttpPut("{id}")]
        // --> El método ahora recibe el DTO de escritura.
        public async Task<IActionResult> Put(int id, [FromBody] SeccionCreateUpdateDTO seccionDto)
        {
            _logger.LogInformation("✏️ [PUT] api/seccion/{Id} - Actualizando sección.", id);

            // --> Pasamos el DTO al servicio.
            var actualizado = await _service.UpdateAsync(id, seccionDto);

            if (!actualizado)
            {
                _logger.LogWarning("⚠️ No se pudo actualizar. Sección con ID {Id} no encontrada.", id);
                return NotFound();
            }

            _logger.LogInformation("✅ Sección con ID {Id} actualizada correctamente.", id);
            return NoContent();
        }

        // --- DELETE ---
        // Este método estaba perfecto y no necesita cambios.
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