using Microsoft.AspNetCore.Mvc;
using MVCEXAMEN.Data;
using MVCEXAMEN.Models;

namespace MVCEXAMEN.Controllers
{
    public class EmpresaController : Controller
    {
        private readonly DbCont _context;
        public EmpresaController(DbCont con)
        {
            _context = con;
        }
        public IActionResult Index()
        {
            var empresas = _context.sp_listarEmpresas();
            return View(empresas);
        }

        //----CREAR----
        public IActionResult Crear()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Crear(Empresas emp)
        {
            if (ModelState.IsValid)
            {
                _context.sp_insertarEmpresa(emp.Nombre, emp.Telefono, emp.Ubicacion, emp.Email);
                return RedirectToAction("Index");
            }
            return View();
        }

        //----EDITAR----
        public IActionResult Editar(int id)
        {
            var empresa = _context.sp_listarEmpresa(id);
            return View(empresa);
        }
        [HttpPost]
        public IActionResult Editar(Empresas emp)
        {
            if (ModelState.IsValid)
            {
                _context.sp_actualizarEmpresa(emp.Id, emp.Nombre, emp.Telefono, emp.Ubicacion, emp.Email, emp.Estado);
                return RedirectToAction("Index");
            }
            return View();
        }

        //---ACTIVAR
        [HttpPost]
        public IActionResult Activar(int id)
        {
            _context.sp_activarEmpresa(id);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult Desactivar(int id)
        {
            _context.sp_desactivarEmpresa(id);
            return RedirectToAction("Index");
        }



    }
}
