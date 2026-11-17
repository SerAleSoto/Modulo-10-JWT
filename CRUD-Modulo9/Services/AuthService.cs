using CRUD_Modulo9.Data;
using CRUD_Modulo9.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;


namespace CRUD_Modulo9.Services
{
    public class AuthService
    {
        private readonly IConfiguration _config;
        private readonly AppDbContext _context;

        public AuthService(IConfiguration config, AppDbContext context)
        {
            _config = config;
            _context = context;
        }

        public string? Login(string username,string password)
        {
            var usuario = _context.Usuarios 
                            .FirstOrDefault(x => x.Username == username && x.Password == password);

            if (usuario == null)
                return null;

            return GenerarToken(usuario);
        }

        private string GenerarToken(Usuario usuario)
        {
            var claims = new[]
            {
              new Claim(ClaimTypes.Name,usuario.Username),
              new Claim(ClaimTypes.Role,usuario.Rol)
            };

            var jwtKey = _config["Jwt:Key"];
            if (string.IsNullOrWhiteSpace(jwtKey))
                throw new InvalidOperationException("La clave JWT no está configurada.");

            var keyBytes = Encoding.UTF8.GetBytes(jwtKey);
            if (keyBytes.Length < 32)
                keyBytes = SHA256.HashData(keyBytes);

            var key = new SymmetricSecurityKey(keyBytes);
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(issuer:_config["Jwt:Issuer"],
                            audience: _config["Jwt:Audience"],
                            claims: claims,
                            expires: DateTime.Now.AddHours(2),
                            signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
