namespace Biblioteca_Pregot_Rubio.Models
{
    public class TipoSocio
    {
        public int idTipoSocio { get; set; }
        public string nombre { get; set; } = string.Empty;
        public int maxLibros { get; set; }
        public int diasPrestamo { get; set; }
        public decimal multaPorDia { get; set; }

        public List<Socio> socios { get; set; } = new();
    }
}