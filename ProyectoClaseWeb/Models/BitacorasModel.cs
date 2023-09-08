using Dapper;
using ProyectoClaseWeb.Entities;
using ProyectoClaseWeb.Interfaces;
using System.Data.SqlClient;

namespace ProyectoClaseWeb.Models
{
    public class BitacorasModel : IBitacorasModel
    {
        private readonly IConfiguration _configuration;
        public BitacorasModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void RegistrarBitacora(ErrorBitacoraEntities entidad)
        {
            using (var conexion = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                //solo para ejecutar y seguir
                conexion.Execute("RegistrarBitacora",
                    new {entidad.Detalle, entidad.Origen}, commandType: System.Data.CommandType.StoredProcedure);
            }
        }
    }
}
