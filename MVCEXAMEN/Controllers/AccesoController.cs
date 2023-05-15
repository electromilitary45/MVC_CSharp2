using Microsoft.AspNetCore.Mvc;

namespace MVCEXAMEN.Controllers
{
    public class AccesoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
