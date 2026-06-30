using Microsoft.EntityFrameworkCore;
using GestionSolicitudes.Infrastructure.Entities;

namespace GestionSolicitudes.Infrastructure.Persistence
{
    public class PruebaTecnicaDbContext : DbContext
    {
        public PruebaTecnicaDbContext(DbContextOptions<PruebaTecnicaDbContext> options) 
            : base(options)
        {
        }

        public DbSet<Rol> Roles { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Estado> Estados { get; set; }
        public DbSet<Solicitud> Solicitudes { get; set; }
        public DbSet<Seguimiento> Seguimientos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuraciones para la tabla Roles
            modelBuilder.Entity<Rol>(entity =>
            {
                entity.ToTable("Roles");
                entity.HasKey(e => e.Id).HasName("PK_Roles");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Nombre).HasColumnName("nombre").HasMaxLength(50).IsRequired();
                entity.Property(e => e.FechaCreacion).HasColumnName("fecha_creacion").HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.FechaModificacion).HasColumnName("fecha_modificacion");
                entity.HasIndex(e => e.Nombre).IsUnique();
            });

            // Configuraciones para la tabla Usuarios
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("Usuarios");
                entity.HasKey(e => e.Id).HasName("PK_Usuarios");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.NombreUsuario).HasColumnName("nombre_usuario").HasMaxLength(100).IsRequired();
                entity.Property(e => e.Contrasenia).HasColumnName("contrasenia").HasMaxLength(100).IsRequired();
                entity.Property(e => e.RolId).HasColumnName("rol_id");
                entity.Property(e => e.Activo).HasColumnName("activo");
                entity.Property(e => e.FechaCreacion).HasColumnName("fecha_creacion").HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.FechaModificacion).HasColumnName("fecha_modificacion");
                entity.HasIndex(e => e.NombreUsuario).IsUnique();
            });

            // Configuraciones para la tabla Estados
            modelBuilder.Entity<Estado>(entity =>
            {
                entity.ToTable("Estados");
                entity.HasKey(e => e.Id).HasName("PK_Estados");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Nombre).HasColumnName("nombre").HasMaxLength(50).IsRequired();
                entity.Property(e => e.FechaCreacion).HasColumnName("fecha_creacion").HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.FechaModificacion).HasColumnName("fecha_modificacion");
                entity.HasIndex(e => e.Nombre).IsUnique();
            });

            // Configuraciones para la tabla Solicitudes
            modelBuilder.Entity<Solicitud>(entity =>
            {
                entity.ToTable("Solicitudes");
                entity.HasKey(e => e.Id).HasName("PK_Solicitudes");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Numero).HasColumnName("numero").HasMaxLength(50).IsRequired();
                entity.Property(e => e.FechaSolicitud).HasColumnName("fecha_solicitud").HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.UsuarioId).HasColumnName("usuario_id");
                entity.Property(e => e.Tipo).HasColumnName("tipo").HasMaxLength(50);
                entity.Property(e => e.EstadoId).HasColumnName("estado_id");
                entity.Property(e => e.Observaciones).HasColumnName("observaciones");
                entity.Property(e => e.FechaCreacion).HasColumnName("fecha_creacion").HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.FechaModificacion).HasColumnName("fecha_modificacion");
                entity.HasIndex(e => e.Numero).IsUnique();
                
                // Configurar relaciones
                entity.HasOne(d => d.Usuario)
                    .WithMany(p => p.Solicitudes)
                    .HasForeignKey(d => d.UsuarioId)
                    .OnDelete(DeleteBehavior.Restrict);
                    
                entity.HasOne(d => d.Estado)
                    .WithMany(p => p.Solicitudes)
                    .HasForeignKey(d => d.EstadoId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configuraciones para la tabla Seguimientos
            modelBuilder.Entity<Seguimiento>(entity =>
            {
                entity.ToTable("Seguimientos");
                entity.HasKey(e => e.Id).HasName("PK_Seguimientos");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.SolicitudId).HasColumnName("solicitud_id");
                entity.Property(e => e.UsuarioId).HasColumnName("usuario_id");
                entity.Property(e => e.FechaSeguimiento).HasColumnName("fecha_seguimiento").HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.EstadoId).HasColumnName("estado_id");
                entity.Property(e => e.Comentario).HasColumnName("comentario");
                entity.Property(e => e.FechaCreacion).HasColumnName("fecha_creacion").HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.FechaModificacion).HasColumnName("fecha_modificacion");

                // Configurar relaciones
                entity.HasOne(d => d.Solicitud)
                    .WithMany(p => p.Seguimientos)
                    .HasForeignKey(d => d.SolicitudId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.Usuario)
                    .WithMany(p => p.Seguimientos)
                    .HasForeignKey(d => d.UsuarioId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.Estado)
                    .WithMany(p => p.Seguimientos)
                    .HasForeignKey(d => d.EstadoId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}