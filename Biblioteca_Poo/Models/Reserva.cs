namespace Biblioteca_Poo.Models
{
    public class Reserva
    {
        public int IdReserva { get; set; }

        public int NroSocio { get; set; }
        public string ISBN { get; set; } = string.Empty;

        public DateTime FechaReserva { get; set; }

        public int IdEstadoReserva { get; set; }

        public Socio? Socio { get; set; }
        public Libro? Libro { get; set; }
        public EstadoReserva? EstadoReserva { get; set; }
    }
}