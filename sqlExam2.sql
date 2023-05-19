use master
go

create database sqlExam2;
go

drop database sqlExam2
go

use sqlExam2
go


------------------------------TABLAS---------------------
create TABLE empresas(
    id int identity(1,1) primary key,
    nombre varchar(50) not null,
    telefono varchar(50) not null,
    ubicacion varchar(50) not null,
    email varchar(256) not null,
	estado int not null
);
go

CREATE table usuarios(
    id int identity(1,1) primary key,
	rol int not null,
    nombre varchar(50) not null,
    apellido varchar(50) not null,
    email varchar(50) not null,
    contrasena varchar(256) not null,
	estado int not null, 
);
go

CREATE TABLE empresasUsuarios(
    id int identity(1,1) primary key,
    idEmpresa int not null,
    idUsuario int not null,
    foreign key(idEmpresa) references empresas(id),
    foreign key(idUsuario) references usuarios(id)
);
go

---------------------------PROCEDIMIENTOS ALMACENADOS--------------
-----USUARIOS-----

create procedure sp_registroUsuario
 @nombre varchar(50),
 @apellido varchar(50),
 @email varchar(50),
 @contrasena varchar(256),
 @rol int
 as begin
	if(not exists(select * from usuarios where email=@email))
		insert into usuarios (nombre,apellido,email,contrasena,estado,rol) values (@nombre,@apellido,@email,@contrasena,1,@rol)
 end
go

create procedure sp_actualizarUsuario
	@id int,
	@nombre varchar(50),
	@apellido varchar(50),
	@email varchar(50),
	@contrasena varchar(256),
	@estado int,
	@rol int
	as begin
		update usuarios set nombre=@nombre,apellido=@apellido,email=@email,contrasena=@contrasena,estado=@estado,rol=@rol 
		where id=@id;
	end
go

--drop procedure sp_actualizarUsuario
--go


create procedure sp_desactivarUsuario
	@id int
	as begin
		update usuarios set estado=0 where id=@id;
	end
go

create procedure sp_activarUsuario
	@id int
	as begin
		update usuarios set estado=1 where id=@id;
	end
go

create procedure sp_listarUsuarios
	as begin
		SELECT u.id, u.nombre, u.apellido, u.email, u.contrasena, u.rol, u.estado
		FROM usuarios u
		
	end
go

--drop procedure sp_listarUsuarios
--go

create procedure sp_listarUsuario
	@id int
	as begin
		SELECT u.id, u.nombre, u.apellido, u.email, u.contrasena, u.rol, u.estado
		FROM usuarios u
		where id=@id
	end
go

--drop procedure sp_listarUsuario
--go

create procedure sp_encontrarUsuario
	@email varchar(50),
	@contrasena varchar(50)
	as begin
		SELECT u.id, u.nombre, u.apellido, u.email, u.rol, u.estado, u.contrasena
		FROM usuarios u
		where email=@email and contrasena=@contrasena
	end
go

drop proc sp_encontrarUsuario
go

CREATE PROCEDURE sp_ListarUsuariosSinEmpresa
	AS
	BEGIN
		SELECT *
		FROM usuarios u
		WHERE NOT EXISTS (
			SELECT 1
			FROM empresasUsuarios eu
			WHERE eu.idUsuario = u.id
		)
	END
go

-----EMPRESAS----
CREATE PROCEDURE sp_insertarEmpresa
	@nombre varchar(50),
	@telefono varchar(50),
	@ubicacion varchar(50),
	@email varchar(50)
	AS BEGIN
		if(not exists(select * from empresas where email=@email))
			INSERT INTO empresas(nombre, telefono, ubicacion, email,estado) VALUES(@nombre, @telefono, @ubicacion, @email,1)
	END
go

create procedure sp_actualizarEmpresa
	@id int,
	@nombre varchar(50),
	@telefono varchar(50),
	@ubicacion varchar(50),
	@email varchar(50),
	@estado int
	as begin
		update empresas set nombre=@nombre,telefono=@telefono,ubicacion=@ubicacion,email=@email,estado=@estado where id=@id
	end
go

create procedure sp_desactivarEmpresa
	@id int
	as begin
		update empresas set estado=0 where id=@id;
	end
go

create procedure sp_activarEmpresa
	@id int
	as begin
		update empresas set estado=1 where id=@id;
	end
go

create procedure sp_listarEmpresas
	as begin
		select e.id, e.nombre, e.telefono, e.ubicacion, e.email, e.estado 
		from empresas e
	end
go

create procedure sp_listarEmpresa
	@id int
	as begin
		select e.id, e.nombre, e.telefono, e.ubicacion, e.email, e.estado 
		from empresas e 
		where id=@id
	end
go

-----EMPRESAS USURAIOS------
create procedure sp_insertarEmpUsu
	@idEmp int,
	@idUsu int
	as begin
		if(not exists(select * from empresasUsuarios where idUsuario=@idUsu))
			insert into empresasUsuarios (idEmpresa, idUsuario) values (@idEmp,@idUsu)
	end
go

drop proc sp_insertarEmpUsu
go

create procedure sp_actualizarEmpUsu
	@id int,
	@idEmp int,
	@idUsu int
	as begin
		update empresasUsuarios set idEmpresa=@idEmp, idUsuario=@idUsu where id=@id
	end
go

create procedure sp_listarEmpsUsus
	as begin
		select eu.id, u.id AS UsuariosId, e.id AS EmpresasId, eu.idEmpresa as idEmpresa, eu.idUsuario as idUsuario,
		e.nombre as NombreEmpresa,u.nombre as NombreUsuario,u.apellido as apellidoUsuario
		from empresasUsuarios eu, usuarios u ,empresas e
		where eu.idEmpresa=e.id and eu.idUsuario=u.id
	end
go

--drop procedure sp_listarEmpsUsus
--go

create procedure sp_listarEmpUsu
	@id int
	as begin
		select eu.id, u.id AS UsuariosId, e.id AS EmpresasId, eu.idEmpresa as idEmpresa, eu.idUsuario as idUsuario
		from empresasUsuarios eu, usuarios u, empresas e
		where idUsuario=@id
	end
go
--drop proc sp_listarEmpUsu
--go
create procedure sp_eliminarEmpUsu
	@id int
	as begin
		delete empresasUsuarios where id=@id
	end
go


--------------------------------USUARIOS SUPERADMIN------------------------------
insert into usuarios(nombre,apellido,email,contrasena,estado,rol) values ('Derek','Leiva','dereklevilla45@gmail.com','12345678',1,3);
go

-----selects------
select * from usuarios
go

select * from empresas
go

select * from empresasUsuarios
go

insert into empresasUsuarios (idEmpresa, idUsuario) values (1,4)



SELECT eu.id, eu.idEmpresa, eu.idUsuario, e.nombre AS NombreEmpresa, u.nombre AS NombreUsuario 
    FROM empresasUsuarios eu
    INNER JOIN empresas e ON eu.idEmpresa = e.id
    INNER JOIN usuarios u ON eu.idUsuario = u.id
go

exec sp_encontrarUsuario @email='dereklevilla45@gmail.com', @contrasena='12345678'
go

exec sp_insertarEmpUsu @idEmp=1,@idUsu=1
go

delete from empresasUsuarios where id=1

delete from usuarios where id=2

insert into empresasUsuarios (idEmpresa, idUsuario) values (1,3)