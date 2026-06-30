namespace Biblioteca_Pregot_Rubio.Models
{
    public class EstadoPrestamo
    {
        public int idEstadoPrestamo { get; set; }
        public string nombre { get; set; } = string.Empty;

        public List<Prestamo> prestamos { get; set; } = new();
    }
}