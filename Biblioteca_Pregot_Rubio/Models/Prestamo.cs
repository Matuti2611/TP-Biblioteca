namespace Biblioteca_Pregot_Rubio.Models
{
    public class Prestamo
    {
        public int idPrestamo { get; set; }

        public int nroSocio { get; set; }
        public string isbn { get; set; } = string.Empty;

        public DateTime fechaPrestamo { get; set; }
        public DateTime fechaVencimiento { get; set; }
        public DateTime? fechaDevolucion { get; set; }

        public int idEstadoPrestamo { get; set; }
        public bool renovado { get; set; }

        public Socio? socio { get; set; }
        public Libro? libro { get; set; }
        public EstadoPrestamo? estadoPrestamo { get; set; }

        public List<Multa> multas { get; set; } = new();
    }
}