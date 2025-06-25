using MemoriaAPI.Data;
using MemoriaAPI.Models;
using MemoriaAPI.Models.DTO;
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
                .Include(s => s.Paginas) // Incluye las páginas hijas
                .OrderBy(s => s.Orden)
                .ToListAsync();
        }

        public async Task<Seccion?> GetByIdAsync(int id)
        {
            return await _context.Secciones
                .Include(s => s.Paginas) // Incluye las páginas hijas
                .FirstOrDefaultAsync(s => s.IdSeccion == id);
        }

        public async Task<Seccion> CreateAsync(SeccionCreateUpdateDTO seccionDto)
        {
            // Mapeamos del DTO al modelo de base de datos
            var nuevaSeccion = new Seccion
            {
                Nombre = seccionDto.Nombre,
                Url = seccionDto.Url,
                Orden = seccionDto.Orden,
                Anio = seccionDto.Anio,
                IconoCss = seccionDto.IconoCss,
                NombreEnsamblado = seccionDto.NombreEnsamblado
            };

            _context.Secciones.Add(nuevaSeccion);
            await _context.SaveChangesAsync();
            return nuevaSeccion; 
        }


        public async Task<bool> UpdateAsync(int id, SeccionCreateUpdateDTO seccionDto)
        {

            var seccionExistente = await _context.Secciones.FindAsync(id);

            if (seccionExistente == null)
            {
                return false; 
            }


            seccionExistente.Nombre = seccionDto.Nombre;
            seccionExistente.Url = seccionDto.Url;
            seccionExistente.Orden = seccionDto.Orden;
            seccionExistente.Anio = seccionDto.Anio;
            seccionExistente.IconoCss = seccionDto.IconoCss;
            seccionExistente.NombreEnsamblado = seccionDto.NombreEnsamblado;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                // Manejo de concurrencia por si otro usuario la modificó al mismo tiempo
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