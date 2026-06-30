using System;
using System.Linq;

namespace Biblioteca_Pregot_Rubio
{
    public class ReportesFlujo
    {
        public static void ejecutar(GestorBiblioteca gestor)
        {
            bool volver = false;
            while (!volver)
            {
                Console.WriteLine("=== MENU REPORTES ===");
                Console.WriteLine("1. Libros mas prestados (Top 5)");
                Console.WriteLine("2. Socios con multas pendientes");
                Console.WriteLine("3. Prestamos vencidos");
                Console.WriteLine("4. Disponibilidad de un libro");
                Console.WriteLine("5. Historial de un socio");
                Console.WriteLine("6. Volver al menu principal");
                Console.Write("Seleccione una opcion de reporte: ");

                string? opcion = Console.ReadLine();
                Console.WriteLine();

                switch (opcion)
                {
                    case "1":
                        reporteLibrosMasPrestados(gestor);
                        break;
                    case "2":
                        reporteSociosConMultasPendientes(gestor);
                        break;
                    case "3":
                        reportePrestamosVencidos(gestor);
                        break;
                    case "4":
                        reporteDisponibilidadLibro(gestor);
                        break;
                    case "5":
                        reporteHistorialSocio(gestor);
                        break;
                    case "6":
                        volver = true;
                        break;
                    default:
                        Console.WriteLine("Opcion invalida.");
                        break;
                }
                if (!volver)
                {
                    Console.WriteLine();
                }
            }
        }

        private static void reporteLibrosMasPrestados(GestorBiblioteca gestor)
        {
            Console.WriteLine("=== LIBROS MAS PRESTADOS ===");
            var report = gestor.obtenerLibrosMasPrestados();
            if (report.Count == 0)
            {
                Console.WriteLine("Sin registros de prestamos.");
                return;
            }

            foreach (var item in report)
            {
                Console.WriteLine($"- {item.libro.titulo} (Autor: {item.libro.autor}, ISBN: {item.libro.isbn}) | Prestamos historicos: {item.count}");
            }
        }

        private static void reporteSociosConMultasPendientes(GestorBiblioteca gestor)
        {
            Console.WriteLine("=== SOCIOS CON MULTAS PENDIENTES ===");
            var sociosMultados = gestor.obtenerSociosConMultasPendientes();
            if (sociosMultados.Count == 0)
            {
                Console.WriteLine("No hay socios con multas pendientes.");
                return;
            }

            foreach (var s in sociosMultados)
            {
                decimal total = s.multas.Where(m => !m.pagada).Sum(m => m.monto);
                Console.WriteLine($"- Socio Nro: {s.nroSocio} | Nombre: {s.nombre} {s.apellido} | Monto Total: ${total}");
            }
        }

        private static void reportePrestamosVencidos(GestorBiblioteca gestor)
        {
            Console.WriteLine("=== PRESTAMOS VENCIDOS ===");
            var vencidos = gestor.obtenerPrestamosVencidos();
            if (vencidos.Count == 0)
            {
                Console.WriteLine("No hay prestamos vencidos pendientes de devolucion.");
                return;
            }

            foreach (var p in vencidos)
            {
                int diasVencidos = (DateTime.Now.Date - p.fechaVencimiento.Date).Days;
                Console.WriteLine($"- {p.libro?.titulo} (ISBN: {p.isbn}) | Retirado por: {p.socio?.nombre} {p.socio?.apellido} (Nro: {p.nroSocio}) | Vencio el: {p.fechaVencimiento:dd/MM/yyyy} ({diasVencidos} dias de retraso)");
            }
        }

        private static void reporteDisponibilidadLibro(GestorBiblioteca gestor)
        {
            Console.Write("Ingrese ISBN o parte del titulo: ");
            string? input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Busqueda invalida.");
                return;
            }

            var libros = gestor.buscarLibrosDisponibilidad(input);
            if (libros.Count == 0)
            {
                Console.WriteLine("No se encontraron libros.");
                return;
            }

            foreach (var l in libros)
            {
                int disponibles = gestor.obtenerCopiasDisponibles(l);
                int reservasPendientes = l.reservas.Count(r => r.idEstadoReserva == 1);

                Console.WriteLine($"Libro: {l.titulo} (ISBN: {l.isbn})");
                Console.WriteLine($"- Copias totales: {l.cantidadCopias}");
                Console.WriteLine($"- Copias disponibles: {disponibles}");
                Console.WriteLine($"- Reservas pendientes: {reservasPendientes}");
            }
        }

        private static void reporteHistorialSocio(GestorBiblioteca gestor)
        {
            Console.Write("Ingrese el numero de socio: ");
            if (!int.TryParse(Console.ReadLine(), out int nroSocio))
            {
                Console.WriteLine("Numero de socio invalido.");
                return;
            }

            var socio = gestor.obtenerHistorialSocio(nroSocio);
            if (socio == null)
            {
                Console.WriteLine("Socio no encontrado.");
                return;
            }

            Console.WriteLine($"=== HISTORIAL DEL SOCIO {socio.nombre.ToUpper()} {socio.apellido.ToUpper()} (Nro: {socio.nroSocio}) ===");
            Console.WriteLine("--- PRESTAMOS ---");
            if (socio.prestamos.Count == 0)
            {
                Console.WriteLine("Sin prestamos en el historial.");
            }
            else
            {
                foreach (var p in socio.prestamos)
                {
                    string estado = "Desconocido";
                    if (p.fechaDevolucion != null)
                    {
                        estado = "Devuelto";
                    }
                    else if (DateTime.Now > p.fechaVencimiento)
                    {
                        estado = "Vencido";
                    }
                    else
                    {
                        estado = "Activo";
                    }
                    Console.WriteLine($"- {p.libro?.titulo} (ISBN: {p.isbn}) | Retiro: {p.fechaPrestamo:dd/MM/yyyy} | Vence: {p.fechaVencimiento:dd/MM/yyyy} | Devuelto: {(p.fechaDevolucion != null ? p.fechaDevolucion.Value.ToString("dd/MM/yyyy") : "N/A")} | Estado: {estado}");
                }
            }

            Console.WriteLine("--- RESERVAS ---");
            if (socio.reservas.Count == 0)
            {
                Console.WriteLine("Sin reservas en el historial.");
            }
            else
            {
                foreach (var r in socio.reservas)
                {
                    string estado = r.idEstadoReserva switch
                    {
                        1 => "Pendiente",
                        2 => "Cumplida",
                        3 => "Cancelada",
                        _ => "Desconocido"
                    };
                    Console.WriteLine($"- {r.libro?.titulo} (ISBN: {r.isbn}) | Fecha Reserva: {r.fechaReserva:dd/MM/yyyy} | Estado: {estado}");
                }
            }
        }
    }
}
