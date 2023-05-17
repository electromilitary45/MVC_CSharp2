namespace MVCEXAMEN.Models
{
    public class EmpresasUsuarios
    {
      

        public int Id { get; set; }
        public int IdEmpresa { get; set; }
        public int IdUsuario { get; set; }

        public Empresas Empresas { get; set; }
        public Usuarios Usuarios { get; set; }
    }

}
