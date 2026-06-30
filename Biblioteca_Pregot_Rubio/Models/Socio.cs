namespace Biblioteca_Pregot_Rubio.Models
{
    public class Socio
    {
        public int nroSocio { get; set; }
        public string nombre { get; set; } = string.Empty;
        public string apellido { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;

        public int idTipoSocio { get; set; }
        public bool activo { get; set; }

        public TipoSocio? tipoSocio { get; set; }

        public List<Prestamo> prestamos { get; set; } = new();
        public List<Reserva> reservas { get; set; } = new();
        public List<Multa> multas { get; set; } = new();
    }
}