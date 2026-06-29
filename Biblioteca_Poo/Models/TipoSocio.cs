namespace Biblioteca_Poo.Models
{
	public class TipoSocio
	{
		public int IdTipoSocio { get; set; }
		public string Nombre { get; set; } = string.Empty;
		public int MaxLibros { get; set; }
		public int DiasPrestamo { get; set; }
		public decimal MultaPorDia { get; set; }

		public List<Socio> Socios { get; set; } = new();
	}
}