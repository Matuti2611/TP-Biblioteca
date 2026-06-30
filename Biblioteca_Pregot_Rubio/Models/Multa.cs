namespace Biblioteca_Pregot_Rubio.Models
{
    public class Multa
    {
        public int idMulta { get; set; }

        public int idPrestamo { get; set; }
        public int nroSocio { get; set; }

        public decimal monto { get; set; }
        public bool pagada { get; set; }
        public DateTime fechaGeneracion { get; set; }

        public Prestamo? prestamo { get; set; }
        public Socio? socio { get; set; }
    }
}