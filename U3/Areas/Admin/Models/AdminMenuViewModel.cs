using U3.Models.Entities;
namespace U3.Areas.Admin.Models
{
    public class AdminMenuViewModel
    {
        public IEnumerable<Clasificacion> Clasifi { get; set; } = null!;
    }
}
