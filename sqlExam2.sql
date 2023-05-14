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
	estado bit not null
);
go

CREATE table usuarios(
    id int identity(1,1) primary key,
	rol int not null,
    nombre varchar(50) not null,
    apellido varchar(50) not null,
    email varchar(50) not null,
    contrasena varchar(256) not null,
	estado bit not null, 
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
	@estado bit,
	@rol int
	as begin
		update usuarios set nombre=@nombre,apellido=@apellido,email=@email,contrasena=@contrasena,estado=@estado,rol=@rol 
		where id=@id;
	end
go

drop procedure sp_actualizarUsuario
go


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

drop procedure sp_listarUsuarios
go

create procedure sp_listarUsuario
	@id int
	as begin
		SELECT u.id, u.nombre, u.apellido, u.email, u.contrasena, u.rol, u.estado
		FROM usuarios u
		where id=@id
	end
go

drop procedure sp_listarUsuario
go

-----EMPRESAS----
CREATE PROCEDURE sp_insertarEmpresa
	@nombre varchar(50),
	@telefono varchar(50),
	@ubicacion varchar(50),
	@email varchar(50),
	@estado bit
	AS BEGIN
		if(not exists(select * from empresas where email=@email))
			INSERT INTO empresas(nombre, telefono, ubicacion, email,estado) VALUES(@nombre, @telefono, @ubicacion, @email,@estado)
	END
go

create procedure sp_actualizarEmpresa
	@id int,
	@nombre varchar(50),
	@telefono varchar(50),
	@ubicacion varchar(50),
	@email varchar(50),
	@estado bit
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
		select e.id,e.nombre, e.email,e.telefono,e.ubicacion,e.estado
		from empresas e
	end
go

create procedure sp_listarEmpresa
	@id int
	as begin
		select * from empresas where id=@id
	end
go


---------------------------------ROLES PREDETERMINADOS-----------------------
INSERT INTO Roles(nombre) VALUES('Cliente');
go
INSERT INTO Roles(nombre) VALUES('Admin');
go
INSERT INTO Roles(nombre) VALUES('SuperAdmin');
go
INSERT INTO Roles(nombre) VALUES('Empleado');
go

--------------------------------USUARIOS SUPERADMIN------------------------------
insert into usuarios(nombre,apellido,email,contrasena,estado,rol) values ('Derek','Leiva','dereklevilla45@gmail.com','12345678',1,3);

-----selects
select * from usuarios
select * from empresas
