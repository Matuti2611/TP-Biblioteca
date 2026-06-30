namespace Biblioteca_Pregot_Rubio.Models
{
    public class EstadoReserva
    {
        public int idEstadoReserva { get; set; }
        public string nombre { get; set; } = string.Empty;

        public List<Reserva> reservas { get; set; } = new();
    }
}