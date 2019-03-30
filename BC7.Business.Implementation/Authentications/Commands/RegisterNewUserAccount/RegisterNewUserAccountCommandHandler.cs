using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BC7.Database;
using BC7.Entity;
using BC7.Security;
using BC7.Security.PasswordUtilities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BC7.Business.Implementation.Authentications.Commands.RegisterNewUserAccount
{
    public class RegisterNewUserAccountCommandHandler : IRequestHandler<RegisterNewUserAccountCommand, Guid>
    {
        private readonly IBitClub7Context _context;
        private readonly IMapper _mapper;

        public RegisterNewUserAccountCommandHandler(IBitClub7Context context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(RegisterNewUserAccountCommand command, CancellationToken cancellationToken = default(CancellationToken))
        {
            await ValidateForUniqueness(command);

            var userAccountData = _mapper.Map<UserAccountData>(command);

            var hashSalt = PasswordEncryptionUtilities.GenerateSaltedHash(command.Password);
            userAccountData.Salt = hashSalt.Salt;
            userAccountData.Hash = hashSalt.Hash;
            userAccountData.Role = UserRolesHelper.User;

            await _context.Set<UserAccountData>().AddAsync(userAccountData);
            await _context.SaveChangesAsync();

            return userAccountData.Id;
        }

        private async Task ValidateForUniqueness(RegisterNewUserAccountCommand command)
        {
            var isDuplicated = await _context.Set<UserAccountData>()
                            .AnyAsync(x => x.Email == command.Email || x.Login == command.Login);

            if (isDuplicated)
            {
                // TODO: Maybe throw custom Exception type and handle it as a BadRequest or other error message code
                throw new InvalidOperationException("User with given email or login already exists");
            }
        }
    }
}