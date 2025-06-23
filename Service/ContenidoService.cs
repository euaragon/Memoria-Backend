using MemoriaAPI.Data;
using MemoriaAPI.Models;
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

        public async Task<IEnumerable<Contenido>> GetAllAsync()
        {
            return await _context.Contenidos
                .Include(c => c.Seccion)
                .OrderByDescending(c => c.FechaPublicacion)
                .ToListAsync();
        }

        public async Task<Contenido?> GetByIdAsync(int id)
        {
            return await _context.Contenidos
                .Include(c => c.Seccion)
                .FirstOrDefaultAsync(c => c.IdContenido == id);
        }

        public async Task<Contenido> CreateAsync(Contenido contenido)
        {
            contenido.FechaPublicacion = DateTime.UtcNow;
            _context.Contenidos.Add(contenido);
            await _context.SaveChangesAsync();
            return contenido;
        }

        public async Task<bool> UpdateAsync(int id, Contenido contenido)
        {
            if (id != contenido.IdContenido)
                return false;

            _context.Entry(contenido).State = EntityState.Modified;

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
