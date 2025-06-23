using MemoriaAPI.Models;

namespace MemoriaAPI.Service
{
    public interface IPaginaService
    {
        Task<IEnumerable<Pagina>> GetAllAsync();
        Task<Pagina?> GetByIdAsync(int id);
        Task<Pagina> CreateAsync(Pagina pagina);
        Task<bool> UpdateAsync(int id, Pagina pagina);
        Task<bool> DeleteAsync(int id);
    }
}
