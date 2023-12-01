namespace U3.Areas.Admin.Models
{
    public class AdminPromocionesViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public double Precio { get; set; }
        public double? Promocion { get; set; }

    }
}
