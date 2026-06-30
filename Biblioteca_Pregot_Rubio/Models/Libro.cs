namespace Biblioteca_Pregot_Rubio.Models
{
    public class Libro
    {
        public string isbn { get; set; } = string.Empty;
        public string titulo { get; set; } = string.Empty;
        public string autor { get; set; } = string.Empty;
        public string genero { get; set; } = string.Empty;
        public int cantidadCopias { get; set; }

        public List<Prestamo> prestamos { get; set; } = new();
        public List<Reserva> reservas { get; set; } = new();
    }
}