using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MVCEXAMEN.Models;

namespace MVCEXAMEN.Data
{
    public class DbCont:DbContext
    {
        //Constructor para que detecte la conexion a la bd
        public DbCont(DbContextOptions<DbCont> options):base(options) { 
            
        }

        public DbSet<Usuarios> Usuarios { get; set; }
        public DbSet<Empresas> Empresas { get; set; }

        public DbSet<EmpresasUsuarios> EmpresasUsuarios { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Empresas>(entity =>
            {
                entity.ToTable("empresas");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Nombre).HasColumnName("nombre").HasMaxLength(50).IsRequired();
                entity.Property(e => e.Telefono).HasColumnName("telefono").HasMaxLength(50).IsRequired();
                entity.Property(e => e.Ubicacion).HasColumnName("ubicacion").HasMaxLength(50).IsRequired();
                entity.Property(e => e.Email).HasColumnName("email").HasMaxLength(256).IsRequired();
                entity.Property(e => e.Estado).HasColumnName("estado").IsRequired();
            });

            modelBuilder.Entity<Usuarios>(entity =>
            {
                entity.ToTable("usuarios");

                entity.Property(u => u.Id).HasColumnName("id");
                entity.Property(u => u.rol).HasColumnName("rol").IsRequired();
                entity.Property(u => u.Nombre).HasColumnName("nombre").HasMaxLength(50).IsRequired();
                entity.Property(u => u.Apellido).HasColumnName("apellido").HasMaxLength(50).IsRequired();
                entity.Property(u => u.Email).HasColumnName("email").HasMaxLength(50).IsRequired();
                entity.Property(u => u.Contrasena).HasColumnName("contrasena").HasMaxLength(256).IsRequired();
                entity.Property(u => u.estado).HasColumnName("estado").IsRequired();


            });

            modelBuilder.Entity<EmpresasUsuarios>(entity =>
            {
                entity.ToTable("empresasUsuarios");
                entity.HasKey(eu => eu.Id);
                entity.Property(eu => eu.Id).HasColumnName("id");
                entity.Property(eu => eu.IdEmpresa).HasColumnName("idEmpresa").IsRequired();
                entity.Property(eu => eu.IdUsuario).HasColumnName("idUsuario").IsRequired();

                entity.HasOne(eu => eu.Empresas)
                .WithMany() // Especifica el nombre de la propiedad de navegación en la entidad Empresas si existe
                .HasForeignKey(eu => eu.IdEmpresa);

                entity.HasOne(eu => eu.Usuarios)
                .WithMany() // Especifica el nombre de la propiedad de navegación en la entidad Usuarios si existe
                .HasForeignKey(eu => eu.IdUsuario);
            });

            modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UsuariosClaims");
            modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("UsuariosLogins");
            modelBuilder.Entity<IdentityUserToken<string>>().ToTable("UsuariosTokens");
            modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UsuariosRoles");
            modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("RolesClaims");
            modelBuilder.Entity<IdentityRole>().ToTable("Roles");
        }

        ////----METODOS PARA LOS PROCEDIMIENTOS ALMACENADOS DE USUARIOS----
        public List<Usuarios> sp_listarUsuarios()
        {
            return Usuarios.FromSqlRaw("exec sp_listarUsuarios").ToList();
        }
        public Usuarios sp_listarUsuario(int id)
        {
            //explica el codigo
            var usuario = Usuarios.FromSqlRaw("exec sp_listarUsuario {0}", id).ToList(); 
            return usuario[0];
        }

        public void sp_registroUsuario(string nombre, string apellidos, string email, string contrasena, int rol)
        {
            Database.ExecuteSqlRaw($"exec sp_registroUsuario '{nombre}','{apellidos}','{email}','{contrasena}',{rol}");
        }

        public void sp_actualizarUsuario(int id, string nombre, string apellidos, string email, string contrasena, int rol, int estado)
        {
            
            Database.ExecuteSqlRaw($"exec sp_actualizarUsuario {id},'{nombre}','{apellidos}','{email}','{contrasena}',{estado},{rol}");
        }

        public void sp_desactivarUsuario(int id) 
        { 
            Database.ExecuteSqlRaw($"exec sp_desactivarUsuario {id}");
        }

        public void sp_activarUsuario(int id)
        {
            Database.ExecuteSqlRaw($"exec sp_activarUsuario {id}");
        }

        ///METODOS PARA LOS PROCEDIMIENTOS ALMACENADOS DE EMPRESAS
        public List<Empresas> sp_listarEmpresas()
        {
            return Empresas.FromSqlRaw("exec sp_listarEmpresas").ToList();
        }

        public Empresas sp_listarEmpresa(int id)
        {
            var empresa = Empresas.FromSqlRaw("exec sp_listarEmpresa {0}", id).ToList();
            return empresa[0];
        }

        public void sp_insertarEmpresa(string nombre, string telefono, string ubicacion, string email)
        {
            Database.ExecuteSqlRaw($"exec sp_insertarEmpresa '{nombre}','{telefono}','{ubicacion}','{email}'");
        }

        public void sp_actualizarEmpresa(int id, string nombre, string telefono, string ubicacion, string email, int estado)
        {
            Database.ExecuteSqlRaw($"exec sp_actualizarEmpresa {id},'{nombre}','{telefono}','{ubicacion}','{email}',{estado}");
        }

        public void sp_desactivarEmpresa(int id)
        {
            Database.ExecuteSqlRaw($"exec sp_desactivarEmpresa {id}");
        }

        public void sp_activarEmpresa(int id)
        {
            Database.ExecuteSqlRaw($"exec sp_activarEmpresa {id}");
        }

        //Metodos para los procedimientos almacenados de empresasUsuarios
        public List<EmpresasUsuarios> sp_listarEmpsUsus()
        {
            return EmpresasUsuarios.FromSqlRaw("exec sp_listarEmpsUsus").ToList();
        }



        


    }
}
