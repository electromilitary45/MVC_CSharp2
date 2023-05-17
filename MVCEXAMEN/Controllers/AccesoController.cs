using Microsoft.AspNetCore.Mvc;
using MVCEXAMEN.Data;
using MVCEXAMEN.Models;

namespace MVCEXAMEN.Controllers
{
    public class AccesoController : Controller
    {
        private readonly DbCont _context;
        public AccesoController(DbCont con)
        {
            _context = con;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(Usuarios usuarios)
        {
           var usuario= _context.sp_encontrarUsuario( usuarios.Email,usuarios.Contrasena);
            if (usuario != null) {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View();
            } 
        }

    }
}
