using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCEXAMEN.Data;
using MVCEXAMEN.Models;

namespace MVCEXAMEN.Controllers
{
    
    public class EmpresaUsuarioController : Controller
    {
        private readonly DbCont _context;

        public EmpresaUsuarioController(DbCont context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var empresaUsuarios = _context.sp_listarEmpsUsus();
            return View(empresaUsuarios);
        }

        public IActionResult Crear()
        {
            var viewModel = new EmpUsuView
            {
                EmpresasUsuarios = _context.sp_listarEmpsUsus(),
                Empresas = _context.sp_listarEmpresas(),
                Usuarios = _context.sp_ListarUsuariosSinEmpresa()
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Crear(EmpresasUsuarios empresasUsuarios)
        {
            if (!ModelState.IsValid)
            {
                int idUsuario = Convert.ToInt32(empresasUsuarios.IdUsuario);
                int idEmpresa = Convert.ToInt32(empresasUsuarios.IdEmpresa);
                _context.sp_insertarEmpUsu(empresasUsuarios.IdUsuario, empresasUsuarios.IdEmpresa);
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Eliminar(int id) {
            if (ModelState.IsValid) {
                _context.sp_eliminarEmpUsu(id); 
            }
            return RedirectToAction("Index");
        }


    }
}
