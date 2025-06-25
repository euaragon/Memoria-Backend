using MemoriaAPI.Models;
using MemoriaAPI.Models.DTO; // Asegúrate de que este using esté presente

namespace MemoriaAPI.Service
{
    public interface ISeccionService
    {
        Task<IEnumerable<Seccion>> GetAllAsync();
        Task<Seccion?> GetByIdAsync(int id);

        // FIRMA CORREGIDA: Ahora usa el DTO, igual que en tu clase.
        Task<Seccion> CreateAsync(SeccionCreateUpdateDTO seccionDto);

        // FIRMA CORREGIDA: Ahora usa el DTO, igual que en tu clase.
        Task<bool> UpdateAsync(int id, SeccionCreateUpdateDTO seccionDto);

        Task<bool> DeleteAsync(int id);
    }
}