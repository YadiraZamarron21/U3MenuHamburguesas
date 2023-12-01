using U3.Models.Entities;
namespace U3.Models.ViewModels
{
    public class MenuViewModel
    {
        public IEnumerable<Clasificacion> ClasificacionesL { get; set; } = null!;

        public int IdSeleccionado { get; set; }
        public string Descripcion { get; set; } = null!;

    }
}
