using System;

namespace Biblioteca_Pregot_Rubio
{
    public class BibliotecaUI
    {
        private readonly GestorBiblioteca _gestor;

        public BibliotecaUI(GestorBiblioteca gestor)
        {
            _gestor = gestor;
        }

        public void iniciar()
        {
            mostrarLibrosDisponibles();

            bool salir = false;
            while (!salir)
            {
                Console.WriteLine("=== SISTEMA DE GESTION DE BIBLIOTECA ===");
                Console.WriteLine("1. Registrar Prestamo");
                Console.WriteLine("2. Registrar Devolucion");
                Console.WriteLine("3. Registrar Reserva");
                Console.WriteLine("4. Consultar Detalle de Socio");
                Console.WriteLine("5. Reportes");
                Console.WriteLine("6. Salir");
                Console.Write("Seleccione una opcion: ");
                
                string? opcion = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(opcion)) continue;
                Console.WriteLine();

                switch (opcion)
                {
                    case "1":
                        PrestamoFlujo.ejecutar(_gestor);
                        break;
                    case "2":
                        DevolucionFlujo.ejecutar(_gestor);
                        break;
                    case "3":
                        ReservaFlujo.ejecutar(_gestor);
                        break;
                    case "4":
                        SocioFlujo.ejecutar(_gestor);
                        break;
                    case "5":
                        ReportesFlujo.ejecutar(_gestor);
                        break;
                    case "6":
                        salir = true;
                        break;
                    default:
                        Console.WriteLine("Opcion invalida.");
                        break;
                }
                Console.WriteLine();
            }
        }

        private void mostrarLibrosDisponibles()
        {
            Console.WriteLine("=== LIBROS DISPONIBLES ===");
            var libros = _gestor.obtenerLibrosDisponibles();
            foreach (var libro in libros)
            {
                int disponibles = _gestor.obtenerCopiasDisponibles(libro);
                Console.WriteLine($"{libro.titulo} - {libro.autor} | Disponibles: {disponibles}");
            }
            Console.WriteLine();
        }
    }
}
