using MemoriaAPI.Models;

namespace MemoriaAPI.Service
{
    public interface IContenidoService
    {
        Task<IEnumerable<Contenido>> GetAllAsync();
        Task<Contenido?> GetByIdAsync(int id);
        Task<Contenido> CreateAsync(Contenido contenido);
        Task<bool> UpdateAsync(int id, Contenido contenido);
        Task<bool> DeleteAsync(int id);
    }
}
