using CommentService.Domain.Enteties;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CommentService.Services
{
    public class JWTservice
    {
        public string CreateJWToken(string userId, string userNickName, string email, string userRole)
        {
            var claimsIdentity = GetIdentity(userId, userNickName, email, userRole);

            var jwt = new JwtSecurityToken
                (
                    issuer: JWTconfig.ValidIssuer,
                    audience: JWTconfig.ValidAudience,
                    notBefore: DateTime.Now,
                    claims: claimsIdentity.Claims,
                    expires: DateTime.Now.Add(TimeSpan.FromSeconds(120)),
                    signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWTconfig.Key)), SecurityAlgorithms.HmacSha256)
                );

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        private ClaimsIdentity GetIdentity(string userId, string userNickName, string email, string userRole)
        {
            if (email != null && userRole != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, userNickName),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, userRole),
                    new Claim(JwtRegisteredClaimNames.Email, email),
                    new Claim(JwtRegisteredClaimNames.NameId, userId)
                };
                ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                   ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }

            return null;
        }
    }
}
