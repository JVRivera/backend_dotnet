using BackendTareas.Services;
using Microsoft.AspNetCore.Mvc;
using BackendTareas.Models;
using BackendTareas.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BackendTareas.Controllers
{
    [ApiController]
    [Route("api/usuarios")]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuariosController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerUsuarios()
        {
            var tareas = await _usuarioService.ObtenerUsuarios();
            return Ok(tareas);
        }

        [HttpPost]
        public async Task<IActionResult> CrearUsuario([FromBody] Usuario usuario)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var nuevoUsuario = await _usuarioService.CrearUsuario(usuario);
            return Ok(nuevoUsuario);
        }         
           
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarUsuario(int id, [FromBody] UsuarioDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != dto.Id)
                return BadRequest("El ID no coincide");

            var usuarioActualizado = await _usuarioService.ActualizarUsuario(dto);

            if (usuarioActualizado == null)
                return NotFound();

            return Ok(usuarioActualizado);
        }   

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var result = await _usuarioService.EliminarUsuario(id);

            if (!result)
                return NotFound("Usuario no encontrada");

            return Ok(result);
        }     

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO login)
        {
            var usuario = await _usuarioService.Login(login.Email, login.Password);

            if (usuario == null)
                return Unauthorized("Usuario o contraseña incorrectos");

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes("MiClaveSuperSecretaParaJWT2024SistemaTareasSegura");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, usuario.Nombre),
                    new Claim(ClaimTypes.Email, usuario.Email),
                    new Claim(ClaimTypes.Role, usuario.Rol)
                }),

                Expires = DateTime.UtcNow.AddHours(2), // Expiración
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new
            {
                token = tokenHandler.WriteToken(token),
                usuario
            });
        }                  
    }
}