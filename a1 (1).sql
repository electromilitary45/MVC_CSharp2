
USE sqlExam
GO
CREATE LOGIN [derek] WITH PASSWORD=N'123', DEFAULT_DATABASE=sqlExam, CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
GO

-----DANGER-----
use master;
go

create database sqlExam;
go

use sqlExam;
go

drop DATABASE sqlExam;
go

drop procedure sp_obtenerUsuario;
go

drop procedure sp_registroUsuario;
go
------TABLAS------
create TABLE empresas(
    id int identity(1,1) primary key,
    nombre varchar(50) not null,
    telefono varchar(50) not null,
    ubicacion varchar(50) not null,
    email varchar(256) not null,
	estado bit not null
);
go

CREATE table usuarios(
    id int identity(1,1) primary key,
    nombre varchar(50) not null,
    apellido varchar(50) not null,
    email varchar(50) not null,
    contrasena varchar(256) not null,
	estado bit not null,
);
go

CREATE TABLE Roles(
    id int identity(1,1) primary key,
    nombre varchar(50) not null
);
go

CREATE TABLE usuariosRoles(
    id int identity(1,1) primary key,
    idUsuario int not null,
    idRol int not null,
    foreign key(idUsuario) references usuarios(id),
    foreign key(idRol) references Roles(id)
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

-------PROCEDIMIENTOS-------
declare @tipo int;
go
declare @id as int;
go
declare @idEmpresa int;
go
----procedimientos empresas----
CREATE PROCEDURE spInsertarEmpresa(
@nombre varchar(50),
@telefono varchar(50),
@ubicacion varchar(50),
@email varchar(50),
@estado bit
)
AS BEGIN
	if(not exists(select * from empresas where email=@email))
		INSERT INTO empresas(nombre, telefono, ubicacion, email,estado) VALUES(@nombre, @telefono, @ubicacion, @email,@estado)
END
go

create procedure sp_ListarEmpresas
	as begin
		select * from empresas;
	end
go

create procedure sp_obtenerEmpresa(
	@id int
)
as begin
	select * from empresas where id=@id;
end
go

create procedure sp_actualizarEmpresa(
	@id int,
	@nombre varchar(50),
	@telefono varchar(50),
	@ubicacion varchar(50),
	@email varchar(256),
	@estado bit
)
as begin
	update empresas set nombre=@nombre,email=@email,estado=@estado,telefono=@telefono,ubicacion=@ubicacion;
end
go

---procediminetos usuairos
create procedure sp_registroUsuario(
	@nombre varchar(50),
	@apellido varchar(50),
	@email varchar(50),
	@contrasena varchar(256),
	@tipo int,
	@id int,
	@idEmpresa int,
	@estado bit
)
as 
begin
	if(not exists(select * from usuarios where @email=email))
		begin
			if (@tipo = 1)
				begin
					insert into usuarios (nombre,apellido,email,contrasena,estado) values (@nombre , @apellido, @email, @contrasena,@estado);
					set @id = (select usuarios.id from usuarios where @email=email);
					insert into usuariosRoles (idRol,idUsuario) values (1, @id);
				end
			else if (@tipo= 2)
				begin
					insert into usuarios (nombre,apellido,email,contrasena,estado) values (@nombre , @apellido, @email, @contrasena,@estado);
					set @id = (select usuarios.id from usuarios where @email=email);
					insert into usuariosRoles (idRol,idUsuario) values (2, @id);
					insert into empresasUsuarios (idEmpresa,idUsuario) values (@idEmpresa,@id);
				end
			else if(@tipo = 3)
				begin
					insert into usuarios (nombre,apellido,email,contrasena,estado) values (@nombre , @apellido, @email, @contrasena,@estado);
					set @id = (select usuarios.id from usuarios where @email=email);
					insert into usuariosRoles (idRol,idUsuario) values (3, @id);
					insert into empresasUsuarios (idEmpresa,idUsuario) values (@idEmpresa,@id);
				end
			else
				begin
					insert into usuarios (nombre,apellido,email,contrasena,estado) values (@nombre , @apellido, @email, @contrasena,@estado);
					set @id = (select usuarios.id from usuarios where @email=email);
					insert into usuariosRoles (idRol,idUsuario) values (4, @id);
					insert into empresasUsuarios (idEmpresa,idUsuario) values (@idEmpresa,@id);
				end
		end
end
go

create procedure sp_obtenerUsuario(
@id int
)
as begin
	if((select idRol from usuariosRoles where idUsuario=@id)!=2) --1cliente
		select u.nombre, u.apellido, u.email,u.contrasena,e.nombre,r.nombre
		from usuarios u
		join usuariosRoles ur on u.id=ur.idUsuario
		join roles r on ur.idRol= r.id
		join empresasUsuarios eu on u.id=eu.idUsuario
		join empresas e on eu.idEmpresa=e.id
		where u.id=@id
	else
		select u.nombre, u.apellido, u.email,u.contrasena,r.nombre
		from usuarios u
		join usuariosRoles ur on u.id=ur.idUsuario
		join roles r on ur.idRol= r.id
		where u.id=@id
end
go

create procedure sp_obtenerUsuarios
as begin
	select u.id,u.nombre, u.apellido,u.email,u.contrasena,u.estado,e.nombre,r.nombre
	from usuarios u
	join usuariosRoles ur on u.id=ur.idUsuario
	join roles r on ur.idRol= r.id
	join empresasUsuarios eu on u.id=eu.idUsuario
	join empresas e on eu.idEmpresa=e.id
	
end
go

create procedure sp_obtenerUsuariosEmpresa(
@idEmpresa int
)
as begin
	select u.nombre, u.apellido, u.email, u.contrasena, r.nombre, e.nombre
	from usuarios u
	join usuariosRoles ur on u.id=ur.idUsuario
	join roles r on ur.idRol= r.id
	join empresasUsuarios eu on u.id=eu.idUsuario
	join empresas e on eu.idEmpresa=e.id
	where e.id= @idEmpresa
end
go

create procedure sp_actualizarUsuario(
@id int,
@nombre varchar(50),
@apellido varchar(50),
@email varchar(50),
@contrasena varchar(256),
@estado bit,
@tipo int,
@rol int
)
as begin
	update usuarios set nombre=@nombre,apellido=@apellido,email=@email,contrasena=@contrasena,estado=@estado where id=@id;
	update usuariosRoles set idRol=@rol where idUsuario=@id;
end
go

------PROCEDIMINETOS ROLES---------
create procedure sp_obtenerroles
as begin
	select  * from Roles;
end
go

create procedure sp_obtenerUsuarioRoles
	as begin
		select * from usuariosRoles;
	end
go

------INSERT ROLES------
INSERT INTO Roles(nombre) VALUES('Cliente');
go
INSERT INTO Roles(nombre) VALUES('Admin');
go
INSERT INTO Roles(nombre) VALUES('SuperAdmin');
go
INSERT INTO Roles(nombre) VALUES('Empleado');
go
----INSERT EMPRESAS------
insert into empresas(nombre,email,telefono,ubicacion,estado) values('Mustang','mustag@gmail.com','88884444','cartago',1);
go
insert into empresas(nombre,email,telefono,ubicacion,estado) values('Ford','ford@gmail.com','88884444','cartago',1);
go

-----INSERT USUARIOS-----
insert into usuarios(nombre,apellido,email,contrasena,estado) values ('Derek','Leiva','derek@gmail.com','Leiva1234',1);
go
insert into usuariosRoles(idRol,idUsuario) values(3,1);
go

	
----------EXEC USUAARIOS------
exec sp_registroUsuario @nombre='alejad', @apellido='Leiva',@email='asdle@gmail.com',@contrasena='Villaley45*',@tipo=1,@id=0,@idEmpresa=0,@estado=1;
go

--------EXEC EMPRESAS----------
exec spInsertarEmpresa @nombre='FORD', @email='asd@gmail.com',@telefono='88884444',@ubicacion='cartago',@estado=1;
go

exec sp_obtenerUsuariosEmpresa @idEmpresa = 2
go

-------------SELECTS GENERALES------
select * from empresas
go
select * from Roles
go
select * from usuariosRoles
go
select * from usuarios
go
select * from empresasUsuarios
go

--select usuarios.id from usuarios where email='dereklevilla45@gmail'
--delete from usuarios 



--usuario maestro--

exec sp_obtenerUsuario @id=2
go

--select u.nombre, u.apellido, u.email,u.contrasena,e.nombre,r.nombre
--from usuarios u
--join usuariosRoles ur on u.id=ur.idUsuario
--join roles r on ur.idRol= r.id
--join empresasUsuarios eu on u.id=eu.idUsuario
--join empresas e on eu.idEmpresa=e.id
--where u.id=2

--select u.nombre, u.apellido, u.email, u.contrasena, r.nombre, e.nombre
--from usuarios u
--join usuariosRoles ur on u.id=ur.idUsuario
--join roles r on ur.idRol= r.id
--join empresasUsuarios eu on u.id=eu.idUsuario
--join empresas e on eu.idEmpresa=e.id
--where e.id= 1






