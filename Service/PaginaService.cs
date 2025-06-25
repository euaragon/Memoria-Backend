using MemoriaAPI.Data;
using MemoriaAPI.Models;
using MemoriaAPI.Models.DTO;
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


        public async Task<Pagina> CreateAsync(PaginaCreateUpdateDTO paginaDto)
        {
            var nuevaPagina = new Pagina
            {
                Nombre = paginaDto.Nombre,
                Url = paginaDto.Url,
                Orden = paginaDto.Orden,
                SeccionId = paginaDto.SeccionId // Asignamos la relación
            };

            _context.Paginas.Add(nuevaPagina);
            await _context.SaveChangesAsync();
            return nuevaPagina;
        }

        public async Task<bool> UpdateAsync(int id, PaginaCreateUpdateDTO paginaDto)
        {
            var paginaExistente = await _context.Paginas.FindAsync(id);

            if (paginaExistente == null)
            {
                return false;
            }

            paginaExistente.Nombre = paginaDto.Nombre;
            paginaExistente.Url = paginaDto.Url;
            paginaExistente.Orden = paginaDto.Orden;
            paginaExistente.SeccionId = paginaDto.SeccionId; // Permite mover la página a otra sección

            await _context.SaveChangesAsync();
            return true;
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
