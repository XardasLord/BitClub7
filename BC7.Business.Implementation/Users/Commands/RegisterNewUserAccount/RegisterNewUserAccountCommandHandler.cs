using System;
using System.Threading;
using System.Threading.Tasks;
using BC7.Business.Helpers;
using BC7.Database;
using BC7.Domain;
using BC7.Infrastructure.CustomExceptions;
using BC7.Repository;
using BC7.Security;
using BC7.Security.PasswordUtilities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BC7.Business.Implementation.Users.Commands.RegisterNewUserAccount
{
    public class RegisterNewUserAccountCommandHandler : IRequestHandler<RegisterNewUserAccountCommand, Guid>
    {
        private readonly IBitClub7Context _context;
        private readonly IUserMultiAccountHelper _userMultiAccountHelper;
        private readonly IUserMultiAccountRepository _userMultiAccountRepository;

        public RegisterNewUserAccountCommandHandler(
            IBitClub7Context context,
            IUserMultiAccountHelper userMultiAccountHelper,
            IUserMultiAccountRepository userMultiAccountRepository)
        {
            _context = context;
            _userMultiAccountHelper = userMultiAccountHelper;
            _userMultiAccountRepository = userMultiAccountRepository;
        }

        public async Task<Guid> Handle(RegisterNewUserAccountCommand command, CancellationToken cancellationToken = default(CancellationToken))
        {
            await ValidateForUniqueness(command);

            var sponsorId = await GetSponsorId(command);
            var hashSalt = PasswordEncryptionUtilities.GenerateSaltedHash(command.Password);

            var userAccountData = new UserAccountData
            (
                id: Guid.NewGuid(),
                email: command.Email,
                login: command.Login,
                firstName: command.FirstName,
                lastName: command.LastName,
                street: command.Street,
                city: command.City,
                zipCode: command.ZipCode,
                country: command.Country,
                btcWalletAddress: command.BtcWalletAddress,
                role: UserRolesHelper.User
            );

            userAccountData.SetPassword(hashSalt.Salt, hashSalt.Hash);

            await _context.Set<UserAccountData>().AddAsync(userAccountData);
            await _context.SaveChangesAsync();

            // TODO: Event that user was created: eventhandler should create new multiaccount for him
            var userMultiAccount = new UserMultiAccount
            (
                id: Guid.NewGuid(),
                userAccountDataId: userAccountData.Id,
                sponsorId: sponsorId,
                multiAccountName: userAccountData.Login
            );
            userMultiAccount.SetAsMainAccount();

            await _userMultiAccountRepository.CreateAsync(userMultiAccount);

            return userAccountData.Id;
        }

        private async Task ValidateForUniqueness(RegisterNewUserAccountCommand command)
        {
            var isDuplicated = await _context.Set<UserAccountData>()
                            .AnyAsync(x => x.Email == command.Email || x.Login == command.Login);

            if (isDuplicated)
            {
                throw new ValidationException("User with given email or login already exists");
            }
        }

        private Task<Guid> GetSponsorId(RegisterNewUserAccountCommand command)
        {
            if (!string.IsNullOrEmpty(command.SponsorRefLink))
            {
                return GetSponsorByReflink(command.SponsorRefLink);
            }

            if (!string.IsNullOrEmpty(command.SponsorLogin))
            {
                return GetSponsorByLogin(command.SponsorLogin);
            }

            return GetRandomSponsorId();
        }

        private async Task<Guid> GetSponsorByLogin(string login)
        {
            var sponsor = await _userMultiAccountRepository.GetByAccountNameAsync(login);

            if (sponsor is null)
            {
                throw new InvalidOperationException("User with multi account name does not exist");
            }

            return sponsor.Id;
        }

        private async Task<Guid> GetSponsorByReflink(string reflink)
        {
            var sponsor = await _userMultiAccountRepository.GetByReflinkAsync(reflink);

            if (sponsor is null)
            {
                throw new InvalidOperationException("User with given reflink does not exist");
            }

            return sponsor.Id;
        }

        private async Task<Guid> GetRandomSponsorId()
        {
            var randomSponsor = await _userMultiAccountHelper.GetRandomUserMultiAccount();

            return randomSponsor.Id;
        }
    }
}