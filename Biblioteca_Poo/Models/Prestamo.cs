namespace Biblioteca_Poo.Models
{
    public class Prestamo
    {
        public int IdPrestamo { get; set; }

        public int NroSocio { get; set; }
        public string ISBN { get; set; } = string.Empty;

        public DateTime FechaPrestamo { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public DateTime? FechaDevolucion { get; set; }

        public int IdEstadoPrestamo { get; set; }
        public bool Renovado { get; set; }

        public Socio? Socio { get; set; }
        public Libro? Libro { get; set; }
        public EstadoPrestamo? EstadoPrestamo { get; set; }

        public List<Multa> Multas { get; set; } = new();
    }
}