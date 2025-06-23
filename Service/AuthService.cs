using MemoriaAPI.Data;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MemoriaAPI.Service
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _config;
        private readonly MemoriaDbContext _context;

        public AuthService(IConfiguration config, MemoriaDbContext context)
        {
            _config = config;
            _context = context;
        }

        public string? Authenticate(string username, string password)
        {
            var user = _context.Usuarios.FirstOrDefault(u =>
                u.NombreUsuario == username && u.Contraseña == password); // ⚠️ solo para demo, sin hash

            if (user == null) return null;

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.NombreUsuario!),
                new Claim(ClaimTypes.Role, user.Rol ?? "Usuario")
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(int.Parse(_config["Jwt:ExpiresInMinutes"]!)),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
