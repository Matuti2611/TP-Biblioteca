using Biblioteca_Pregot_Rubio.Models;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca_Pregot_Rubio.Data
{
    public class BibliotecaContext : DbContext
    {
        public DbSet<Libro> libros { get; set; }
        public DbSet<Socio> socios { get; set; }
        public DbSet<TipoSocio> tiposSocio { get; set; }
        public DbSet<Prestamo> prestamos { get; set; }
        public DbSet<Reserva> reservas { get; set; }
        public DbSet<Multa> multas { get; set; }
        public DbSet<EstadoPrestamo> estadosPrestamo { get; set; }
        public DbSet<EstadoReserva> estadoReserva { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string dbPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "biblioteca.db");
            optionsBuilder.UseSqlite($"Data Source={dbPath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Libro>(entity =>
            {
                entity.ToTable("Libros");
                entity.HasKey(e => e.isbn);

                entity.Property(e => e.isbn).HasColumnName("ISBN");
                entity.Property(e => e.titulo).HasColumnName("Titulo");
                entity.Property(e => e.autor).HasColumnName("Autor");
                entity.Property(e => e.genero).HasColumnName("Genero");
                entity.Property(e => e.cantidadCopias).HasColumnName("CantidadCopias");
            });

            modelBuilder.Entity<TipoSocio>(entity =>
            {
                entity.ToTable("TiposSocio");
                entity.HasKey(e => e.idTipoSocio);

                entity.Property(e => e.idTipoSocio).HasColumnName("IdTipoSocio");
                entity.Property(e => e.nombre).HasColumnName("Nombre");
                entity.Property(e => e.maxLibros).HasColumnName("MaxLibros");
                entity.Property(e => e.diasPrestamo).HasColumnName("DiasPrestamo");
                entity.Property(e => e.multaPorDia).HasColumnName("MultaPorDia");
            });

            modelBuilder.Entity<Socio>(entity =>
            {
                entity.ToTable("Socios");
                entity.HasKey(e => e.nroSocio);

                entity.Property(e => e.nroSocio).HasColumnName("NroSocio");
                entity.Property(e => e.nombre).HasColumnName("Nombre");
                entity.Property(e => e.apellido).HasColumnName("Apellido");
                entity.Property(e => e.email).HasColumnName("Email");
                entity.Property(e => e.idTipoSocio).HasColumnName("IdTipoSocio");
                entity.Property(e => e.activo).HasColumnName("Activo");

                entity.HasOne(e => e.tipoSocio)
                    .WithMany(e => e.socios)
                    .HasForeignKey(e => e.idTipoSocio);
            });

            modelBuilder.Entity<EstadoPrestamo>(entity =>
            {
                entity.ToTable("EstadosPrestamo");
                entity.HasKey(e => e.idEstadoPrestamo);

                entity.Property(e => e.idEstadoPrestamo).HasColumnName("IdEstadoPrestamo");
                entity.Property(e => e.nombre).HasColumnName("Nombre");
            });

            modelBuilder.Entity<EstadoReserva>(entity =>
            {
                entity.ToTable("EstadoReserva");
                entity.HasKey(e => e.idEstadoReserva);

                entity.Property(e => e.idEstadoReserva).HasColumnName("IdEstadoReserva");
                entity.Property(e => e.nombre).HasColumnName("Nombre");
            });

            modelBuilder.Entity<Prestamo>(entity =>
            {
                entity.ToTable("Prestamos");
                entity.HasKey(e => e.idPrestamo);

                entity.Property(e => e.idPrestamo).HasColumnName("IdPrestamo");
                entity.Property(e => e.nroSocio).HasColumnName("NroSocio");
                entity.Property(e => e.isbn).HasColumnName("ISBN");
                entity.Property(e => e.fechaPrestamo).HasColumnName("FechaPrestamo");
                entity.Property(e => e.fechaVencimiento).HasColumnName("FechaVencimiento");
                entity.Property(e => e.fechaDevolucion).HasColumnName("FechaDevolucion");
                entity.Property(e => e.idEstadoPrestamo).HasColumnName("IdEstadoPrestamo");
                entity.Property(e => e.renovado).HasColumnName("Renovado");

                entity.HasOne(e => e.socio)
                    .WithMany(e => e.prestamos)
                    .HasForeignKey(e => e.nroSocio);

                entity.HasOne(e => e.libro)
                    .WithMany(e => e.prestamos)
                    .HasForeignKey(e => e.isbn);

                entity.HasOne(e => e.estadoPrestamo)
                    .WithMany(e => e.prestamos)
                    .HasForeignKey(e => e.idEstadoPrestamo);
            });

            modelBuilder.Entity<Reserva>(entity =>
            {
                entity.ToTable("Reservas");
                entity.HasKey(e => e.idReserva);

                entity.Property(e => e.idReserva).HasColumnName("IdReserva");
                entity.Property(e => e.nroSocio).HasColumnName("NroSocio");
                entity.Property(e => e.isbn).HasColumnName("ISBN");
                entity.Property(e => e.fechaReserva).HasColumnName("FechaReserva");
                entity.Property(e => e.idEstadoReserva).HasColumnName("IdEstadoReserva");

                entity.HasOne(e => e.socio)
                    .WithMany(e => e.reservas)
                    .HasForeignKey(e => e.nroSocio);

                entity.HasOne(e => e.libro)
                    .WithMany(e => e.reservas)
                    .HasForeignKey(e => e.isbn);

                entity.HasOne(e => e.estadoReserva)
                    .WithMany(e => e.reservas)
                    .HasForeignKey(e => e.idEstadoReserva);
            });

            modelBuilder.Entity<Multa>(entity =>
            {
                entity.ToTable("Multas");
                entity.HasKey(e => e.idMulta);

                entity.Property(e => e.idMulta).HasColumnName("IdMulta");
                entity.Property(e => e.idPrestamo).HasColumnName("IdPrestamo");
                entity.Property(e => e.nroSocio).HasColumnName("NroSocio");
                entity.Property(e => e.monto).HasColumnName("Monto");
                entity.Property(e => e.pagada).HasColumnName("Pagada");
                entity.Property(e => e.fechaGeneracion).HasColumnName("FechaGeneracion");

                entity.HasOne(e => e.prestamo)
                    .WithMany(e => e.multas)
                    .HasForeignKey(e => e.idPrestamo);

                entity.HasOne(e => e.socio)
                    .WithMany(e => e.multas)
                    .HasForeignKey(e => e.nroSocio);
            });
        }
    }
}