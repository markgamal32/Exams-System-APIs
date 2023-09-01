using LMSAPIProject.DTO;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LMSAPIProject
{
    public static class Helper
    {
        public static string ValidateTokenHelp(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var sKey = "welcome to my account in angular exam project";
            var secretKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(sKey));

            try
            {
                SecurityToken validatedToken;
                var claimsPrincipal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = secretKey,
                    ValidateIssuer = false,
                    ValidateAudience = false
                }, out validatedToken);

                var identity = claimsPrincipal.Identity as ClaimsIdentity;
                var emailClaim = identity?.FindFirst(CustomClaims.Email)?.Value;
                if (!string.IsNullOrEmpty(emailClaim))
                {
                    return emailClaim;
                }
                else
                {
                    throw new Exception("Email claim is missing or empty from token.");
                }
            }
            catch (SecurityTokenException ex)
            {
                // Invalid token format
                return null;
            }
            catch (Exception ex)
            {
                // Other errors
                return null;
            }
        }
    }
}
