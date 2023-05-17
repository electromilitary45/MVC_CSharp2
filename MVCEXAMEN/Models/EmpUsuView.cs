namespace MVCEXAMEN.Models
{
    public class EmpUsuView
    {
        public IEnumerable<EmpresasUsuarios> EmpresasUsuarios { get; set; }
        public IEnumerable<Empresas> Empresas { get; set; }
        public IEnumerable<Usuarios> Usuarios { get; set; }
    }
}
