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

        public HomeController(ILogger<HomeController> logger, IUsuariosModel usuariosModel)
        {
            _logger = logger;
            _usuariosModel = usuariosModel;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Principal(UsuariosEntities entidad)
        {
            _usuariosModel.ValidarExisteUsuario(entidad);
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}