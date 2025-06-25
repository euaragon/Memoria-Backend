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




    public class Seccion
    {
        [Key]
        public int IdSeccion { get; set; }

        [Required]
        [MaxLength(255)]
        public string Nombre { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? Url { get; set; } // Se usa si la sección es un enlace directo

        public int Orden { get; set; }
        public int Anio { get; set; }

        // Propiedades útiles para el frontend dinámico
        public string? IconoCss { get; set; }
        public string? NombreEnsamblado { get; set; } // Clave para los Micro-Frontends

        // RELACIÓN CORREGIDA: Una Sección tiene una colección de Páginas.
        public virtual ICollection<Pagina> Paginas { get; set; } = new List<Pagina>();
    }


    public class Pagina
    {
        [Key]
        public int IdPagina { get; set; }

        [Required]
        [MaxLength(255)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string Url { get; set; } = string.Empty;

        public int Orden { get; set; }

        // RELACIÓN CORREGIDA: Se añade la llave foránea a la Sección a la que pertenece.
        [ForeignKey("Seccion")]
        public int SeccionId { get; set; }
        public virtual Seccion Seccion { get; set; } = null!;

        // Una página puede tener una colección de bloques de contenido.
        public virtual ICollection<Contenido> Contenidos { get; set; } = new List<Contenido>();
    }


    public class Contenido
    {
        [Key]
        public int IdContenido { get; set; }

        [MaxLength(255)]
        public string? Titulo { get; set; }

        // Se usa `string?` y no se especifica MaxLength para que EF Core lo mapee
        // a un tipo de dato de texto largo (NVARCHAR(MAX) en SQL Server).
        public string? Texto { get; set; }

        public DateTime FechaPublicacion { get; set; }

        // RELACIÓN CORREGIDA: El contenido ahora pertenece a una Página.
        [ForeignKey("Pagina")]
        public int PaginaId { get; set; }
        public virtual Pagina Pagina { get; set; } = null!;
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



}
