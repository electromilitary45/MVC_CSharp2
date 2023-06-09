﻿
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

                entity.Property(eu => eu.Id).HasColumnName("id");
                entity.Property(eu => eu.IdEmpresa).HasColumnName("idEmpresa").IsRequired();
                entity.Property(eu => eu.IdUsuario).HasColumnName("idUsuario").IsRequired();

                entity.HasOne(e => e.Empresas)
                .WithMany() // Especifica el nombre de la propiedad de navegación en la entidad Empresas si existe
                .HasForeignKey(e => e.IdEmpresa);

                entity.HasOne(u => u.Usuarios)
                .WithMany() // Especifica el nombre de la propiedad de navegación en la entidad Usuarios si existe
                .HasForeignKey(e => e.IdUsuario);
            });
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

        public List<Usuarios> sp_ListarUsuariosSinEmpresa() {
            return Usuarios.FromSqlRaw("exec sp_ListarUsuariosSinEmpresa").ToList();
        }

        //public List<Usuarios> sp_encontrarUsuario(string correo,string contrasena)
        //{
        //    //var usuario = Usuarios.FromSqlRaw($"exec sp_encontrarUsuario '{correo}','{contrasena}'").ToList().FirstOrDefault();
        //    var usuario = Usuarios.FromSqlRaw($"exec sp_encontrarUsuario '{correo}','{contrasena}'").ToList().FirstOrDefault();
        //    return usuario;


        //}
        public Usuarios? sp_encontrarUsuario(string correo, string contrasena)
        {
            var usuario = Usuarios.FromSqlRaw($"exec sp_encontrarUsuario '{correo}', '{contrasena}'").AsEnumerable().FirstOrDefault();
            return usuario;
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

        public void sp_insertarEmpUsu(int idEmpresa, int idUsuario)
        {
            Database.ExecuteSqlRaw($"exec sp_insertarEmpUsu {idEmpresa},{idUsuario}");
        }

        public EmpresasUsuarios sp_listarEmpUsu(int id) { 
            var empUsu = EmpresasUsuarios.FromSqlRaw($"exec sp_listarEmpUsu {id}").ToList();
            if (empUsu.Count == 0)
            {
                return null;
            }
            else
            {
                return empUsu[0];
            }
        }

        public void sp_eliminarEmpUsu(int id)
        {
            Database.ExecuteSqlRaw($"exec sp_eliminarEmpUsu {id}");
        }








    }
}
