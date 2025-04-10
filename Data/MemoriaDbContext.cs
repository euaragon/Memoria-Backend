
using Microsoft.EntityFrameworkCore;
using MemoriaAPI.Models;

namespace MemoriaAPI.Data
{
    public class MemoriaDbContext : DbContext
    {
        public MemoriaDbContext(DbContextOptions<MemoriaDbContext> options) : base(options) { }
        public DbSet<Pagina> Paginas => Set<Pagina>();
        public DbSet<Seccion> Secciones => Set<Seccion>();
        public DbSet<Contenido> Contenidos => Set<Contenido>();
        public DbSet<Usuario> Usuarios => Set<Usuario>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Relación uno a muchos: Pagina - Seccion
            modelBuilder.Entity<Seccion>()
                .HasOne(s => s.Pagina)
                .WithMany() // Una pagina puede tener muchas secciones
                .HasForeignKey(s => s.IdPagina)
                .OnDelete(DeleteBehavior.Cascade); // Si se borra una página, se borran sus secciones

            // Relación uno a muchos: Seccion - Contenido
            modelBuilder.Entity<Contenido>()
                .HasOne(c => c.Seccion)
                .WithMany() // Una sección puede tener muchos contenidos
                .HasForeignKey(c => c.IdSeccion)
                .OnDelete(DeleteBehavior.Cascade); // Si se borra una sección, se borran sus contenidos

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
