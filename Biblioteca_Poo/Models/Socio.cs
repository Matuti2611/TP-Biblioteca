namespace Biblioteca_Poo.Models
{
    public class Socio
    {
        public int NroSocio { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public int IdTipoSocio { get; set; }
        public bool Activo { get; set; }

        public TipoSocio? TipoSocio { get; set; }

        public List<Prestamo> Prestamos { get; set; } = new();
        public List<Reserva> Reservas { get; set; } = new();
        public List<Multa> Multas { get; set; } = new();
    }
}