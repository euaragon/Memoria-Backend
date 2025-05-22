using Dapper;
using Microsoft.Data.SqlClient;

using MemoriaAPI.Models;
using MemoriaAPI.Models.DTO;

namespace MemoriaAPI.Services
{
    public class FallosService : IFallosService
    {
        private readonly IConfiguration _configuration;

        public FallosService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private async Task<IEnumerable<dynamic>> EjecutarQuery(string sql)
        {
            var connectionString = _configuration.GetConnectionString("FallosDb");
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync(sql);
        }

        public async Task<IEnumerable<dynamic>> GetFallosTodos2024()
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
                ORDER BY f.FalNumero ASC;
            ";
            return await EjecutarQuery(query);
        }

        public async Task<IEnumerable<dynamic>> GetFallosCabecera2024()
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
            return await EjecutarQuery(query);
        }

        public async Task<IEnumerable<dynamic>> GetCantidadFallosCabeceraPorSector2024()
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
            return await EjecutarQuery(query);
        }

        public async Task<IEnumerable<dynamic>> GetFallosPiezaSeparada2024()
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
            return await EjecutarQuery(query);
        }

        public async Task<IEnumerable<dynamic>> GetCantidadFallosPiezaSeparadaPorSector2024()
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
            return await EjecutarQuery(query);
        }

        public async Task<IEnumerable<dynamic>> GetFallosGastosReservados2024()
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
            return await EjecutarQuery(query);
        }

        public async Task<IEnumerable<dynamic>> GetCantidadFallosGastosReservadosPorSector2024()
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
            return await EjecutarQuery(query);
        }

        public async Task<IEnumerable<ResultadoDigesto>> Buscar(FiltroFallos filtros)
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
            return await connection.QueryAsync<ResultadoDigesto>(query, filtros);
        }

        public async Task<FalloPdfResultado> DescargarPdf(int digId)
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
            var resultado = await connection.QueryFirstOrDefaultAsync<FalloPdfResultado>(query, new { DigId = digId });
            return resultado;
        }




        public async Task<List<CantidadFallosPorSectorUnificadoDTO>> GetCantidadFallosPorSectorUnificado2024()
        {
            var cabecera = await GetCantidadFallosCabeceraPorSector2024();
            var piezaSeparada = await GetCantidadFallosPiezaSeparadaPorSector2024();
            var gastosReservados = await GetCantidadFallosGastosReservadosPorSector2024();

            // Crear un diccionario para agrupar las cantidades por sector
            var cantidadesPorSector = new Dictionary<string, CantidadFallosPorSectorUnificadoDTO>();

            // Función auxiliar para actualizar el diccionario
            void ActualizarCantidades(IEnumerable<dynamic> datos, Func<dynamic, int> obtenerCantidad, string tipo)
            {
                if (datos != null)
                {
                    foreach (var item in datos)
                    {
                        if (item != null)
                        {
                            var sector = item.SecReducido?.ToString(); // Usa SecReducido (con protección null)
                            if (!string.IsNullOrEmpty(sector))
                            {
                                // La función obtenerCantidad ya no es necesaria, accedemos directamente a la propiedad
                                var cantidad = (int)item.CantidadRegistros; // Accede a CantidadRegistros y cástalo a int

                                if (!cantidadesPorSector.ContainsKey(sector))
                                {
                                    cantidadesPorSector[sector] = new CantidadFallosPorSectorUnificadoDTO
                                    {
                                        Sector = sector,
                                        CantidadCabecera = 0,
                                        CantidadPiezaSeparada = 0,
                                        CantidadGastosReservados = 0,
                                        CantidadTotal = 0
                                    };
                                }

                                switch (tipo)
                                {
                                    case "cabecera":
                                        cantidadesPorSector[sector].CantidadCabecera += cantidad;
                                        break;
                                    case "piezaSeparada":
                                        cantidadesPorSector[sector].CantidadPiezaSeparada += cantidad;
                                        break;
                                    case "gastosReservados":
                                        cantidadesPorSector[sector].CantidadGastosReservados += cantidad;
                                        break;
                                }
                                cantidadesPorSector[sector].CantidadTotal += cantidad;
                            }
                        }
                    }
                }
            }

            ActualizarCantidades(cabecera, item => item.Cantidad, "cabecera");
            ActualizarCantidades(piezaSeparada, item => item.Cantidad, "piezaSeparada");
            ActualizarCantidades(gastosReservados, item => item.Cantidad, "gastosReservados");

            return cantidadesPorSector.Values.ToList();
        }


    }
}
