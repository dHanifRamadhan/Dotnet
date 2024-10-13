using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Project01.Entities;
using Project01.Models;

namespace Project01.Helpers {
    public class Utils {
        public static string GenerateToken(AuthUser item, string keyApp) {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(keyApp);
            var tokenDescriptor = new SecurityTokenDescriptor { 
                Subject = new ClaimsIdentity(new[] {
                    new Claim("UserCode", item.UserCode)
                }),
                Expires = DateTime.UtcNow.AddYears(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}