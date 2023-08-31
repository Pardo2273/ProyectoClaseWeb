using Dapper;
using ProyectoClaseWeb.Entities;
using ProyectoClaseWeb.Interfaces;
using System.Data.SqlClient;

namespace ProyectoClaseWeb.Models
{
    public class UsuariosModel : IUsuariosModel
    {
        //interfaz del sistema para inyectarla..
        private readonly IConfiguration _configuration;
        public UsuariosModel(IConfiguration configuration) { 
            _configuration = configuration;
        }
        public UsuariosEntities? ValidarExisteUsuario(UsuariosEntities entidad)
        {
            //todo lo que exista dentro del using va a poder usar la variable conexion
            using(var conexion= new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))//aqui accedemos al string de conexion que esta en el appsettings.json
            {
                //para que me devuelva un resultado o informacion
                return conexion.Query<UsuariosEntities>("ValidarExisteUsuario",
                    new { entidad. CorreoElectronico, entidad.Contrasenna },
                    commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();

                //solo para ejecutar y seguir..
                //conexion.Execute("",
                //    new { }, 
                //    commandType: System.Data.CommandType.StoredProcedure);
            }
        }
    }
}
