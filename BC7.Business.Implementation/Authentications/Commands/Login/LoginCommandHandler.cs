using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BC7.Database;
using BC7.Domain;
using BC7.Infrastructure.CustomExceptions;
using BC7.Security;
using BC7.Security.PasswordUtilities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BC7.Business.Implementation.Authentications.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, string>
    {
        private readonly IBitClub7Context _context;
        private readonly IOptions<JwtSettings> _jwtSettings;

        public LoginCommandHandler(IBitClub7Context context, IOptions<JwtSettings> jwtSettings)
        {
            _context = context;
            _jwtSettings = jwtSettings;
        }

        public async Task<string> Handle(LoginCommand command, CancellationToken cancellationToken = default(CancellationToken))
        {
            var user = await _context.Set<UserAccountData>().SingleOrDefaultAsync(x => x.Login == command.LoginOrEmail || x.Email == command.LoginOrEmail);
            if (user is null)
            {
                throw new ValidationException("Invalid credentials");
            }

            VerifyLoginCredentials(command.Password, user.Hash, user.Salt);

            var jwtTokenString = CreateTokenString(user.Email, user.Role);

            return jwtTokenString;
        }

        private void VerifyLoginCredentials(string password, string hash, string salt)
        {
            var isPasswordVerified = PasswordEncryptionUtilities.VerifyPassword(password, hash, salt);
            if (!isPasswordVerified)
            {
                throw new ValidationException("Invalid credentials");
            }
        }

        private string CreateTokenString(string email, string role)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Value.SecretKey));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var tokenOptions = new JwtSecurityToken(
                issuer: "http://localhost:4200", // TODO: Address to change in the future
                audience: "http://localhost:4200",
                claims: new List<Claim>
                {
                    // TODO: More claims if needed
                    new Claim(ClaimTypes.Name, email),
                    new Claim(ClaimTypes.Role, role)
                },
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: signinCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }
    }
}
