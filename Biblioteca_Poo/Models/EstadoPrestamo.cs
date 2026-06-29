namespace Biblioteca_Poo.Models
{
    public class EstadoPrestamo
    {
        public int IdEstadoPrestamo { get; set; }
        public string Nombre { get; set; } = string.Empty;

        public List<Prestamo> Prestamos { get; set; } = new();
    }
}