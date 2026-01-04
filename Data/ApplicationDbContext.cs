using Microsoft.EntityFrameworkCore;
using PulseTrain.Domain.Entities;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Estado> Estados { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Seed de estados iniciales
        modelBuilder
            .Entity<Estado>()
            .HasData(
                new Estado { Id = 1, Status = "Activo" },
                new Estado { Id = 2, Status = "Inactivo" },
                new Estado { Id = 3, Status = "Suspendido" }
            );

        // Relación Cliente -> Estado
        modelBuilder
            .Entity<Cliente>()
            .HasOne(c => c.Estado)
            .WithMany()
            .HasForeignKey(c => c.EstadoId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
