using Microsoft.EntityFrameworkCore;
using MemoriaAPI.Models;

namespace MemoriaAPI.Data
{
    public class MemoriaDbContext : DbContext
    {
        public MemoriaDbContext(DbContextOptions<MemoriaDbContext> options) : base(options) { }

        // DbSets para tus entidades. El modelo Usuario debería ser reemplazado por Identity.
        public DbSet<Pagina> Paginas => Set<Pagina>();
        public DbSet<Seccion> Secciones => Set<Seccion>();
        public DbSet<Contenido> Contenidos => Set<Contenido>();
        public DbSet<Usuario> Usuarios => Set<Usuario>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // --- RELACIONES CORREGIDAS ---

            // Relación uno a muchos: Seccion -> Pagina
            // Una Seccion tiene muchas Paginas.
            modelBuilder.Entity<Seccion>()
                .HasMany(s => s.Paginas) // Una Seccion tiene una colección de Paginas
                .WithOne(p => p.Seccion) // Cada Pagina pertenece a una Seccion
                .HasForeignKey(p => p.SeccionId) // La llave foránea está en la tabla Pagina
                .OnDelete(DeleteBehavior.Cascade); // Si se borra una sección, se borran sus páginas.

            // Relación uno a muchos: Pagina -> Contenido
            // Una Pagina tiene muchos Contenidos.
            modelBuilder.Entity<Pagina>()
                .HasMany(p => p.Contenidos) // Una Pagina tiene una colección de Contenidos
                .WithOne(c => c.Pagina) // Cada Contenido pertenece a una Pagina
                .HasForeignKey(c => c.PaginaId) // La llave foránea está en la tabla Contenido
                .OnDelete(DeleteBehavior.Cascade); // Si se borra una página, se borran sus contenidos.



            // Opcional: Configuración de índices únicos
            modelBuilder.Entity<Pagina>()
                .HasIndex(p => p.Url)
                .IsUnique();

            modelBuilder.Entity<Seccion>()
                .HasIndex(s => s.Url)
                .IsUnique();
        }
    }
}