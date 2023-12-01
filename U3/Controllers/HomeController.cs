using Microsoft.AspNetCore.Mvc;
using U3.Models.Entities;
using U3.Models.ViewModels;
using U3.Repositories;

namespace U3.Controllers
{
    public class HomeController : Controller
    {
        private readonly ClasificacionRepository clasifificacionRepository;

        public MenuRepository menuRepository { get; }

        public HomeController(ClasificacionRepository clasifcacionRepos, MenuRepository menuRepos)
        {
            this.clasifificacionRepository = clasifcacionRepos;
            this.menuRepository = menuRepos;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Menu(string? id)
        {
            MenuViewModel vm = new();

            vm.ClasificacionesL = clasifificacionRepository.GetAll().OrderBy(x => x.Nombre);

            if (id != null)
            {

                id = id.Replace("-", " ");

                var prod = menuRepository.GetByNombre(id);

                vm.IdSeleccionado = prod.Id;
                vm.Descripcion = prod.Descripción;
            }

            return View(vm);
        }

        [HttpGet]
        public IActionResult Promociones(PromocionesViewModel vm, int id)
        {
            vm.IndexI = id;

            var lista = menuRepository.GetAll().Where(x => x.PrecioPromocion > 0);
            vm.IndexF = lista.Count();

            //Buscamos objeto en la base de datos
            var prod = lista.ToList()[vm.IndexI];
            if (prod != null)
            {
                vm.Nombre = prod.Nombre;
                vm.Descripcion = prod.Descripción;
                vm.Id = prod.Id;
                vm.Precio = prod.Precio > 0 ? (decimal)prod.Precio : 0;
                vm.Promocion = prod.PrecioPromocion > 0 ? (decimal)prod.PrecioPromocion : 0;
            }

            return View(vm);
        }
    }
}
