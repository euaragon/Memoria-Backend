using MemoriaAPI.Models;
using MemoriaAPI.Models.DTO;

namespace MemoriaAPI.Service
{
    public interface IContenidoService
    {
        Task<IEnumerable<Contenido>> GetAllAsync();
        Task<Contenido?> GetByIdAsync(int id);
        Task<Contenido> CreateAsync(ContenidoCreateUpdateDTO contenidoDto); // <-- Usa el DTO
        Task<bool> UpdateAsync(int id, ContenidoCreateUpdateDTO contenidoDto); // <-- Usa el DTO
        Task<bool> DeleteAsync(int id);
    }
}
