using BackendTareas.Models;
using BackendTareas.DTOs;

namespace BackendTareas.Services
{
    public interface IUsuarioService
    {
        Task<List<UsuarioDto>> ObtenerUsuarios();
        Task<UsuarioDto> CrearUsuario(Usuario usuario);

        Task<UsuarioDto?> ActualizarUsuario(UsuarioDto dto);

        Task<bool> EliminarUsuario(int id);

        Task<Usuario?> Login(string email, string password);
    }
}