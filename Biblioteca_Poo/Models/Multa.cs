namespace Biblioteca_Poo.Models
{
    public class Multa
    {
        public int IdMulta { get; set; }

        public int IdPrestamo { get; set; }
        public int NroSocio { get; set; }

        public decimal Monto { get; set; }
        public bool Pagada { get; set; }
        public DateTime FechaGeneracion { get; set; }

        public Prestamo? Prestamo { get; set; }
        public Socio? Socio { get; set; }
    }
}