using MemoriaAPI.Data;
using MemoriaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace MemoriaAPI.Service
{
    public class PaginaService : IPaginaService
    {
        private readonly MemoriaDbContext _context;

        public PaginaService(MemoriaDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Pagina>> GetAllAsync()
        {
            return await _context.Paginas
                .OrderBy(p => p.Orden)
                .ToListAsync();
        }

        public async Task<Pagina?> GetByIdAsync(int id)
        {
            return await _context.Paginas.FindAsync(id);
        }

        public async Task<Pagina> CreateAsync(Pagina pagina)
        {
            _context.Paginas.Add(pagina);
            await _context.SaveChangesAsync();
            return pagina;
        }

        public async Task<bool> UpdateAsync(int id, Pagina pagina)
        {
            if (id != pagina.IdPagina)
                return false;

            _context.Entry(pagina).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Paginas.AnyAsync(p => p.IdPagina == id))
                    return false;

                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var pagina = await _context.Paginas.FindAsync(id);
            if (pagina == null)
                return false;

            _context.Paginas.Remove(pagina);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
