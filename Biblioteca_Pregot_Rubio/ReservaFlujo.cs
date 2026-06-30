using System;

namespace Biblioteca_Pregot_Rubio
{
    public class ReservaFlujo
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
                Console.WriteLine("RN-01: El socio esta inactivo.");
                return;
            }

            Console.Write("Ingrese el ISBN del libro a reservar: ");
            string? isbn = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(isbn))
            {
                Console.WriteLine("ISBN invalido.");
                return;
            }

            var libro = gestor.obtenerLibroPorISBN(isbn);
            if (libro == null)
            {
                Console.WriteLine("Libro no encontrado.");
                return;
            }

            bool yaReservado = gestor.yaTieneReservaActiva(socio.nroSocio, libro.isbn);
            if (yaReservado)
            {
                Console.WriteLine("RN-08: El socio ya tiene una reserva activa para este libro.");
                return;
            }

            gestor.crearReserva(socio.nroSocio, libro.isbn);
            Console.WriteLine("Reserva registrada con exito.");
        }
    }
}
