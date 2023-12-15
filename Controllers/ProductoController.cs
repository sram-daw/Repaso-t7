using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repaso_t7.Models;
using Repaso_t7.Models.Repository;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Repaso_t7.Controllers
{
    public class ProductoController : Controller
    {
        private readonly IProductoRepository _productoRepository;
        public ProductoController(IProductoRepository producto)
        {
            _productoRepository = producto;
        }

        [HttpGet]
        [Route("Productos")]
        public async Task<IActionResult> Index()
        {
            //deserializacion de booleano para comprobar inicio de sesion
            byte[] isLoggedBytes = HttpContext.Session.Get("isLogged");
            bool isLogged = isLoggedBytes != null && BitConverter.ToBoolean(isLoggedBytes);


            if (isLogged)
            {
                //deserializacion de objeto de variable de sesion
                byte[] usuarioBytes = HttpContext.Session.Get("User");
                string usuarioString = Encoding.UTF8.GetString(usuarioBytes);
                Usuario user = JsonSerializer.Deserialize<Usuario>(usuarioString);
                if (!user.isadmin)
                {
                    var productos = await _productoRepository.GetAllProductos();
                    return View(productos);
                }
                else
                {
                    return RedirectToAction("AdminIndex");
                }

            }
            else
            {
                TempData["ErrorMessage"] = "Debes iniciar sesión para ver la lista de productos.";
                return View("Login");
            }

        }

        [HttpGet]
        public async Task<IActionResult> AdminIndex()
        {
            var usuarios = await _productoRepository.GetAllUsers();
            return View(usuarios);
        }

        [HttpGet]
        [Route("Crear")]
        public async Task<IActionResult> Crear()
        {
            return View();
        }

        [HttpGet]
        [Route("Detalles/{id}")]
        public async Task<IActionResult> Detalles(int id)
        {
            Producto producto = await _productoRepository.GetProductoById(id);
            return View(producto);
        }

        [HttpPost]
        public async Task<IActionResult> Guardar(string nombre, decimal precio)
        {
            bool isGuardarOk = await _productoRepository.GuardarProducto(nombre, precio);
            if (!isGuardarOk)
            {
                TempData["ErrorMessage"] = "Ha habido un error al guardar el producto. Vuelve a intentarlo.";

            }
            else
            {
                TempData["OkMessage"] = "El producto se ha guardado correctamente.";
            }
            return View("Crear");
        }

        [HttpGet]
        [Route("Editar/{id}")]
        public async Task<IActionResult> Editar(int id)
        {
            Producto producto = await _productoRepository.GetProductoById(id);
            if (producto != null)
            {
                return View(producto);
            }
            else
            {
                TempData["ErrorMessage"] = "No hay ningún producto con ese ID";
                return View("Index");
            }

        }

        [HttpPost]
        public async Task<IActionResult> Actualizar(int id, string nombre, decimal precio)
        {
            bool isEditarOk = await _productoRepository.UpdateProducto(id, nombre, precio);
            Producto producto = await _productoRepository.GetProductoById(id);
            if (isEditarOk)
            {
                TempData["OkMessage"] = "Se ha editado correctamente el producto.";

                return View("Editar", producto);
            }
            else
            {
                TempData["ErrorMessage"] = "No se ha podido editar el producto. Inténtalo de nuevo.";
                return View("Editar", producto);
            }
        }

        [HttpGet]
        [Route("Borrar/{id}")]
        public async Task<IActionResult> Borrar(int id)
        {
            bool isBorrarOk = await _productoRepository.DeleteProducto(id);
            var productos = await _productoRepository.GetAllProductos();
            if (isBorrarOk)
            {
                TempData["OkMessage"] = "Se ha eliminado correctamente el producto.";
                return View("Index", productos);
            }
            else
            {
                TempData["ErrorMessage"] = "No ha podido eliminarse el producto. Es posible que el ID sea incorrecto";
                return View("Index", productos);
            }

        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CheckLogin(string usuario, string contraseña)
        {
            var productos = await _productoRepository.GetAllProductos();
            bool isLoginOk = await _productoRepository.CheckLogin(usuario, contraseña);
            Usuario user = await _productoRepository.GetUser(usuario, contraseña);
            if (isLoginOk)
            {
                //se guarda en la variable de sesión un booleano a true para indicar que se inició la sesión
                HttpContext.Session.Set("isLogged", BitConverter.GetBytes(true));

                //serializacion de objeto para almacenarlo en variable de sesion
                string usuarioString = JsonSerializer.Serialize(user);
                HttpContext.Session.Set("User", Encoding.UTF8.GetBytes(usuarioString));
                return RedirectToAction("Index");
            }
            else
            {
                TempData["ErrorMessage"] = "Las credenciales introducidas son incorrectas.";
                HttpContext.Session.Set("isLogged", BitConverter.GetBytes(false));
                return View("Login");
            }


        }

        //hacer un sistema de login con variable de sesion. Capturas para apuntes

    }
}
