using U3.Models.Entities;
namespace U3.Areas.Admin.Models
{
    public class AdminAgregarProductoViewModel
    {
        public Menu menuHamburguesas { get; set; } = new();
        public IEnumerable<Clasificacion> Clasificaciones { get; set; } = null!;
        public IFormFile? Archivo { get; set; }

    }
}
