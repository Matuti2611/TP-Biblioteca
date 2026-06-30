namespace Biblioteca_Pregot_Rubio.Models
{
    public class Reserva
    {
        public int idReserva { get; set; }

        public int nroSocio { get; set; }
        public string isbn { get; set; } = string.Empty;

        public DateTime fechaReserva { get; set; }

        public int idEstadoReserva { get; set; }

        public Socio? socio { get; set; }
        public Libro? libro { get; set; }
        public EstadoReserva? estadoReserva { get; set; }
    }
}