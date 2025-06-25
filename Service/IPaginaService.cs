using MemoriaAPI.Models;
using MemoriaAPI.Models.DTO;

namespace MemoriaAPI.Service
{
    public interface IPaginaService
    {
        Task<IEnumerable<Pagina>> GetAllAsync();
        Task<Pagina?> GetByIdAsync(int id);
        Task<Pagina> CreateAsync(PaginaCreateUpdateDTO paginaDto);
        Task<bool> UpdateAsync(int id, PaginaCreateUpdateDTO paginaDto);
        Task<bool> DeleteAsync(int id);
    }
}
