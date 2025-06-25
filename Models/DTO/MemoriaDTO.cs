

using System.ComponentModel.DataAnnotations;

namespace MemoriaAPI.Models.DTO
{


    public class PaginaDTO
    {
        public int IdPagina { get; set; }
        public string? Nombre { get; set; }
        public string? Url { get; set; }
        public int Orden { get; set; }
    }



    public class PaginaCreateUpdateDTO
    {
        [Required(ErrorMessage = "El nombre de la página es obligatorio.")]
        [MaxLength(255)]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "La URL es obligatoria.")]
        [MaxLength(255)]
        public string Url { get; set; } = string.Empty;

        public int Orden { get; set; }

        [Required(ErrorMessage = "Toda página debe pertenecer a una sección.")]
        public int SeccionId { get; set; } // La clave para saber dónde crear la página
    }




    public class SeccionDTO 
    {
        public int IdSeccion { get; set; }
        public string? Nombre { get; set; }
        public string? Url { get; set; }
        public int Orden { get; set; }
        public string? IconoCss { get; set; }
        public string? NombreEnsamblado { get; set; } 


        public List<PaginaDTO> Paginas { get; set; } = new List<PaginaDTO>();
    }


    public class SeccionCreateUpdateDTO
    {
        [Required(ErrorMessage = "El nombre de la sección es obligatorio.")]
        [MaxLength(255)]
        public string Nombre { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? Url { get; set; }

        public int Orden { get; set; }

        [Required(ErrorMessage = "El año es obligatorio.")]
        public int Anio { get; set; }

        public string? IconoCss { get; set; }

        public string? NombreEnsamblado { get; set; }
    }


    public class ContenidoDTO
    {
        public int IdContenido { get; set; }
        public string? Titulo { get; set; }
        public string? Texto { get; set; }
        public DateTime FechaPublicacion { get; set; }
    }

    public class ContenidoCreateUpdateDTO
    {
        [Required]
        public int PaginaId { get; set; } // A qué página pertenece este contenido

        [MaxLength(255)]
        public string? Titulo { get; set; }

        [Required]
        public string Texto { get; set; } = string.Empty;
    }

    public class UsuarioDTO
    {
        public int IdUsuario { get; set; }
        public string? NombreUsuario { get; set; }
        public string? Rol { get; set; }
    }
}