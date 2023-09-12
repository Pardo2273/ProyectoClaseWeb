using Dapper;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using ProyectoClaseWeb.Entities;
using ProyectoClaseWeb.Interfaces;
using System.Data;
using System.Data.SqlClient;
using System.Net.Mail;

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

        public UsuariosEntities? VerificacionCorreo(UsuariosEntities entidad)
        {
            using (var conexion = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                return conexion.Query<UsuariosEntities>("BuscarExisteCorreo",
                    new { entidad.CorreoElectronico },
                    commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            }
        }

        private string? RecuperarContrasenna(string CorreoElectronico)
        {
            using (var conexion = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                return conexion.Query<string>("RecuperarContrasenna",
                    new { CorreoElectronico },
                    commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            }
        }

        public void Email(string correo)
        {
            //aqui consulto el password
            var respuesta = RecuperarContrasenna(correo);

            //declaro las variables para llenar lo necesario del mailkit
            var CorreoServidor = _configuration.GetSection("Parametros:CorreoServidor").Value;
            var ContrasennaServidor = _configuration.GetSection("Parametros:Contrasenna").Value;
            var Servidor = _configuration.GetSection("Parametros:Servidor").Value;
            var Puerto= _configuration.GetSection("Parametros:Puerto").Value;
            //rellenamos lo solicitado..
            var mensaje = new MimeMessage();
            mensaje.From.Add(MailboxAddress.Parse(CorreoServidor));//el .parse es solo para poner correos y evitar poner nombres..
            mensaje.To.Add(MailboxAddress.Parse(correo));
            mensaje.Subject = "Su contraseña es la siguiente:";
            mensaje.Body = new TextPart(TextFormat.Html)//ponemos que el texto tendra formato html para darle un poco de valor al correo
            {
                Text = "<h1>Estimado/a " + correo + ",</h1>" +
                        "<p>Hemos recibido su solicitud de recuperación de contraseña para su cuenta</p>" +
                           "<p>A continuación, le proporcionamos la información solicitada:</p>" +
                           "<p><strong>Correo electrónico:</strong> " + correo + "</p>" +
                           "<p><strong>Contraseña:</strong> " + respuesta + "</p>" 
            };
            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            smtp.Connect(Servidor, int.Parse(Puerto), false);
            smtp.Authenticate(CorreoServidor, ContrasennaServidor);
            smtp.Send(mensaje);
            smtp.Disconnect(true);
        }


    }
}
