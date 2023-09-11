using Dapper;
using ProyectoClaseWeb.Entities;
using ProyectoClaseWeb.Interfaces;
using System.Data;
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
        public UsuariosEntities? ValidarCredenciales(UsuariosEntities entidad)
        {
            using (var client = new HttpClient())
            {
                string urlApi = _configuration.GetSection("Parametros:urlApi").Value + "/Usuarios";//tomando ruta del api y le concatenamos lo que vamos a consumir..
                //serializamos(pasamos un objeto a formato json)
                JsonContent body = JsonContent.Create(entidad);

                HttpResponseMessage response = client.PostAsync(urlApi, body).Result;

                if (response.IsSuccessStatusCode)
                    //Deserializamos(pasamos json a objeto)
                    return response.Content.ReadFromJsonAsync<UsuariosEntities>().Result;

                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    throw new Exception("Excepción Web Api:" + response.Content.ReadAsStringAsync().Result);//si se cae en el api pues hacemos que entre en el catch del controldor del front-end y le enviamos el error que tuvo el api

                 return null;
            }
        }

        public int RegistrarUsuario(UsuariosEntities entidad) { 
            using (var conexion = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                //el Execute retorna el # de filas afectadas... (es lo unico que retorna..)
                return conexion.Execute("RegistrarUsuario", 
                    new { entidad.CorreoElectronico, entidad.Contrasenna }, 
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public string BuscarExisteCorreo(string CorreoElectronico)
        {
            using (var conexion = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var resultado= conexion.Query<UsuariosEntities>("BuscarExisteCorreo",
                    new { CorreoElectronico },
                    commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();

                if(resultado == null)
                    return string.Empty;
                else
                {
                    if (!resultado.Estado)//si es falso..
                        return "Ya existe una cuenta inactiva con este correo";
                    else
                        return "Ya existe una cuenta con este correo";
                }

            }
        }
    }
}
