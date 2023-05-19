using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using MVCEXAMEN.Data;
using MVCEXAMEN.Models;


namespace MVCEXAMEN.Controllers
{
    public class AccesoController : Controller
    {
        private readonly DbCont _context;
        private readonly IHttpContextAccessor contxt;

        [ActivatorUtilitiesConstructor]
        public AccesoController(DbCont con, IHttpContextAccessor httpContextAccessor)
        {
            _context = con;
            contxt = httpContextAccessor;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(Usuarios usuarios,Empresas empresas)
        {
           var usuario= _context.sp_encontrarUsuario( usuarios.Email,usuarios.Contrasena);
            if (usuario !=null  && usuario.estado == 1) {
                var usuEmp=_context.sp_listarEmpUsu(usuario.Id);

                contxt.HttpContext.Session.SetString("Nombre", usuario.Nombre);
                contxt.HttpContext.Session.SetString("Apellido", usuario.Apellido);
                contxt.HttpContext.Session.SetInt32("Rol", usuario.rol);
                contxt.HttpContext.Session.SetString("Email", usuario.Email);
                if (usuEmp == null)
                {
                    contxt.HttpContext.Session.SetInt32("idEmpresa", 0);
                }
                else {
                    contxt.HttpContext.Session.SetInt32("idEmpresa", usuEmp.IdEmpresa);
                }
                
                contxt.HttpContext.Session.SetInt32("log", 1);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View();
            }

        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Acceso");
        }

    }
}
