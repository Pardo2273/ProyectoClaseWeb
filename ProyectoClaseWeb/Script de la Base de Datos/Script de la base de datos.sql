create database ProyectoWeb

create table Usuarios(
ConsecutivoUsuario bigint primary key identity(1,1),
CorreoElectronico varchar(70) unique, 
Contrasenna varchar(15), 
Estado bit)

Create table Errores(
ConsecutivoError bigint primary key identity(1,1),
Fecha datetime,
Detalle varchar(max),
Origen varchar(250),
ConsecutivoUsuario bigint,
foreign key(ConsecutivoUsuario) references Usuarios(ConsecutivoUsuario))

/*Procedimientos Almacenados*/
Create procedure ValidarExisteUsuario
@CorreoElectronico varchar(70),
@Contrasenna varchar(15)
as
begin
	Select ConsecutivoUsuario, CorreoElectronico, Estado
	From Usuarios
	where CorreoElectronico =  @CorreoElectronico
	and Contrasenna=@Contrasenna
	and Estado=1
end
go

/*prueba de locos*/
create procedure x
as
begin select ""
end