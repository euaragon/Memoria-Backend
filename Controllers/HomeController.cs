using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using MemoriaAPI.Models;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace MemoriaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConfiguracionController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ConfiguracionController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult GetConfiguracion()
        {
            var baseUrl = _configuration["ApiConfiguration:BaseUrl"];
            return Ok(new { BaseUrl = baseUrl });
        }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class FallosController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public FallosController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private async Task<IEnumerable<dynamic>> EjecutarQuery(string sql)
        {
            var connectionString = _configuration.GetConnectionString("FallosDb");
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync(sql);
        }

        [HttpGet("fallos2024/todos")]
        public async Task<IActionResult> GetFallosTodos2024()
        {
            var query = @"
                SELECT
                    f.FalNumero,
                    f.FalFechaEmision,
                    e.ExpNumero,
                    e.ExpEjercicio,
                    e.ExpNomenclador,
                    te.TipExpNombre,
                    en.EntRazonSocial,
                    s.SecNombre,
                    s.SecReducido
                FROM Fallo_Expediente fe
                    INNER JOIN Fallo f ON f.FalId = fe.FalId
                    INNER JOIN Expediente e ON e.ExpId = fe.ExpId
                    INNER JOIN Tipo_Expediente te ON te.TipExpId = e.TipExpId
                    INNER JOIN Organismo o ON o.OrgId = e.OrgId
                    INNER JOIN Entidad en ON en.EntId = o.EntId
                    INNER JOIN Sector s ON s.SecId = e.SecId
                WHERE
                    YEAR(f.FalFechaEmision) = 2024
                ORDER BY f.FalNumero ASC; -- Ordenamos por número de fallo
            ";
            var resultados = await EjecutarQuery(query);
            return Ok(resultados);
        }

        [HttpGet("fallos2024/cabecera")]
        public async Task<IActionResult> GetFallosCabecera2024()
        {
            var query = @"
                SELECT
                    f.FalNumero,
                    f.FalFechaEmision,
                    e.ExpNumero,
                    e.ExpEjercicio,
                    e.ExpNomenclador,
                    te.TipExpNombre,
                    en.EntRazonSocial,
                    s.SecNombre,
                    s.SecReducido
                FROM Fallo_Expediente fe
                    INNER JOIN Fallo f ON f.FalId = fe.FalId
                    INNER JOIN Expediente e ON e.ExpId = fe.ExpId
                    INNER JOIN Tipo_Expediente te ON te.TipExpId = e.TipExpId
                    INNER JOIN Organismo o ON o.OrgId = e.OrgId
                    INNER JOIN Entidad en ON en.EntId = o.EntId
                    INNER JOIN Sector s ON s.SecId = e.SecId
                WHERE
                    YEAR(f.FalFechaEmision) = 2024
                    AND te.TipExpNombre = 'CABECERA'
                ORDER BY f.FalNumero ASC;
            ";
            var resultados = await EjecutarQuery(query);
            return Ok(resultados);
        }

        [HttpGet("fallos2024/cabecera/cantidad-por-sector")]
        public async Task<IActionResult> GetCantidadFallosCabeceraPorSector2024()
        {
            var query = @"
                SELECT
                    COALESCE(s.SecReducido, 'TOTAL') AS SecReducido,
                    COUNT(*) AS CantidadRegistros
                FROM Fallo_Expediente fe
                    INNER JOIN Fallo f ON f.FalId = fe.FalId
                    INNER JOIN Expediente e ON e.ExpId = fe.ExpId
                    INNER JOIN Tipo_Expediente te ON te.TipExpId = e.TipExpId
                    INNER JOIN Organismo o ON o.OrgId = e.OrgId
                    INNER JOIN Entidad en ON en.EntId = o.EntId
                    INNER JOIN Sector s ON s.SecId = e.SecId
                WHERE
                    YEAR(f.FalFechaEmision) = 2024
                    AND te.TipExpNombre = 'CABECERA'
                GROUP BY ROLLUP(s.SecReducido)
                ORDER BY CantidadRegistros DESC;
            ";
            var resultados = await EjecutarQuery(query);
            return Ok(resultados);
        }

        [HttpGet("fallos2024/pieza-separada")]
        public async Task<IActionResult> GetFallosPiezaSeparada2024()
        {
            var query = @"
                SELECT
                    f.FalNumero,
                    f.FalFechaEmision,
                    e.ExpNumero,
                    e.ExpEjercicio,
                    e.ExpNomenclador,
                    te.TipExpNombre,
                    en.EntRazonSocial,
                    s.SecNombre,
                    s.SecReducido
                FROM Fallo_Expediente fe
                    INNER JOIN Fallo f ON f.FalId = fe.FalId
                    INNER JOIN Expediente e ON e.ExpId = fe.ExpId
                    INNER JOIN Tipo_Expediente te ON te.TipExpId = e.TipExpId
                    INNER JOIN Organismo o ON o.OrgId = e.OrgId
                    INNER JOIN Entidad en ON en.EntId = o.EntId
                    INNER JOIN Sector s ON s.SecId = e.SecId
                WHERE
                    YEAR(f.FalFechaEmision) = 2024
                    AND te.TipExpNombre = 'PIEZA SEPARADA'
                ORDER BY f.FalNumero ASC;
            ";
            var resultados = await EjecutarQuery(query);
            return Ok(resultados);
        }

        [HttpGet("fallos2024/pieza-separada/cantidad-por-sector")]
        public async Task<IActionResult> GetCantidadFallosPiezaSeparadaPorSector2024()
        {
            var query = @"
                SELECT
                    COALESCE(s.SecReducido, 'TOTAL') AS SecReducido,
                    COUNT(*) AS CantidadRegistros
                FROM Fallo_Expediente fe
                    INNER JOIN Fallo f ON f.FalId = fe.FalId
                    INNER JOIN Expediente e ON e.ExpId = fe.ExpId
                    INNER JOIN Tipo_Expediente te ON te.TipExpId = e.TipExpId
                    INNER JOIN Organismo o ON o.OrgId = e.OrgId
                    INNER JOIN Entidad en ON en.EntId = o.EntId
                    INNER JOIN Sector s ON s.SecId = e.SecId
                WHERE
                    YEAR(f.FalFechaEmision) = 2024
                    AND te.TipExpNombre = 'PIEZA SEPARADA'
                GROUP BY ROLLUP(s.SecReducido)
                ORDER BY CantidadRegistros DESC;
            ";
            var resultados = await EjecutarQuery(query);
            return Ok(resultados);
        }

        [HttpGet("fallos2024/gastos-reservados")]
        public async Task<IActionResult> GetFallosGastosReservados2024()
        {
            var query = @"
                SELECT
                    f.FalNumero,
                    f.FalFechaEmision,
                    e.ExpNumero,
                    e.ExpEjercicio,
                    e.ExpNomenclador,
                    te.TipExpNombre,
                    en.EntRazonSocial,
                    s.SecNombre,
                    s.SecReducido
                FROM Fallo_Expediente fe
                    INNER JOIN Fallo f ON f.FalId = fe.FalId
                    INNER JOIN Expediente e ON e.ExpId = fe.ExpId
                    INNER JOIN Tipo_Expediente te ON te.TipExpId = e.TipExpId
                    INNER JOIN Organismo o ON o.OrgId = e.OrgId
                    INNER JOIN Entidad en ON en.EntId = o.EntId
                    INNER JOIN Sector s ON s.SecId = e.SecId
                WHERE
                    YEAR(f.FalFechaEmision) = 2024
                    AND te.TipExpNombre = 'GASTOS RESERVADOS'
                ORDER BY f.FalNumero ASC;
            ";
            var resultados = await EjecutarQuery(query);
            return Ok(resultados);
        }

        [HttpGet("fallos2024/gastos-reservados/cantidad-por-sector")]
        public async Task<IActionResult> GetCantidadFallosGastosReservadosPorSector2024()
        {
            var query = @"
                SELECT
                    COALESCE(s.SecReducido, 'TOTAL') AS SecReducido,
                    COUNT(*) AS CantidadRegistros
                FROM Fallo_Expediente fe
                    INNER JOIN Fallo f ON f.FalId = fe.FalId
                    INNER JOIN Expediente e ON e.ExpId = fe.ExpId
                    INNER JOIN Tipo_Expediente te ON te.TipExpId = e.TipExpId
                    INNER JOIN Organismo o ON o.OrgId = e.OrgId
                    INNER JOIN Entidad en ON en.EntId = o.EntId
                    INNER JOIN Sector s ON s.SecId = e.SecId
                WHERE
                    YEAR(f.FalFechaEmision) = 2024
                    AND te.TipExpNombre = 'GASTOS RESERVADOS'
                GROUP BY ROLLUP(s.SecReducido)
                ORDER BY CantidadRegistros DESC;
            ";
            var resultados = await EjecutarQuery(query);
            return Ok(resultados);
        }

        [HttpGet("buscar")]
        public async Task<IActionResult> Buscar([FromQuery] FiltroFallos filtros)
        {
            var connectionString = _configuration.GetConnectionString("FallosDb");

            using var connection = new SqlConnection(connectionString);

            var query = @"
    SELECT DISTINCT 
        Digesto.DigId, 
        Tipo_Norma.TipNorNombre, 
        Digesto.DigNumero, 
        Entidad.EntRazonSocial, 
        Digesto.TipNorId, 
        Digesto.EntId, 
        Digesto.DigFEmision, 
        Digesto.DigFPublicacion, 
        Digesto.DigFVigencia, 
        Digesto.DigExtracto, 
        Aplicacion_Digesto.AplDigNombre, 
        Digesto.DigModificada, 
        Digesto.DigDerogada, 
        Digesto.DigSeleccion, 
        Digesto.DigInternet, 
        Archivo_Digesto.ArcDigNombre 
    FROM Digesto 
        INNER JOIN Tipo_Norma ON Tipo_Norma.TipNorId = Digesto.TipNorId 
        INNER JOIN Entidad ON Entidad.EntId = Digesto.EntId 
        INNER JOIN Aplicacion_Digesto ON Aplicacion_Digesto.AplDigId = Digesto.AplDigId 
        INNER JOIN Digesto_Organismo_Aplicacion ON Digesto_Organismo_Aplicacion.DigId = Digesto.DigId 
        LEFT JOIN Digesto_Agrupamiento_Entidad ON Digesto_Agrupamiento_Entidad.DigAgrId = Digesto_Organismo_Aplicacion.DigAgrId 
        LEFT JOIN Archivo_Digesto ON Archivo_Digesto.ArcDigId = Digesto.ArcDigId 
    WHERE 
        Digesto.MetCarId <> 3 
        AND Digesto.TipNorId = 9 
        AND Digesto.DigInternet = 1
        AND (@Anio IS NULL OR YEAR(Digesto.DigFEmision) = @Anio)
        AND (@DigNumeroDesde IS NULL OR Digesto.DigNumero LIKE '%' + @DigNumeroDesde + '%')
        AND (
            @EntIdApl IS NULL 
            OR Digesto_Organismo_Aplicacion.EntId = @EntIdApl
            OR (Digesto_Agrupamiento_Entidad.DigAgrId <> 2 AND Digesto_Agrupamiento_Entidad.EntId = @EntIdApl)
            OR Digesto_Agrupamiento_Entidad.DigAgrId = 2
        )
        AND (@DigAgrId IS NULL OR Digesto_Organismo_Aplicacion.DigAgrId = @DigAgrId)
        AND (
            @Keyword IS NULL 
            OR LOWER(Digesto.DigExtracto) LIKE '%' + LOWER(@Keyword) + '%'
        )
    ORDER BY 
        Tipo_Norma.TipNorNombre, 
        Entidad.EntRazonSocial, 
        Digesto.DigFEmision DESC, 
        Digesto.DigNumero DESC;
    ";

            var resultados = await connection.QueryAsync<ResultadoDigesto>(query, filtros);
            return Ok(resultados);
        }


        [HttpGet("pdf/{digId}")]
        public async Task<IActionResult> DescargarPdf(int digId)
        {
            var connectionString = _configuration.GetConnectionString("FallosDb");

            using var connection = new SqlConnection(connectionString);

            var query = @"
                SELECT
                    Fallo.FalNumero,
                    Fallo.ArcBasId,
                    Fallo.ArcFirId,
                    Archivo_Firmado.ArcFirNombre,
                    Archivo_Firmado.ArcFirContentType,
                    Archivo_Firmado.ArcFirArchivo,
                    Archivo_Base.ArcBasNombre,
                    Archivo_Base.ArcBasContentType,
                    Archivo_Base.ArcBasArchivo
                FROM Digesto
                    INNER JOIN Fallo ON Fallo.FalNumero = CONVERT(int, Digesto.DigNumero)
                    INNER JOIN Archivo_Base ON Archivo_Base.ArcBasId = Fallo.ArcBasId
                    LEFT JOIN Archivo_Firmado ON Archivo_Firmado.ArcFirId = Fallo.ArcFirId
                WHERE Digesto.DigId = @DigId AND Digesto.TipNorId = 9;
            ";

            var resultado = await connection.QueryFirstOrDefaultAsync(query, new { DigId = digId });

            if (resultado == null)
                return NotFound();

            byte[] archivo = resultado.ArcFirArchivo ?? resultado.ArcBasArchivo;
            string nombre = resultado.ArcFirNombre ?? resultado.ArcBasNombre;
            string contentType = resultado.ArcFirContentType ?? resultado.ArcBasContentType;

            return File(archivo, contentType ?? "application/pdf", nombre ?? "fallo.pdf");
        }
    }

    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}