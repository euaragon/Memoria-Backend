using Microsoft.AspNetCore.Mvc;
using MemoriaAPI.Models;
using System.Threading.Tasks;
using MemoriaAPI.Services;
using MemoriaAPI.Models.DTO; // Importa el namespace del DTO

namespace MemoriaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FallosController : ControllerBase
    {
        private readonly IFallosService _fallosService;

        public FallosController(IFallosService fallosService)
        {
            _fallosService = fallosService;
        }

        [HttpGet("fallos2024/todos")]
        public async Task<IActionResult> GetFallosTodos2024()
        {
            var resultados = await _fallosService.GetFallosTodos2024();
            return Ok(resultados);
        }

        [HttpGet("fallos2024/cabecera")]
        public async Task<IActionResult> GetFallosCabecera2024()
        {
            var resultados = await _fallosService.GetFallosCabecera2024();
            return Ok(resultados);
        }

        [HttpGet("fallos2024/cabecera/cantidad-por-sector")]
        public async Task<IActionResult> GetCantidadFallosCabeceraPorSector2024()
        {
            var resultados = await _fallosService.GetCantidadFallosCabeceraPorSector2024();
            return Ok(resultados);
        }

        [HttpGet("fallos2024/pieza-separada")]
        public async Task<IActionResult> GetFallosPiezaSeparada2024()
        {
            var resultados = await _fallosService.GetFallosPiezaSeparada2024();
            return Ok(resultados);
        }

        [HttpGet("fallos2024/pieza-separada/cantidad-por-sector")]
        public async Task<IActionResult> GetCantidadFallosPiezaSeparadaPorSector2024()
        {
            var resultados = await _fallosService.GetCantidadFallosPiezaSeparadaPorSector2024();
            return Ok(resultados);
        }

        [HttpGet("fallos2024/gastos-reservados")]
        public async Task<IActionResult> GetFallosGastosReservados2024()
        {
            var resultados = await _fallosService.GetFallosGastosReservados2024();
            return Ok(resultados);
        }

        [HttpGet("fallos2024/gastos-reservados/cantidad-por-sector")]
        public async Task<IActionResult> GetCantidadFallosGastosReservadosPorSector2024()
        {
            var resultados = await _fallosService.GetCantidadFallosGastosReservadosPorSector2024();
            return Ok(resultados);
        }




        [HttpGet("fallos2024/cantidad-por-sector")]
        public async Task<IActionResult> GetCantidadFallosPorSectorUnificado2024()
        {
            var resultados = await _fallosService.GetCantidadFallosPorSectorUnificado2024();
            return Ok(resultados);
        }







        [HttpGet("buscar")]
        public async Task<IActionResult> Buscar([FromQuery] FiltroFallos filtros)
        {
            var resultados = await _fallosService.Buscar(filtros);
            return Ok(resultados);
        }

        [HttpGet("pdf/{digId}")]
        public async Task<IActionResult> DescargarPdf(int digId)
        {
            var resultado = await _fallosService.DescargarPdf(digId);

            if (resultado == null)
            {
                return NotFound();
            }

            // Crear una instancia del DTO y asignar los valores.
            var falloPdf = new FalloPdfDTO
            {
                Archivo = resultado.ArcFirArchivo ?? resultado.ArcBasArchivo,
                NombreArchivo = resultado.ArcFirNombre ?? resultado.ArcBasNombre,
                ContentType = resultado.ArcFirContentType ?? resultado.ArcBasContentType
            };

            return File(falloPdf.Archivo, falloPdf.ContentType ?? "application/pdf", falloPdf.NombreArchivo ?? "fallo.pdf");
        }
    }
}
