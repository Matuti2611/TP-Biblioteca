using System;
using System.Linq;

namespace Biblioteca_Pregot_Rubio
{
    public class SocioFlujo
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

            string estadoSocio = socio.activo ? "Activo" : "Inactivo";
            Console.WriteLine($"Socio Nro: {socio.nroSocio}");
            Console.WriteLine($"Nombre: {socio.nombre} {socio.apellido}");
            Console.WriteLine($"Email: {socio.email}");
            Console.WriteLine($"Tipo Socio: {socio.tipoSocio?.nombre} (Limite: {socio.tipoSocio?.maxLibros} libros, {socio.tipoSocio?.diasPrestamo} dias)");
            Console.WriteLine($"Estado: {estadoSocio}");
            Console.WriteLine();

            var prestamosActivos = socio.prestamos.Where(p => p.fechaDevolucion == null).ToList();
            Console.WriteLine("=== PRESTAMOS ACTIVOS ===");
            if (prestamosActivos.Count == 0)
            {
                Console.WriteLine("Sin prestamos activos.");
            }
            else
            {
                foreach (var p in prestamosActivos)
                {
                    Console.WriteLine($"- {p.libro?.titulo} (ISBN: {p.isbn}) | Prestado: {p.fechaPrestamo:dd/MM/yyyy} | Vence: {p.fechaVencimiento:dd/MM/yyyy}");
                }
            }
            Console.WriteLine();

            var prestamosDevueltos = socio.prestamos.Where(p => p.fechaDevolucion != null).ToList();
            Console.WriteLine("=== HISTORIAL DE DEVOLUCIONES ===");
            if (prestamosDevueltos.Count == 0)
            {
                Console.WriteLine("Sin devoluciones registradas.");
            }
            else
            {
                foreach (var p in prestamosDevueltos)
                {
                    Console.WriteLine($"- {p.libro?.titulo} (ISBN: {p.isbn}) | Devuelto: {p.fechaDevolucion:dd/MM/yyyy}");
                }
            }
            Console.WriteLine();

            var multasPendientes = socio.multas.Where(m => !m.pagada).ToList();
            Console.WriteLine("=== MULTAS PENDIENTES ===");
            if (multasPendientes.Count == 0)
            {
                Console.WriteLine("Sin multas pendientes.");
            }
            else
            {
                decimal total = 0;
                foreach (var m in multasPendientes)
                {
                    Console.WriteLine($"- ID Multa: {m.idMulta} | Monto: ${m.monto} | Generada: {m.fechaGeneracion:dd/MM/yyyy}");
                    total += m.monto;
                }
                Console.WriteLine($"Total pendiente: ${total}");
                Console.Write("¿Desea abonar todas las multas pendientes en este momento? (s/n): ");
                string? rta = Console.ReadLine();
                if (rta?.ToLower() == "s")
                {
                    gestor.pagarMultasSocio(socio.nroSocio);
                    Console.WriteLine("Multas abonadas con exito.");
                }
            }
        }
    }
}
