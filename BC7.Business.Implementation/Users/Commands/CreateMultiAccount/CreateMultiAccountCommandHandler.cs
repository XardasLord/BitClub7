using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BC7.Business.Helpers;
using BC7.Common.Extensions;
using BC7.Database;
using BC7.Entity;
using BC7.Infrastructure.CustomExceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BC7.Business.Implementation.Users.Commands.CreateMultiAccount
{
    public class CreateMultiAccountCommandHandler : IRequestHandler<CreateMultiAccountCommand, Guid>
    {
        private CreateMultiAccountCommand _command;
        private readonly IBitClub7Context _context;
        private readonly IUserMultiAccountHelper _userMultiAccountHelper;

        public CreateMultiAccountCommandHandler(IBitClub7Context context, IUserMultiAccountHelper userMultiAccountHelper)
        {
            _context = context;
            _userMultiAccountHelper = userMultiAccountHelper;
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
            var userAccount = await _context.Set<UserAccountData>()
                .Include(x => x.UserMultiAccounts)
                .SingleOrDefaultAsync(x => x.Id == _command.UserAccountId);
            if (userAccount == null)
            {
                throw new AccountNotFoundException("User with given ID does not exist");
            }

            var invitingMultiAccount = await _userMultiAccountHelper.GetByReflink(_command.RefLink);
            if (invitingMultiAccount == null)
            {
                throw new AccountNotFoundException("Account with given reflink does not exist");
            }

            if (CheckIfReflinkBelongsToRequestedUser(invitingMultiAccount))
            {
                throw new ValidationException("Given reflink belongs to the requested user account");
            }

#warning this validation has to be uncomment in the ETAP 1
            //if (!CheckIfUserPaidMembershipsFee(userAccount))
            //{
            //    throw new InvalidOperationException("The main account did not pay the membership's fee yet");
            //}

            if (!await CheckIfAllMultiAccountsAreInMatrixPositions(userAccount))
            {
                throw new ValidationException("Not all user multi accounts are available in matrix positions");
            }

            if (userAccount.UserMultiAccounts.Count > 20)
            {
                throw new ValidationException("You cannot have more than 20 multi accounts attached to the main account");
            }

            var invitingMatrixAccounts = await GetMatrixForUserAccount(invitingMultiAccount);
            
            if (CheckIfAnyOfUserMultiAccountsExistInGivenMatrix(userAccount, invitingMatrixAccounts))
            {
                // TODO: Probably we should find a random sponsor here instead of throwing an error?
                throw new ValidationException("You cannot have position in matrix with any of your other multi account");
            }
            
            if (!CheckIfEmptySpaceExistInMatrix(invitingMatrixAccounts))
            {
                // TODO: Probably we should find a random sponsor here instead of throwing an error?
                throw new ValidationException("Matrix is full for this reflink");
            }
        }

        private async Task<IEnumerable<MatrixPosition>> GetMatrixForUserAccount(UserMultiAccount invitingMultiAccount)
        {
            var invitingMatrixPosition = await _context.Set<MatrixPosition>()
                .Where(x => x.UserMultiAccountId == invitingMultiAccount.Id).SingleAsync();
            
            var invitingMatrixAccounts = await _context.Set<MatrixPosition>()
                .Where(x => x.Left >= invitingMatrixPosition.Left)
                .Where(x => x.Right <= invitingMatrixPosition.Right)
                .Where(x => x.DepthLevel >= invitingMatrixPosition.DepthLevel)
                .Where(x => x.DepthLevel <= invitingMatrixPosition.DepthLevel + 2)
                .ToListAsync();

            return invitingMatrixAccounts;
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
            // TODO: Move it to helper
            var userMultiAccountIds = userAccount.UserMultiAccounts
                .Select(x => x.Id)
                .ToList();

            var allUserMultiAccountsInMatrixPositions = await _context.Set<MatrixPosition>()
                .Where(x => userMultiAccountIds.Contains(x.Id))
                .Select(x => x.UserMultiAccountId.Value)
                .ToListAsync();

            return allUserMultiAccountsInMatrixPositions.ContainsAll(userMultiAccountIds);
        }
        
        private static bool CheckIfEmptySpaceExistInMatrix(IEnumerable<MatrixPosition> invitatorMatrixPositionMatrixAccounts)
        {
            return invitatorMatrixPositionMatrixAccounts.Any(x => x.UserMultiAccountId == null);
        }

        private static bool CheckIfAnyOfUserMultiAccountsExistInGivenMatrix(UserAccountData userAccount, IEnumerable<MatrixPosition> invitatorMatrixPositionMatrixAccounts)
        {
            var userMultiAccountIds = userAccount.UserMultiAccounts
                            .Select(x => x.Id)
                            .ToList();

            return invitatorMatrixPositionMatrixAccounts.Any(x => userMultiAccountIds.Contains(x.Id));
        }

        private Task<UserMultiAccount> CreateMultiAccount()
        {
            return _userMultiAccountHelper.Create(_command.UserAccountId, _command.RefLink);
        }
    }
}
