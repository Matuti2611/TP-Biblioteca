using Biblioteca_Pregot_Rubio.Data;
using Biblioteca_Pregot_Rubio.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Biblioteca_Pregot_Rubio
{
    public class GestorBiblioteca
    {
        private readonly BibliotecaContext _context;

        public GestorBiblioteca(BibliotecaContext context)
        {
            _context = context;
        }

        public List<Libro> obtenerLibrosDisponibles()
        {
            return _context.libros.Include(l => l.prestamos).ToList();
        }

        public int obtenerCopiasDisponibles(Libro libro)
        {
            int prestamosActivos = libro.prestamos.Count(p => p.fechaDevolucion == null);
            int disponibles = libro.cantidadCopias - prestamosActivos;
            return disponibles < 0 ? 0 : disponibles;
        }

        public Socio? obtenerSocio(int nroSocio)
        {
            return _context.socios
                .Include(s => s.tipoSocio)
                .Include(s => s.prestamos)
                .Include(s => s.multas)
                .FirstOrDefault(s => s.nroSocio == nroSocio);
        }

        public List<Libro> buscarLibros(string busqueda)
        {
            return _context.libros
                .Include(l => l.prestamos)
                .Where(l => l.titulo.ToLower().Contains(busqueda.ToLower()) || l.autor.ToLower().Contains(busqueda.ToLower()))
                .ToList();
        }

        public void realizarPrestamo(int nroSocio, string isbn, out DateTime fechaVencimiento)
        {
            var socio = obtenerSocio(nroSocio);
            int diasPrestamo = socio?.tipoSocio?.diasPrestamo ?? 7;
            fechaVencimiento = DateTime.Now.AddDays(diasPrestamo);

            var nuevoPrestamo = new Prestamo
            {
                nroSocio = nroSocio,
                isbn = isbn,
                fechaPrestamo = DateTime.Now,
                fechaVencimiento = fechaVencimiento,
                idEstadoPrestamo = 1,
                renovado = false
            };

            _context.prestamos.Add(nuevoPrestamo);
            _context.SaveChanges();
        }

        public List<Prestamo> obtenerPrestamosActivos(int nroSocio)
        {
            return _context.prestamos
                .Include(p => p.libro)
                .Where(p => p.nroSocio == nroSocio && p.fechaDevolucion == null)
                .ToList();
        }

        public string registrarDevolucion(int idPrestamo, out decimal multaGenerada)
        {
            multaGenerada = 0;
            var prestamo = _context.prestamos
                .Include(p => p.socio).ThenInclude(s => s!.tipoSocio)
                .FirstOrDefault(p => p.idPrestamo == idPrestamo);

            if (prestamo == null) return "Prestamo no encontrado.";

            DateTime fechaDev = DateTime.Now;
            prestamo.fechaDevolucion = fechaDev;
            prestamo.idEstadoPrestamo = 2;

            if (fechaDev.Date > prestamo.fechaVencimiento.Date)
            {
                int diasDemora = (fechaDev.Date - prestamo.fechaVencimiento.Date).Days;
                decimal multaPorDia = prestamo.socio?.tipoSocio?.multaPorDia ?? 0;
                multaGenerada = diasDemora * multaPorDia;

                if (multaGenerada > 0)
                {
                    var multa = new Multa
                    {
                        idPrestamo = prestamo.idPrestamo,
                        nroSocio = prestamo.nroSocio,
                        monto = multaGenerada,
                        pagada = false,
                        fechaGeneracion = fechaDev
                    };
                    _context.multas.Add(multa);
                }
            }

            var reservaPendiente = _context.reservas
                .Include(r => r.socio)
                .Where(r => r.isbn == prestamo.isbn && r.idEstadoReserva == 1)
                .OrderBy(r => r.fechaReserva)
                .FirstOrDefault();

            string mensajeReserva = "";
            if (reservaPendiente != null)
            {
                reservaPendiente.idEstadoReserva = 2;
                mensajeReserva = $"RN-07: Se cumplio la reserva mas antigua del libro. Notificado socio: {reservaPendiente.socio?.nombre} {reservaPendiente.socio?.apellido} (Nro: {reservaPendiente.nroSocio}).";
            }

            _context.SaveChanges();
            return mensajeReserva;
        }

        public Libro? obtenerLibroPorISBN(string isbn)
        {
            return _context.libros.FirstOrDefault(l => l.isbn == isbn);
        }

        public bool yaTieneReservaActiva(int nroSocio, string isbn)
        {
            return _context.reservas.Any(r => r.nroSocio == nroSocio && r.isbn == isbn && r.idEstadoReserva == 1);
        }

        public void crearReserva(int nroSocio, string isbn)
        {
            var reserva = new Reserva
            {
                nroSocio = nroSocio,
                isbn = isbn,
                fechaReserva = DateTime.Now,
                idEstadoReserva = 1
            };

            _context.reservas.Add(reserva);
            _context.SaveChanges();
        }

        public void pagarMultasSocio(int nroSocio)
        {
            var multasPendientes = _context.multas.Where(m => m.nroSocio == nroSocio && !m.pagada).ToList();
            foreach (var m in multasPendientes)
            {
                m.pagada = true;
            }
            _context.SaveChanges();
        }

        public List<(Libro libro, int count)> obtenerLibrosMasPrestados()
        {
            var report = _context.prestamos
                .GroupBy(p => p.isbn)
                .Select(g => new { isbn = g.Key, prestamosCount = g.Count() })
                .OrderByDescending(x => x.prestamosCount)
                .Take(5)
                .ToList();

            var result = new List<(Libro libro, int count)>();
            foreach (var item in report)
            {
                var libro = _context.libros.FirstOrDefault(l => l.isbn == item.isbn);
                if (libro != null)
                {
                    result.Add((libro, item.prestamosCount));
                }
            }
            return result;
        }

        public List<Socio> obtenerSociosConMultasPendientes()
        {
            return _context.socios
                .Include(s => s.multas)
                .ToList()
                .Where(s => s.multas.Any(m => !m.pagada))
                .ToList();
        }

        public List<Prestamo> obtenerPrestamosVencidos()
        {
            return _context.prestamos
                .Include(p => p.libro)
                .Include(p => p.socio)
                .Where(p => p.fechaDevolucion == null && DateTime.Now > p.fechaVencimiento)
                .ToList();
        }

        public List<Libro> buscarLibrosDisponibilidad(string input)
        {
            return _context.libros
                .Include(l => l.prestamos)
                .Include(l => l.reservas)
                .Where(l => l.isbn == input || l.titulo.ToLower().Contains(input.ToLower()))
                .ToList();
        }

        public Socio? obtenerHistorialSocio(int nroSocio)
        {
            return _context.socios
                .Include(s => s.prestamos).ThenInclude(p => p.libro)
                .Include(s => s.reservas).ThenInclude(r => r.libro)
                .FirstOrDefault(s => s.nroSocio == nroSocio);
        }
    }
}
