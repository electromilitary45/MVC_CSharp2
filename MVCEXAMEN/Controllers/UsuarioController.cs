using Microsoft.AspNetCore.Mvc;
using MVCEXAMEN.Data;
using MVCEXAMEN.Models;

namespace MVCEXAMEN.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly DbCont _context;
        public UsuarioController(DbCont con)
        {
            _context = con;
        }

        public IActionResult Index()
        {
            var usuarios = _context.sp_listarUsuarios();
            
            return View(usuarios);
        }

        //----CREAR-----
        public IActionResult Crear() {
            return View();
        }

        [HttpPost]
        public IActionResult Crear(Usuarios usuarios)
        {
            if (!ModelState.IsValid)
            {
                int rol = Convert.ToInt32(usuarios.rol);
                _context.sp_registroUsuario(usuarios.Nombre, usuarios.Apellido, usuarios.Email, usuarios.Contrasena,usuarios.rol);
                return RedirectToAction("Index");
            }
            return View();
        }

        //-------EDITAR------
        public IActionResult Editar(int id)
        {
            var usuario = _context.sp_listarUsuario(id);
            //UsuarioEmpresaViewModel viewModel = new UsuarioEmpresaViewModel
            //{
            //    Usuario = usuario,
            //    Empresa = empresa
            //};

            return View(usuario);

        }

        [HttpPost]
        public IActionResult Editar(Usuarios usuarios)
        {
            if (ModelState.IsValid)
            {
                int estado = Convert.ToInt32(usuarios.estado);
                _context.sp_actualizarUsuario(usuarios.Id, usuarios.Nombre, usuarios.Apellido, usuarios.Email, usuarios.Contrasena, usuarios.rol,estado);
                return RedirectToAction("Index");
            }
            return View();
        }

        //-------Desactivar---------
        [HttpPost]
        public IActionResult Desactivar(int id)
        {
            if (ModelState.IsValid)
            {
                _context.sp_desactivarUsuario(id);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Activar(int id)
        {
            if (ModelState.IsValid)
            {
                _context.sp_activarUsuario(id);
            }
            return RedirectToAction("Index");
        }

    }
}
