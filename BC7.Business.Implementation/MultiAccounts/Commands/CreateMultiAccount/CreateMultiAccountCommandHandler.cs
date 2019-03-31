using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BC7.Database;
using BC7.Entity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BC7.Business.Implementation.MultiAccounts.Commands.CreateMultiAccount
{
    public class CreateMultiAccountCommandHandler : IRequestHandler<CreateMultiAccountCommand, Guid>
    {
        private CreateMultiAccountCommand _command;
        private readonly IBitClub7Context _context;

        public CreateMultiAccountCommandHandler(IBitClub7Context context)
        {
            _context = context;
        }

        public async Task<Guid> Handle(CreateMultiAccountCommand command, CancellationToken cancellationToken)
        {
            _command = command;

            await ValidateIfMultiAccountCanBeCreated();

            var createdMultiAccount = await CreateMultiAccount();

            return createdMultiAccount.Id;
        }

        private async Task ValidateIfMultiAccountCanBeCreated()
        {
            // TODO: Custom exception
            var userAccount = await _context.Set<UserAccountData>()
                .Include(x => x.UserMultiAccounts)
                .SingleOrDefaultAsync(x => x.Id == _command.UserAccountId);
            if (userAccount == null)
            {
                throw new InvalidOperationException("User with given ID does not exist");
            }

            var multiAccount = await _context.Set<UserMultiAccount>().SingleOrDefaultAsync(x => x.RefLink == _command.RefLink);
            if (multiAccount == null)
            {
                throw new InvalidOperationException("Account with given reflink does not exist");
            }

            if (CheckIfReflinkBelongsToRequestedUser(multiAccount))
            {
                throw new InvalidOperationException("Given reflink belongs to the requested user account");
            }

            if (!CheckIfUserPaidMembershipsFee(userAccount))
            {
                throw new InvalidOperationException("The main account did not pay the membership's fee yet");
            }

            if (await CheckIfAllMultiAccountsAreInMatrixPositions(userAccount))
            {
                throw new InvalidOperationException("Not all user multi accounts are available in matrix positions");
            }
        }

        private static bool CheckIfUserPaidMembershipsFee(UserAccountData userAccount)
        {
            return userAccount.IsMembershipFeePaid;
        }

        private bool CheckIfReflinkBelongsToRequestedUser(UserMultiAccount multiAccount)
        {
            return multiAccount.UserAccountDataId == _command.UserAccountId;
        }

        private async Task<bool> CheckIfAllMultiAccountsAreInMatrixPositions(UserAccountData userAccount)
        {
            var userMultiAccountIds = userAccount.UserMultiAccounts
                .Select(x => x.Id)
                .ToList();

            var allUserMultiAccountsInMatrixPositions = await _context.Set<MatrixPosition>()
                .Where(x => userMultiAccountIds.Contains(x.Id))
                .ToListAsync();

            return allUserMultiAccountsInMatrixPositions.All(x => userMultiAccountIds.Contains(x.UserMultiAccountId));
        }

        private Task<UserMultiAccount> CreateMultiAccount()
        {
            throw new NotImplementedException();
        }
    }
}
