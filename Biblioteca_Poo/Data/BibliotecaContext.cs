using Biblioteca_Poo.Models;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca_Poo.Data
{
    public class BibliotecaContext : DbContext
    {
        public DbSet<Libro> Libros { get; set; }
        public DbSet<Socio> Socios { get; set; }
        public DbSet<TipoSocio> TiposSocio { get; set; }
        public DbSet<Prestamo> Prestamos { get; set; }
        public DbSet<Reserva> Reservas { get; set; }
        public DbSet<Multa> Multas { get; set; }
        public DbSet<EstadoPrestamo> EstadosPrestamo { get; set; }
        public DbSet<EstadoReserva> EstadoReserva { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=biblioteca.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Libro>(entity =>
            {
                entity.ToTable("Libros");

                entity.HasKey(e => e.ISBN);

                entity.Property(e => e.ISBN)
                    .HasColumnName("ISBN");

                entity.Property(e => e.Titulo)
                    .HasColumnName("Titulo");

                entity.Property(e => e.Autor)
                    .HasColumnName("Autor");

                entity.Property(e => e.Genero)
                    .HasColumnName("Genero");

                entity.Property(e => e.CantidadCopias)
                    .HasColumnName("CantidadCopias");
            });

            modelBuilder.Entity<TipoSocio>(entity =>
            {
                entity.ToTable("TiposSocio");

                entity.HasKey(e => e.IdTipoSocio);

                entity.Property(e => e.IdTipoSocio)
                    .HasColumnName("IdTipoSocio");

                entity.Property(e => e.Nombre)
                    .HasColumnName("Nombre");

                entity.Property(e => e.MaxLibros)
                    .HasColumnName("MaxLibros");

                entity.Property(e => e.DiasPrestamo)
                    .HasColumnName("DiasPrestamo");

                entity.Property(e => e.MultaPorDia)
                    .HasColumnName("MultaPorDia");
            });

            modelBuilder.Entity<Socio>(entity =>
            {
                entity.ToTable("Socios");

                entity.HasKey(e => e.NroSocio);

                entity.Property(e => e.NroSocio)
                    .HasColumnName("NroSocio");

                entity.Property(e => e.Nombre)
                    .HasColumnName("Nombre");

                entity.Property(e => e.Apellido)
                    .HasColumnName("Apellido");

                entity.Property(e => e.Email)
                    .HasColumnName("Email");

                entity.Property(e => e.IdTipoSocio)
                    .HasColumnName("IdTipoSocio");

                entity.Property(e => e.Activo)
                    .HasColumnName("Activo");

                entity.HasOne(e => e.TipoSocio)
                    .WithMany(e => e.Socios)
                    .HasForeignKey(e => e.IdTipoSocio);
            });

            modelBuilder.Entity<EstadoPrestamo>(entity =>
            {
                entity.ToTable("EstadosPrestamo");

                entity.HasKey(e => e.IdEstadoPrestamo);

                entity.Property(e => e.IdEstadoPrestamo)
                    .HasColumnName("IdEstadoPrestamo");

                entity.Property(e => e.Nombre)
                    .HasColumnName("Nombre");
            });

            modelBuilder.Entity<EstadoReserva>(entity =>
            {
                entity.ToTable("EstadoReserva");

                entity.HasKey(e => e.IdEstadoReserva);

                entity.Property(e => e.IdEstadoReserva)
                    .HasColumnName("IdEstadoReserva");

                entity.Property(e => e.Nombre)
                    .HasColumnName("Nombre");
            });

            modelBuilder.Entity<Prestamo>(entity =>
            {
                entity.ToTable("Prestamos");

                entity.HasKey(e => e.IdPrestamo);

                entity.Property(e => e.IdPrestamo)
                    .HasColumnName("IdPrestamo");

                entity.Property(e => e.NroSocio)
                    .HasColumnName("NroSocio");

                entity.Property(e => e.ISBN)
                    .HasColumnName("ISBN");

                entity.Property(e => e.FechaPrestamo)
                    .HasColumnName("FechaPrestamo");

                entity.Property(e => e.FechaVencimiento)
                    .HasColumnName("FechaVencimiento");

                entity.Property(e => e.FechaDevolucion)
                    .HasColumnName("FechaDevolucion");

                entity.Property(e => e.IdEstadoPrestamo)
                    .HasColumnName("IdEstadoPrestamo");

                entity.Property(e => e.Renovado)
                    .HasColumnName("Renovado");

                entity.HasOne(e => e.Socio)
                    .WithMany(e => e.Prestamos)
                    .HasForeignKey(e => e.NroSocio);

                entity.HasOne(e => e.Libro)
                    .WithMany(e => e.Prestamos)
                    .HasForeignKey(e => e.ISBN);

                entity.HasOne(e => e.EstadoPrestamo)
                    .WithMany(e => e.Prestamos)
                    .HasForeignKey(e => e.IdEstadoPrestamo);
            });

            modelBuilder.Entity<Reserva>(entity =>
            {
                entity.ToTable("Reservas");

                entity.HasKey(e => e.IdReserva);

                entity.Property(e => e.IdReserva)
                    .HasColumnName("IdReserva");

                entity.Property(e => e.NroSocio)
                    .HasColumnName("NroSocio");

                entity.Property(e => e.ISBN)
                    .HasColumnName("ISBN");

                entity.Property(e => e.FechaReserva)
                    .HasColumnName("FechaReserva");

                entity.Property(e => e.IdEstadoReserva)
                    .HasColumnName("IdEstadoReserva");

                entity.HasOne(e => e.Socio)
                    .WithMany(e => e.Reservas)
                    .HasForeignKey(e => e.NroSocio);

                entity.HasOne(e => e.Libro)
                    .WithMany(e => e.Reservas)
                    .HasForeignKey(e => e.ISBN);

                entity.HasOne(e => e.EstadoReserva)
                    .WithMany(e => e.Reservas)
                    .HasForeignKey(e => e.IdEstadoReserva);
            });

            modelBuilder.Entity<Multa>(entity =>
            {
                entity.ToTable("Multas");

                entity.HasKey(e => e.IdMulta);

                entity.Property(e => e.IdMulta)
                    .HasColumnName("IdMulta");

                entity.Property(e => e.IdPrestamo)
                    .HasColumnName("IdPrestamo");

                entity.Property(e => e.NroSocio)
                    .HasColumnName("NroSocio");

                entity.Property(e => e.Monto)
                    .HasColumnName("Monto");

                entity.Property(e => e.Pagada)
                    .HasColumnName("Pagada");

                entity.Property(e => e.FechaGeneracion)
                    .HasColumnName("FechaGeneracion");

                entity.HasOne(e => e.Prestamo)
                    .WithMany(e => e.Multas)
                    .HasForeignKey(e => e.IdPrestamo);

                entity.HasOne(e => e.Socio)
                    .WithMany(e => e.Multas)
                    .HasForeignKey(e => e.NroSocio);
            });
        }
    }
}