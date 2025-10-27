using Book_Clinic.Authentication.Services;
using Book_Clinic.Data;
using Book_Clinic.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Book_Clinic.Authentication.Utilities
{
    public class JwtTokenGenerator
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;

        public JwtTokenGenerator(IConfiguration configuration, ApplicationDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }


        public async Task<string> GenerateToken(User user)
        {
            if (string.IsNullOrEmpty(user.Id) || string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Role))
                throw new ArgumentException("User properties cannot be null or empty");

            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.UserName ?? user.Email),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
        };

            // ✅ Fetch the JWT key directly from the DB (or use a helper method)
            var jwtKey = await _context.JwtKeys
                .Where(s => s.KeyName == "JwtKey")
                .Select(s => s.Value)
                .FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(jwtKey))
                throw new Exception("JWT key not found in database.");

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(Convert.ToDouble(_configuration["Jwt:ExpireDays"])),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }


}
