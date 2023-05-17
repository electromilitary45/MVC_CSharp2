use master
go
-----
create database practica2
go

use practica2
go

drop database practica2
go

--- 

create table categoria(
idCategoria int identity(1,1) primary key,
nombre varchar(50) not null,
descripcion varchar(256) not null,
);
go

create table producto(
idProducto int identity(1,1) primary key,
codigoBarra int not null,
idCategoria int not null,
nombre varchar(50) not null,
descripcion varchar(256) not null,

foreign key (idCategoria) references categoria(idCategoria)
);
go

-----------------------PROCEDIMIENTOS ALMACENADOS--------------
go

----PROCEDIMIENTO REGISTRO CATEGORIA
create procedure sp_registro_categoria(
@Nombre varchar(50),
@Descripcion varchar(256)
)
as 
begin
	insert into categoria(nombre,descripcion) values(@Nombre,@Descripcion)
end
go

----PROCEDIMIENTO ACTUALIZAR CATEGORIA
create procedure sp_actualizar_categoria(
@idCategoria int,
@nombre varchar(50),
@descripcion varchar(256)

)
as
begin
	update categoria set nombre=@nombre, descripcion=@descripcion where idCategoria=@idCategoria
end
go

------ PROCEDIMIENTO ELIMINAR CATEGORIA-------
create procedure sp_eliminar_categoria(
@idCategoria int
)
as
begin
	delete from categoria where idCategoria=@idCategoria
end
go

------ PROCEDIMIENTO LISTAR CATEGORIAS-------
create procedure sp_listar_categorias
as begin
	select * from categoria
end
go

------ PROCEDIMIENTO Listar CATEGORIA Especifica-------
create procedure sp_obtener_categoria(
@idCategoria int
)
as begin
	select * from categoria where @idCategoria=idCategoria
end


exec RegistroCategoria
	@Nombre='Granos',
	@Descripcion='Granos Enteros y Granos Integrales'
go

exec ActualizarCategoria
	@idCategoria=1,
	@nombre='GRANITOS',
	@Descripcion='GRANITOS MAGICOS'
go

exec EliminarCategoria
	@idCategoria=1
go

drop procedure RegistroCategoria
drop procedure ActualizarCategoria
drop procedure EliminarCategoria

--select 
select * from categoria