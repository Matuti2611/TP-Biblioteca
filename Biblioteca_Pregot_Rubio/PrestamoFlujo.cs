using Biblioteca_Pregot_Rubio.Models;
using System;
using System.Linq;

namespace Biblioteca_Pregot_Rubio
{
    public class PrestamoFlujo
    {
        public static void ejecutar(GestorBiblioteca gestor)
        {
            Console.Write("Ingrese el numero de socio: ");
            if (!int.TryParse(Console.ReadLine(), out int nroSocio))
            {
                Console.WriteLine("Numero de socio invalido.");
                return;
            }

            var socio = gestor.obtenerSocio(nroSocio);
            if (socio == null)
            {
                Console.WriteLine("Socio no encontrado.");
                return;
            }

            if (!socio.activo)
            {
                Console.WriteLine("RN-01: El socio esta inactivo y no puede realizar prestamos.");
                return;
            }

            if (socio.multas.Any(m => !m.pagada))
            {
                Console.WriteLine("RN-02: El socio tiene multas pendientes de pago.");
                return;
            }

            int prestamosActivos = socio.prestamos.Count(p => p.fechaDevolucion == null);
            if (socio.tipoSocio != null && prestamosActivos >= socio.tipoSocio.maxLibros)
            {
                Console.WriteLine($"RN-04: El socio ha alcanzado el limite maximo de prestamos simultaneos ({socio.tipoSocio.maxLibros}).");
                return;
            }

            Console.Write("Ingrese titulo o autor del libro a buscar: ");
            string? busqueda = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(busqueda))
            {
                Console.WriteLine("Busqueda invalida.");
                return;
            }

            var librosCoincidentes = gestor.buscarLibros(busqueda);
            if (librosCoincidentes.Count == 0)
            {
                Console.WriteLine("No se encontraron libros que coincidan con la busqueda.");
                return;
            }

            Console.WriteLine("Libros encontrados:");
            for (int i = 0; i < librosCoincidentes.Count; i++)
            {
                var l = librosCoincidentes[i];
                int disponibles = gestor.obtenerCopiasDisponibles(l);
                Console.WriteLine($"{i + 1}. {l.titulo} - {l.autor} (ISBN: {l.isbn}) | Disponibles: {disponibles}");
            }

            Console.Write("Seleccione el numero de libro: ");
            if (!int.TryParse(Console.ReadLine(), out int indexLibro) || indexLibro < 1 || indexLibro > librosCoincidentes.Count)
            {
                Console.WriteLine("Seleccion invalida.");
                return;
            }

            var libroSeleccionado = librosCoincidentes[indexLibro - 1];
            int disponiblesLibro = gestor.obtenerCopiasDisponibles(libroSeleccionado);

            if (disponiblesLibro <= 0)
            {
                Console.WriteLine("RN-03: No hay copias disponibles para este libro.");
                Console.Write("¿Desea reservar el libro? (s/n): ");
                string? rta = Console.ReadLine();
                if (rta?.ToLower() == "s")
                {
                    crearReserva(gestor, socio.nroSocio, libroSeleccionado.isbn);
                }
                return;
            }

            gestor.realizarPrestamo(socio.nroSocio, libroSeleccionado.isbn, out DateTime fechaVencimiento);
            Console.WriteLine($"Prestamo registrado con exito. Fecha de vencimiento: {fechaVencimiento:dd/MM/yyyy}");
        }

        private static void crearReserva(GestorBiblioteca gestor, int nroSocio, string isbn)
        {
            bool yaReservado = gestor.yaTieneReservaActiva(nroSocio, isbn);
            if (yaReservado)
            {
                Console.WriteLine("RN-08: El socio ya tiene una reserva activa para este libro.");
                return;
            }

            gestor.crearReserva(nroSocio, isbn);
            Console.WriteLine("Reserva registrada con exito.");
        }
    }
}
