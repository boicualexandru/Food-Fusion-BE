﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Services.Authentication.Exceptions;
using Services.Authentication.Models;

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
            var user = GetUser(loginModel);
            var token = GetToken(user);

            return token;
        }

        public string RegisterAndGetToken(RegisterModel registerModel)
        {
            var user = RegisterAndGetUser(registerModel);
            var token = GetToken(user);

            return token;
        }

        private User GetUser(LoginModel loginModel)
        {
            var user = _dbContext.Users.FirstOrDefault(u =>
                string.Equals(u.Email.Trim(), loginModel.Email.Trim(), StringComparison.InvariantCultureIgnoreCase));
            user = user ?? throw new UserNotFoundException();


            var isLoginModelValid = _hasher.VerifyHashedText(user.HashPassword, loginModel.Password);
            if (!isLoginModelValid)
            {
                throw new InvalidPasswordException();
            }

            return user;
        }

        private User RegisterAndGetUser(RegisterModel registerModel)
        {
            var user = new User
            {
                Email = registerModel.Email.Trim().ToLower(),
                FullName = registerModel.FullName.Trim(),
                HashPassword = _hasher.GetHash(registerModel.Password),
                Role = UserRole.Client
            };

            _dbContext.Users.Add(user);

            try
            {
                _dbContext.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                if (!(ex?.InnerException is SqlException innerEx)) throw;

                // SqlServerViolationOfUniqueIndex 2601
                // SqlServerViolationOfUniqueConstraint 2627
                if (innerEx.Number == 2601 ||
                    innerEx.Number == 2627)
                {
                    throw new EmailAlreadyExistsException();
                }

                throw;
            }

            return user;
        }

        private string GetToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(CustomDefinedClaimNames.FullName, user.FullName),
                new Claim(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()),
                new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.Now.AddDays(_jwtSettings.DaysBeforeExpiration)).ToUnixTimeSeconds().ToString()),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
            };

            var managedRestaurantIds = GetManagedRestaurantIds(user.Id);
            foreach (var restaurantId in managedRestaurantIds)
            {
                claims.Add(new Claim(CustomDefinedClaimNames.ManagedRestaurant, restaurantId.ToString()));
            }

            var employeeOfRestaurantIds = GetEmployeeOfRestaurantIds(user.Id);
            foreach (var restaurantId in employeeOfRestaurantIds)
            {
                claims.Add(new Claim(CustomDefinedClaimNames.EmployeeOfRestaurant, restaurantId.ToString()));
            }

            var token = new JwtSecurityToken(
                new JwtHeader(new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SymetricSecurityKey)),
                    SecurityAlgorithms.HmacSha256)),
                new JwtPayload(claims));

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private IList<int> GetManagedRestaurantIds(int userId)
        {
            var restaurantIds = _dbContext.Restaurants
                .AsNoTracking()
                .Where(r => r.ManagerId == userId)
                .Select(r => r.Id)
                .ToList();

            return restaurantIds;
        }

        private IList<int> GetEmployeeOfRestaurantIds(int userId)
        {
            var restaurantIds = _dbContext.RestaurantEmployees
                .AsNoTracking()
                .Where(re => re.UserId == userId)
                .Select(re => re.RestaurantId)
                .ToList();

            return restaurantIds;
        }
    }
}
