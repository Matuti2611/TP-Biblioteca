using System;

namespace Biblioteca_Pregot_Rubio
{
    public class DevolucionFlujo
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

            var prestamosActivos = gestor.obtenerPrestamosActivos(nroSocio);
            if (prestamosActivos.Count == 0)
            {
                Console.WriteLine("El socio no tiene prestamos activos.");
                return;
            }

            Console.WriteLine("Prestamos activos del socio:");
            for (int i = 0; i < prestamosActivos.Count; i++)
            {
                var p = prestamosActivos[i];
                Console.WriteLine($"{i + 1}. {p.libro?.titulo} (ISBN: {p.isbn}) | Prestado el: {p.fechaPrestamo:dd/MM/yyyy} | Vencimiento: {p.fechaVencimiento:dd/MM/yyyy}");
            }

            Console.Write("Seleccione el numero de prestamo a devolver: ");
            if (!int.TryParse(Console.ReadLine(), out int indexPrestamo) || indexPrestamo < 1 || indexPrestamo > prestamosActivos.Count)
            {
                Console.WriteLine("Seleccion invalida.");
                return;
            }

            var prestamo = prestamosActivos[indexPrestamo - 1];
            string resultadoDev = gestor.registrarDevolucion(prestamo.idPrestamo, out decimal multaGenerada);

            if (multaGenerada > 0)
            {
                Console.WriteLine($"Devolucion registrada con retraso. Se genero una multa de ${multaGenerada}.");
            }
            else
            {
                Console.WriteLine("Devolucion registrada a tiempo sin multas.");
            }

            if (!string.IsNullOrEmpty(resultadoDev))
            {
                Console.WriteLine(resultadoDev);
            }
        }
    }
}
