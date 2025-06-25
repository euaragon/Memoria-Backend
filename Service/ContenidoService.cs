using MemoriaAPI.Data;
using MemoriaAPI.Models;
using MemoriaAPI.Models.DTO; // Importa tus DTOs
using Microsoft.EntityFrameworkCore;

namespace MemoriaAPI.Service
{
    public class ContenidoService : IContenidoService 
    {
        private readonly MemoriaDbContext _context;

        public ContenidoService(MemoriaDbContext context)
        {
            _context = context;
        }

        // GET: Corregido para incluir la jerarquía correcta
        public async Task<IEnumerable<Contenido>> GetAllAsync()
        {
            return await _context.Contenidos
                // CORRECCIÓN: Incluimos la Página a la que pertenece el contenido.
                .Include(c => c.Pagina)
                    // EXTRA: También incluimos la Sección de esa Página para tener el contexto completo.
                    .ThenInclude(p => p.Seccion)
                .OrderByDescending(c => c.FechaPublicacion)
                .ToListAsync();
        }

        // GET BY ID: Corregido para incluir la jerarquía correcta
        public async Task<Contenido?> GetByIdAsync(int id)
        {
            return await _context.Contenidos
                // CORRECCIÓN: Incluimos la Página...
                .Include(c => c.Pagina)
                    // ... y la Sección de esa Página.
                    .ThenInclude(p => p.Seccion)
                .FirstOrDefaultAsync(c => c.IdContenido == id);
        }

        // CREATE: Ahora usa un DTO para mayor seguridad
        public async Task<Contenido> CreateAsync(ContenidoCreateUpdateDTO contenidoDto)
        {
            // Mapeamos del DTO al modelo de base de datos
            var nuevoContenido = new Contenido
            {
                PaginaId = contenidoDto.PaginaId, // Se asigna la relación
                Titulo = contenidoDto.Titulo,
                Texto = contenidoDto.Texto,
                FechaPublicacion = DateTime.UtcNow // Buena práctica usar UTC en el servidor
            };

            _context.Contenidos.Add(nuevoContenido);
            await _context.SaveChangesAsync();
            return nuevoContenido;
        }

        // UPDATE: Implementación robusta usando el patrón "Cargar y luego Actualizar"
        public async Task<bool> UpdateAsync(int id, ContenidoCreateUpdateDTO contenidoDto)
        {
            // 1. Cargar la entidad de contenido existente desde la BD
            var contenidoExistente = await _context.Contenidos.FindAsync(id);

            if (contenidoExistente == null)
            {
                return false; // No se encontró para actualizar
            }

            // 2. Actualizar las propiedades desde el DTO
            contenidoExistente.PaginaId = contenidoDto.PaginaId; // Permite mover contenido de página
            contenidoExistente.Titulo = contenidoDto.Titulo;
            contenidoExistente.Texto = contenidoDto.Texto;
            // Opcional: podrías querer actualizar una fecha de "Última Modificación" aquí

            // 3. Guardar los cambios
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Contenidos.AnyAsync(c => c.IdContenido == id))
                    return false;

                throw;
            }
        }

        // DELETE: Tu implementación ya era correcta.
        public async Task<bool> DeleteAsync(int id)
        {
            var contenido = await _context.Contenidos.FindAsync(id);
            if (contenido == null)
                return false;

            _context.Contenidos.Remove(contenido);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}