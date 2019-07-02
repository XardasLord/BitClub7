using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BC7.Common.Settings;
using BC7.Infrastructure.CustomExceptions;
using BC7.Repository;
using BC7.Security.PasswordUtilities;
using MediatR;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BC7.Business.Implementation.Authentications.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, string>
    {
        private readonly IUserAccountDataRepository _userAccountDataRepository;
        private readonly IOptions<JwtSettings> _jwtSettings;

        public LoginCommandHandler(IUserAccountDataRepository userAccountDataRepository, IOptions<JwtSettings> jwtSettings)
        {
            _userAccountDataRepository = userAccountDataRepository;
            _jwtSettings = jwtSettings;
        }

        public async Task<string> Handle(LoginCommand command, CancellationToken cancellationToken = default(CancellationToken))
        {
            var user = await _userAccountDataRepository.GetAsync(command.LoginOrEmail);
            if (user is null)
            {
                throw new ValidationException("Invalid credentials");
            }

            VerifyLoginCredentials(command.Password, user.Hash, user.Salt);

            var jwtTokenString = CreateTokenString(user.Id, user.Email, user.Role);

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

        private string CreateTokenString(Guid id, string email, string role)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Value.SecretKey));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            
            var tokenOptions = new JwtSecurityToken(
                issuer: "http://localhost:4200", // TODO: Address to change in the future
                audience: "http://localhost:4200",
                claims: new List<Claim>
                {
                    new Claim(ClaimTypes.Sid, id.ToString()),
                    new Claim(ClaimTypes.Email, email),
                    new Claim(ClaimTypes.Role, role)
                },
                expires: DateTime.Now.AddHours(1), // To rethink
                signingCredentials: signinCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }
    }
}
