using ProyectoClaseWeb.Entities;

namespace ProyectoClaseWeb.Interfaces
{
    public interface IUsuariosModel
    {
        public UsuariosEntities? ValidarExisteUsuario(UsuariosEntities entidad);
    }
}
