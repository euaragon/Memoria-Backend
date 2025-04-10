
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MemoriaAPI.Models
{


    public class FiltroFallos
    {
        public string? DigNumeroDesde { get; set; }
        public int? EntIdApl { get; set; }
        public string? Keyword { get; set; }
        public int? Anio { get; set; } = 2024;
        public int? DigAgrId { get; set; }
    }



    public class ResultadoDigesto
    {
        public int DigId { get; set; }
        public string TipNorNombre { get; set; }
        public string DigNumero { get; set; }
        public string EntRazonSocial { get; set; }
        public int TipNorId { get; set; }
        public int EntId { get; set; }
        public DateTime? DigFEmision { get; set; }
        public DateTime? DigFPublicacion { get; set; }
        public DateTime? DigFVigencia { get; set; }
        public string DigExtracto { get; set; }
        public string AplDigNombre { get; set; }
        public bool DigModificada { get; set; }
        public bool DigDerogada { get; set; }
        public bool DigSeleccion { get; set; }
        public bool DigInternet { get; set; }
        public string ArcDigNombre { get; set; }
    }




























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
