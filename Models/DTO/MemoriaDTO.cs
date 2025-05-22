namespace MemoriaAPI.Models.DTO
{
    public class MemoriaDTO
    {

        public class PaginaDto
        {
            public int IdPagina { get; set; }
            public string? Nombre { get; set; }
            public string? Url { get; set; }
            public int Orden { get; set; }
        }

        public class SeccionDto
        {
            public int IdSeccion { get; set; }
            public string? Nombre { get; set; }
            public string? Url { get; set; }
            public int Orden { get; set; }

            public string? NombrePagina { get; set; }
        }

        public class ContenidoDto
        {
            public int IdContenido { get; set; }
            public string? Titulo { get; set; }
            public string? Texto { get; set; }
            public DateTime FechaPublicacion { get; set; }
        }

        public class UsuarioDto
        {
            public int IdUsuario { get; set; }
            public string? NombreUsuario { get; set; }
            public string? Rol { get; set; }

        }
    }
}
