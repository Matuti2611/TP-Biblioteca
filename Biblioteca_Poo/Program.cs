using Biblioteca_Poo.Data;
using Microsoft.EntityFrameworkCore;

using var context = new BibliotecaContext();

Console.WriteLine("=== LIBROS DISPONIBLES ===");

var libros = context.Libros
    .Include(l => l.Prestamos)
    .ToList();

foreach (var libro in libros)
{
    int prestamosActivos = libro.Prestamos
        .Count(p => p.FechaDevolucion == null);

    int disponibles = libro.CantidadCopias - prestamosActivos;

    Console.WriteLine($"{libro.Titulo} - {libro.Autor} | Disponibles: {disponibles}");
}

Console.WriteLine();
Console.WriteLine("Presione una tecla para salir...");
Console.ReadKey();