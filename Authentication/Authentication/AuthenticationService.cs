using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using DataAccess.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Services.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly FoodFusionContext _dbContext;
        private readonly IHasher _hasher;
        private readonly JwtSettings _jwtSettings;

        public AuthenticationService(FoodFusionContext dbContext, 
            IHasher hasher,
            IOptions<JwtSettings> jwtSettings)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _hasher = hasher ?? throw new ArgumentNullException(nameof(hasher));
            _jwtSettings = jwtSettings?.Value ?? throw new ArgumentNullException(nameof(jwtSettings));
        }

        public string GetToken(LoginModel loginModel)
        {
            var user = GetUserByLoginModel(loginModel);
            var token = GetTokenByUser(user);

            return token;
        }

        private User GetUserByLoginModel(LoginModel loginModel)
        {
            var user = _dbContext.Users.First(u =>
                string.Equals(u.Email.Trim(), loginModel.Email.Trim(), StringComparison.InvariantCultureIgnoreCase));

            var isLoginModelValid = _hasher.VerifyHashedText(user.HashPassword, loginModel.Password);
            if (!isLoginModelValid)
            {
                throw new InvalidOperationException("Invalid LoginModel");
            }

            return user;
        }

        private string GetTokenByUser(User user)
        {
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(CustomDefinedClaimNames.FullName, user.FullName),
                new Claim(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()),
                new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.Now.AddDays(_jwtSettings.DaysBeforeExpiration)).ToUnixTimeSeconds().ToString()),
                new Claim(CustomDefinedClaimNames.Role, user.Role.ToString()),
            };

            var token = new JwtSecurityToken(
                new JwtHeader(new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SymetricSecurityKey)),
                    SecurityAlgorithms.HmacSha256)),
                new JwtPayload(claims));

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
