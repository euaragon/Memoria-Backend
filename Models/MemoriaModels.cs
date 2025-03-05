
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MemoriaAPI.Models
{
    public class Pagina
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdPagina { get; set; }

        [Required]
        [MaxLength(255)]
        public string? Nombre { get; set; }

        [Required]
        [MaxLength(255)]
        public string? Url { get; set; }

        public int Orden { get; set; }
    }

    public class Seccion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdSeccion { get; set; }

        [ForeignKey("Pagina")]
        public int IdPagina { get; set; }

        public Pagina? Pagina { get; set; }

        [Required]
        [MaxLength(255)]
        public string? Nombre { get; set; }

        [Required]
        [MaxLength(255)]
        public string? Url { get; set; }

        public int Orden { get; set; }
    }

    public class Contenido
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdContenido { get; set; }

        [ForeignKey("Seccion")]
        public int IdSeccion { get; set; }

        public Seccion? Seccion { get; set; }

        [MaxLength(255)]
        public string? Titulo { get; set; }

        public string? Texto { get; set; }

        public DateTime FechaPublicacion { get; set; }
    }

    public class Usuario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdUsuario { get; set; }

        [Required]
        [MaxLength(255)]
        public string? NombreUsuario { get; set; }

        [Required]
        public string? Contraseña { get; set; }

        [MaxLength(50)]
        public string? Rol { get; set; }
    }



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
