using BackendTareas.Data;
using BackendTareas.Models;
using BackendTareas.DTOs;
using Microsoft.EntityFrameworkCore;

namespace BackendTareas.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly AppDbContext _context;

        public UsuarioService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<UsuarioDto>> ObtenerUsuarios()
        {
            return await _context.Usuarios
                .Select(u => new UsuarioDto
                {
                    Id = u.Id,
                    Nombre = u.Nombre,
                    Email = u.Email,
                    Rol = u.Rol
                })
                .ToListAsync();
        }

        public async Task<UsuarioDto> CrearUsuario(Usuario usuario)
        {
            var existe = await _context.Usuarios
                .AnyAsync(u => u.Email == usuario.Email);

            if (existe)
                throw new Exception("El email ya existe");

            // Hashear password
            usuario.Password = BCrypt.Net.BCrypt.HashPassword(usuario.Password); 

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return new UsuarioDto
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Email = usuario.Email,
                Rol = usuario.Rol
            };
        }    

        public async Task<UsuarioDto?> ActualizarUsuario(UsuarioDto dto)
        {
            var usuarioExistente = await _context.Usuarios.FindAsync(dto.Id);

            if (usuarioExistente == null)
                return null;

            usuarioExistente.Nombre = dto.Nombre;
            usuarioExistente.Email = dto.Email;
            usuarioExistente.Rol = dto.Rol;

            if (!string.IsNullOrWhiteSpace(dto.Password))
            {
                usuarioExistente.Password =
                    BCrypt.Net.BCrypt.HashPassword(dto.Password);
            }

            await _context.SaveChangesAsync();

            return new UsuarioDto
            {
                Id = usuarioExistente.Id,
                Nombre = usuarioExistente.Nombre,
                Email = usuarioExistente.Email,
                Rol = usuarioExistente.Rol
            };
        }  

        public async Task<bool> EliminarUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
                return false;

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return true;
        }     

        public async Task<Usuario?> Login(string email, string password)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == email);

            if (usuario == null)
                return null;

            bool passwordValido = BCrypt.Net.BCrypt.Verify(password, usuario.Password);

            if (!passwordValido)
                return null;

            return usuario;
        }                
    }
}