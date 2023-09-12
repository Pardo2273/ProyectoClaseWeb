using ProyectoClaseWeb.Entities;

namespace ProyectoClaseWeb.Interfaces
{
    public interface IUsuariosModel
    {
        public UsuariosEntities? ValidarCredenciales(UsuariosEntities entidad);
        public int RegistrarUsuario(UsuariosEntities entidad);
        public string BuscarExisteCorreo(string CorreoElectronico);
        public UsuariosEntities? VerificacionCorreo(UsuariosEntities entidad);
        public void Email(string correo);
    }
}
