namespace Biblioteca_Poo.Models
{
    public class Libro
    {
        public string ISBN { get; set; } = string.Empty;
        public string Titulo { get; set; } = string.Empty;
        public string Autor { get; set; } = string.Empty;
        public string Genero { get; set; } = string.Empty;
        public int CantidadCopias { get; set; }

        public List<Prestamo> Prestamos { get; set; } = new();
        public List<Reserva> Reservas { get; set; } = new();
    }
}