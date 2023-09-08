using ProyectoClaseWeb.Interfaces;
using ProyectoClaseWeb.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession();//esto tambien debe ir para habilitar el uso de las variables de sesion

//aqui tenemos que hacer la inyeccion de dependencias, por cada clase que queramos inyectar se hace de la siguiente forma
builder.Services.AddScoped<IUsuariosModel, UsuariosModel>();
builder.Services.AddScoped<IBitacorasModel, BitacorasModel>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();//con esto habilitamos el uso de variables de sesion 
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
