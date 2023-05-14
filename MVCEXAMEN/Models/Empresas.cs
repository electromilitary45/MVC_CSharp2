namespace MVCEXAMEN.Models
{
    public class Empresas
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Telefono { get; set; }
        public string Ubicacion  { get; set; }
        public string Email { get; set; }
        public int Estado { get; set; }

        public ICollection<EmpresasUsuarios> EmpresasUsuarios { get; set; } = new List<EmpresasUsuarios>();
    }
}
