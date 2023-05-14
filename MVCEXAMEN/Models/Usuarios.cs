using System.ComponentModel.DataAnnotations.Schema;

namespace MVCEXAMEN.Models
{
    public class Usuarios
    {
        public int Id { get; set; }
        public int rol { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string Contrasena { get; set; }
        public int estado { get; set; }

        public ICollection<EmpresasUsuarios> EmpresasUsuarios { get; set; } = new List<EmpresasUsuarios>();

    }
}
