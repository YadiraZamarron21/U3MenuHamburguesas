using Microsoft.AspNetCore.Mvc;
using U3.Areas.Admin.Models;
using U3.Models.Entities;
using U3.Repositories;

namespace U3.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MenuController : Controller
    {
        private readonly ClasificacionRepository clasificacionesRepository;
        private MenuRepository menuRepository;

        public MenuController(ClasificacionRepository ClasifRepository, MenuRepository menuRepos)
        {
            this.clasificacionesRepository = ClasifRepository;
            this.menuRepository = menuRepos;
        }

        [HttpGet]
        [HttpPost]

        public IActionResult Index(AdminMenuViewModel vm)
        {

            vm.Clasifi = clasificacionesRepository.GetAll().OrderBy(x => x.Nombre);

            return View(vm);
        }

        [HttpGet]
        public IActionResult Agregar()
        {
            AdminAgregarProductoViewModel vm = new();
            vm.Clasificaciones = clasificacionesRepository.GetAll().OrderBy(x => x.Nombre);
            return View(vm);
        }

        [HttpPost]
        public IActionResult Agregar(AdminAgregarProductoViewModel vm)
        {
            ModelState.Clear();
            if (string.IsNullOrWhiteSpace(vm.menuHamburguesas.Nombre))
            {
                ModelState.AddModelError("", "Ingrese el nombre.");
            }
            if (vm.menuHamburguesas.Precio == null || vm.menuHamburguesas.Precio <= 0)
            {
                ModelState.AddModelError("", "Ingrese el precio.");
            }
            if (string.IsNullOrWhiteSpace(vm.menuHamburguesas.Descripción))
            {
                ModelState.AddModelError("", "Ingrese la descripción.");
            }

            if (vm.Archivo != null)
            {
                if (vm.Archivo.ContentType != "image/png")
                {
                    ModelState.AddModelError("", "solo se permiten imagenes png.");
                }

                if (vm.Archivo.Length > 500 * 1024)//500kb
                {
                    ModelState.AddModelError("", "solo se permiten archivos no mayores a 500kb");

                }
            }

            if (ModelState.IsValid)
            {
                menuRepository.Insert(vm.menuHamburguesas);

                if (vm.Archivo == null)
                {
                    System.IO.File.Copy("wwwroot/images/burger.png", $"wwwroot/hamburguesas/{vm.menuHamburguesas.Id}.png");
                }
                else
                {
                    System.IO.FileStream fs = System.IO.File.Create($"wwwroot/hamburguesas/{vm.menuHamburguesas.Id}.png");
                    vm.Archivo.CopyTo(fs);
                    fs.Close();
                }
                return RedirectToAction("Index");
            }

            vm.Clasificaciones = clasificacionesRepository.GetAll().OrderBy(x => x.Nombre);
            return View(vm);
        }



        [HttpGet]
        public IActionResult Editar(int id)
        {
            var prod = menuRepository.Get(id);

            if (prod == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                AdminAgregarProductoViewModel vm = new();
                vm.menuHamburguesas = prod;
                vm.Clasificaciones = clasificacionesRepository.GetAll().OrderBy(x => x.Nombre);
                return View(vm);

            }
        }


        [HttpPost]
        public IActionResult Editar(AdminAgregarProductoViewModel vm)
        {

            ModelState.Clear();

            if (vm.Archivo != null) //Si selecciono un archivo
            {
                if (vm.Archivo.ContentType != "image/png")
                {
                    ModelState.AddModelError("", "Solo se permiten imagenes PNG.");
                }

                if (vm.Archivo.Length > 500 * 1024)//500kb
                {
                    ModelState.AddModelError("", "Solo se permiten archivos no mayores a 500Kb");

                }
            }

            if (ModelState.IsValid)
            {
                var prod = menuRepository.Get(vm.menuHamburguesas.Id);
                if (prod == null)
                {
                    return RedirectToAction("Index");
                }

                prod.Nombre = vm.menuHamburguesas.Nombre;
                prod.Precio = vm.menuHamburguesas.Precio;
                prod.Descripción = vm.menuHamburguesas.Descripción;
                prod.IdClasificacion = vm.menuHamburguesas.IdClasificacion;

                menuRepository.Update(prod);

                //Editar la foto
                if (vm.Archivo != null)
                {
                    System.IO.FileStream fs = System.IO.File.Create($"wwwroot/hamburguesas/{vm.menuHamburguesas.Id}.png");
                    vm.Archivo.CopyTo(fs);
                    fs.Close();
                }
                return RedirectToAction("Index");
            }

            vm.Clasificaciones = clasificacionesRepository.GetAll().OrderBy(x => x.Nombre);

            return View(vm);
        }

        [HttpGet]
        public IActionResult Eliminar(int id)
        {
            var producto = menuRepository.Get(id);

            if (producto == null)
            {
                return RedirectToAction("Index");
            }

            return View(producto);
        }

        [HttpPost]
        public IActionResult Eliminar(Menu menu)
        {
            var producto = menuRepository.Get(menu.Id);
            if (producto == null)
            {
                return RedirectToAction("Index");
            }

            menuRepository.Delete(producto);

            var ruta = $"wwwroot/hamburguesas/{menu.Id}.png";

            if (System.IO.File.Exists(ruta))
            {
                System.IO.File.Delete(ruta);
            }

            return RedirectToAction("Index");
        }




        [HttpGet]
        public IActionResult AgregarPromocion(int id)
        {
            var hamburguesa = menuRepository.Get(id);

            if (hamburguesa == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                AdminPromocionesViewModel vm = new();

                vm.Id = hamburguesa.Id;
                vm.Nombre = hamburguesa.Nombre;
                vm.Precio = hamburguesa.Precio;
                vm.Promocion = hamburguesa.PrecioPromocion;

                return View(vm);
            }
        }

        [HttpPost]
        public IActionResult AgregarPromocion(AdminPromocionesViewModel vm)
        {
            if (vm.Promocion < 0)
            {
                ModelState.AddModelError("", "Ingrese el precio de promoción.");
            }
            if (vm.Promocion > vm.Precio)
            {
                ModelState.AddModelError("", "El precio de promoción debe ser menor al precio actual.");
            }

            if (ModelState.IsValid)
            {
                var product = menuRepository.Get(vm.Id);
                if (product == null)
                {
                    return RedirectToAction("Index");
                }

                product.PrecioPromocion = vm.Promocion;

                menuRepository.Update(product);

                return RedirectToAction("Index");

            }
            return View(vm);
        }

        [HttpGet]
        public IActionResult EliminarPromocion(int id)
        {
            var prod = menuRepository.Get(id);
            if (prod == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                AdminPromocionesViewModel vm = new();
                vm.Id = prod.Id;
                vm.Nombre = prod.Nombre;
                vm.Precio = prod.Precio;
                vm.Promocion = prod.PrecioPromocion;

                return View(vm);
            }
        }

        [HttpPost]
        public IActionResult EliminarPromocion(AdminPromocionesViewModel vm)
        {

            if (vm != null && (vm.Promocion != null || vm.Promocion != 0))
            {
                var producto = menuRepository.Get(vm.Id);
                if (producto == null)
                {
                    return RedirectToAction("Index");
                }

                producto.PrecioPromocion = null;
                vm.Promocion = producto.PrecioPromocion;

                menuRepository.Update(producto);

                return RedirectToAction("Index");
            }

            return View(vm);
        }
    }
}
