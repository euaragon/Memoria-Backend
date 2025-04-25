using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using MemoriaAPI.Models;

namespace MemoriaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FallosController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public FallosController(IConfiguration configuration)
        {
            _configuration = configuration;
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
WHERE Digesto.DigId = @DigId AND Digesto.TipNorId = 9
";

            var resultado = await connection.QueryFirstOrDefaultAsync(query, new { DigId = digId });

            if (resultado == null)
                return NotFound();

            // Si hay archivo firmado, usar ese. Si no, usar el base.
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