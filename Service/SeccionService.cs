using MemoriaAPI.Data;
using MemoriaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace MemoriaAPI.Service
{
    public class SeccionService : ISeccionService
    {
        private readonly MemoriaDbContext _context;

        public SeccionService(MemoriaDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Seccion>> GetAllAsync()
        {
            return await _context.Secciones
                .Include(s => s.Pagina)
                .OrderBy(s => s.Orden)
                .ToListAsync();
        }

        public async Task<Seccion?> GetByIdAsync(int id)
        {
            return await _context.Secciones
                .Include(s => s.Pagina)
                .FirstOrDefaultAsync(s => s.IdSeccion == id);
        }

        public async Task<Seccion> CreateAsync(Seccion seccion)
        {
            _context.Secciones.Add(seccion);
            await _context.SaveChangesAsync();
            return seccion;
        }

        public async Task<bool> UpdateAsync(int id, Seccion seccion)
        {
            if (id != seccion.IdSeccion)
                return false;

            _context.Entry(seccion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Secciones.AnyAsync(s => s.IdSeccion == id))
                    return false;

                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var seccion = await _context.Secciones.FindAsync(id);
            if (seccion == null)
                return false;

            _context.Secciones.Remove(seccion);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
