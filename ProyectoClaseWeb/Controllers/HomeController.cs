using Microsoft.AspNetCore.Mvc;
using ProyectoClaseWeb.Entities;
using ProyectoClaseWeb.Interfaces;
using ProyectoClaseWeb.Models;
using System.Diagnostics;

namespace ProyectoClaseWeb.Controllers
{
    public class HomeController : Controller
    {
        //en vez de hacer una instancia lo que realizo es la inyeccion de dependencia
        private readonly ILogger<HomeController> _logger;
        private readonly IUsuariosModel _usuariosModel;
        private readonly IBitacorasModel _bitacorasModel;

        public HomeController(ILogger<HomeController> logger, IUsuariosModel usuariosModel, IBitacorasModel bitacorasModel)
        {
            _logger = logger;
            _usuariosModel = usuariosModel;
            _bitacorasModel = bitacorasModel;
        }

        [HttpGet]
        public IActionResult Index()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                RegistrarBitacora(ex, ControllerContext);
                ViewBag.mensaje = "Se presento un inconveniente";
                return View();
            }
        }

        [HttpPost]
        public IActionResult Principal(UsuariosEntities entidad)
        {
            try
            {
                //int num1 = 10;int num2 = 0;int num3 = num1/num2; //esto lo tuve para me diera un error 

                var resultado = _usuariosModel.ValidarCredenciales(entidad);

                if (resultado != null)
                {
                    HttpContext.Session.SetString("Correo", resultado.CorreoElectronico);//capturamos la variable de sesion.. (variable de sesion es un variable en el servidor el cual guarda un dato que puedo usarlo cuando quiera y donde quiera)
                    return View();
                }
                else
                {
                    ViewBag.mensaje = "Valide sus credenciales por favor";
                    return View("Index");
                }
            }
            catch (Exception ex)
            {
                RegistrarBitacora(ex, ControllerContext);
                ViewBag.mensaje = "Se presento un inconveniente";
                return View("Index");
            }
        }

        [HttpGet]
        public IActionResult RegistrarUsuario()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                RegistrarBitacora(ex, ControllerContext);
                ViewBag.mensaje = "Se presento un inconveniente";
                return View("Index");
            }
        }

        [HttpPost]
        public IActionResult RegistrarUsuario(UsuariosEntities entidad)
        {
            try
            {
                _usuariosModel.RegistrarUsuario(entidad);
                return View("Index");
            }
            catch (Exception ex)
            {
                RegistrarBitacora(ex, ControllerContext);
                ViewBag.mensaje = "Se presento un inconveniente";
                return View("Index");
            }
        }

        [HttpGet]
        public IActionResult BuscarCorreo(string CorreoElectronico)
        {
            return Json(_usuariosModel.BuscarExisteCorreo(CorreoElectronico));
        }

        [HttpGet]
        public IActionResult RecuperarContrasena()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RecuperarContrasena(UsuariosEntities entidad) {
            var resultado = _usuariosModel.VerificacionCorreo(entidad);
            if (resultado == null)
            {
                ViewBag.faltaCuenta = "Usted no posee una cuenta, favor registrarse";
                return View();
            }
            else
            {
                if (resultado.Estado)
                {
                    _usuariosModel.Email(entidad.CorreoElectronico);
                    ViewBag.mensaje = "Su contraseña se envio al correo ingresado";
                    return View();
                }
                else
                {
                    ViewBag.inactivo = "Su cuenta se encuentra inactiva.";
                    return View();
                }
            }
        }
        private void RegistrarBitacora(Exception ex, ControllerContext contexto)
        {
            ErrorBitacoraEntities error = new ErrorBitacoraEntities();
            error.Origen = contexto.ActionDescriptor.ControllerName + "-" + contexto.ActionDescriptor.ActionName;//da el nombre del controlador y la accion del controlador (se le llama reflection cuando hacemos uso del nombre las cosas que tiene Visual ala hora de usar una libreria que visual tiene por dentro la cual se llama reflection)
            error.Detalle = ex.Message;
            _bitacorasModel.RegistrarBitacora(error);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}