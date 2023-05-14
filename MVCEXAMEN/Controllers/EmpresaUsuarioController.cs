using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCEXAMEN.Data;

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


    }
}
