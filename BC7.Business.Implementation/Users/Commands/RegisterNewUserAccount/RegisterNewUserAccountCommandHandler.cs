using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BC7.Business.Helpers;
using BC7.Database;
using BC7.Entity;
using BC7.Security;
using BC7.Security.PasswordUtilities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BC7.Business.Implementation.Users.Commands.RegisterNewUserAccount
{
    public class RegisterNewUserAccountCommandHandler : IRequestHandler<RegisterNewUserAccountCommand, Guid>
    {
        private readonly IBitClub7Context _context;
        private readonly IMapper _mapper;
        private readonly IReflinkHelper _reflinkHelper;
        private readonly IUserMultiAccountHelper _userMultiAccountHelper;

        public RegisterNewUserAccountCommandHandler(
            IBitClub7Context context,
            IMapper mapper,
            IReflinkHelper reflinkHelper,
            IUserMultiAccountHelper userMultiAccountHelper)
        {
            _context = context;
            _mapper = mapper;
            _reflinkHelper = reflinkHelper;
            _userMultiAccountHelper = userMultiAccountHelper;
        }

        public async Task<Guid> Handle(RegisterNewUserAccountCommand command, CancellationToken cancellationToken = default(CancellationToken))
        {
            await ValidateForUniqueness(command);

            var invitingUserMultiAccountId = await GetInvitingUserId(command);

            var userAccountData = _mapper.Map<UserAccountData>(command);

            var hashSalt = PasswordEncryptionUtilities.GenerateSaltedHash(command.Password);
            userAccountData.Salt = hashSalt.Salt;
            userAccountData.Hash = hashSalt.Hash;
            userAccountData.Role = UserRolesHelper.User;
            userAccountData.IsMembershipFeePaid = false;

            await _context.Set<UserAccountData>().AddAsync(userAccountData);
            await _context.SaveChangesAsync();

            // TODO: Event that user was created: eventhandler should create new multiaccount for him
            var userMultiAccount = new UserMultiAccount
            {
                UserAccountDataId = userAccountData.Id,
                UserMultiAccountInvitingId = invitingUserMultiAccountId,
                MultiAccountName = userAccountData.Login,
                RefLink = _reflinkHelper.GenerateReflink(), // TODO: This should be empty on first account creating - it should be generated after buying the lvl0 matrix position
                IsMainAccount = true
            };
            await _context.Set<UserMultiAccount>().AddAsync(userMultiAccount);
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

        private Task<Guid> GetInvitingUserId(RegisterNewUserAccountCommand command)
        {
            if (!string.IsNullOrEmpty(command.InvitingRefLink))
            {
                return GetInvitingUserIdByRefLink(command.InvitingRefLink);
            }

            if (!string.IsNullOrEmpty(command.InvitingUserLogin))
            {
                return GetInvitingUserIdByLogin(command.InvitingUserLogin);
            }

            return GetRandomInvitingUserId();
        }

        private async Task<Guid> GetInvitingUserIdByLogin(string login)
        {
            var invitingUser = await _userMultiAccountHelper.GetByAccountName(login);

            if (invitingUser == null)
            {
                throw new InvalidOperationException("User with given reflink does not exist");
            }

            return invitingUser.Id;
        }

        private async Task<Guid> GetInvitingUserIdByRefLink(string reflink)
        {
            var invitingUser = await _userMultiAccountHelper.GetByReflink(reflink);

            if (invitingUser == null)
            {
                throw new InvalidOperationException("User with given reflink does not exist");
            }

            return invitingUser.Id;
        }

        private async Task<Guid> GetRandomInvitingUserId()
        {
            var randomSponsor = await _userMultiAccountHelper.GetRandomUserMultiAccount();

            return randomSponsor.Id;
        }
    }
}