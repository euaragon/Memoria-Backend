using MemoriaAPI.Models;
using MemoriaAPI.Models.DTO;

namespace MemoriaAPI.Services
{
    public interface IFallosService
    {
        Task<IEnumerable<dynamic>> GetFallosTodos2024();
        Task<IEnumerable<dynamic>> GetFallosCabecera2024();
        Task<IEnumerable<dynamic>> GetCantidadFallosCabeceraPorSector2024();
        Task<IEnumerable<dynamic>> GetFallosPiezaSeparada2024();
        Task<IEnumerable<dynamic>> GetCantidadFallosPiezaSeparadaPorSector2024();
        Task<IEnumerable<dynamic>> GetFallosGastosReservados2024();
        Task<IEnumerable<dynamic>> GetCantidadFallosGastosReservadosPorSector2024();
        Task<List<CantidadFallosPorSectorUnificadoDTO>> GetCantidadFallosPorSectorUnificado2024();
        Task<IEnumerable<ResultadoDigesto>> Buscar(FiltroFallos filtros);
        Task<FalloPdfResultado> DescargarPdf(int digId);
    }
}
