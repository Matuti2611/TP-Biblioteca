namespace Biblioteca_Poo.Models
{
    public class EstadoReserva
    {
        public int IdEstadoReserva { get; set; }
        public string Nombre { get; set; } = string.Empty;

        public List<Reserva> Reservas { get; set; } = new();
    }
}