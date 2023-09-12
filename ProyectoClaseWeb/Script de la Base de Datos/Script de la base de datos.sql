create database ProyectoWeb
use ProyectoWeb

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

Create table Bitacoras(
ConsecutivoBitacora bigint primary key identity(1,1),
Fecha datetime,
Detalle varchar(max),
Origen varchar(250))

/*Procedimientos Almacenados de Usuarios*/
Create procedure ValidarCredenciales
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

create procedure RegistrarUsuario
@CorreoElectronico varchar(70),
@Contrasenna varchar(15)
as
begin
	INSERT INTO [dbo].[Usuarios]([CorreoElectronico],[Contrasenna],[Estado])
	 VALUES(@CorreoElectronico,@Contrasenna, 1)
end
go

create procedure BuscarExisteCorreo
@CorreoElectronico varchar(70)
as
begin
	Select ConsecutivoUsuario, CorreoElectronico, Estado
	From Usuarios
	where CorreoElectronico =  @CorreoElectronico
end 
go

create procedure RecuperarContrasenna
@CorreoElectronico varchar(70)
as 
begin
	Select Contrasenna 
	from Usuarios
	WHERE CorreoElectronico= @CorreoElectronico
end
go

/*Procedimientos Almacenados de Bitacoras y Errores*/
create procedure RegistrarBitacora
@Detalle varchar(max),
@Origen varchar(250)
as
begin
	Insert into Bitacoras values(GETDATE() ,@Detalle, @Origen)
end
go