using Dapper;
using ProyectoClaseWeb.Entities;
using ProyectoClaseWeb.Interfaces;
using System.Data.SqlClient;

namespace ProyectoClaseWeb.Models
{
    public class UsuariosModel : IUsuariosModel
    {
        public void ValidarExisteUsuario(UsuariosEntities entidad)
        {
            //todo lo que exista dentro del using va a poder usar la variable conexion
            using(var conexion= new SqlConnection(""))
            {
                //para que me devuelva un resultado o informacion
                //var resultado = conexion.Query<UsuariosEntities>("",
                //    new {}, 
                //    commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();

                //solo para ejecutar y seguir..
                conexion.Execute("",
                    new { }, 
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }
    }
}
