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
        private readonly IUserAccountDataHelper _userAccountDataHelper;
        private readonly IUserMultiAccountHelper _userMultiAccountHelper;
        private readonly IMatrixPositionHelper _matrixPositionHelper;

        public CreateMultiAccountCommandHandler(
            IBitClub7Context context, 
            IUserAccountDataHelper userAccountDataHelper,
            IUserMultiAccountHelper userMultiAccountHelper, 
            IMatrixPositionHelper matrixPositionHelper)
        {
            _context = context;
            _userAccountDataHelper = userAccountDataHelper;
            _userMultiAccountHelper = userMultiAccountHelper;
            _matrixPositionHelper = matrixPositionHelper;
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
            var userAccount = await _userAccountDataHelper.GetById(_command.UserAccountId);
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


            if (!CheckIfUserPaidMembershipsFee(userAccount))
            {
#warning this validation has to be uncomment in the ETAP 1
                //throw new InvalidOperationException("The main account did not pay the membership's fee yet");
            }

            var userMultiAccountIds = userAccount.UserMultiAccounts.Select(x => x.Id).ToList();
            if (!await CheckIfAllMultiAccountsAreInMatrixPositions(userMultiAccountIds))
            {
                throw new ValidationException("Not all user multi accounts are available in matrix positions");
            }

            if (userAccount.UserMultiAccounts.Count > 20)
            {
                throw new ValidationException("You cannot have more than 20 multi accounts attached to the main account");
            }

            var invitingUserMatrixAccounts = await _matrixPositionHelper.GetMatrixForUserMultiAccount(invitingMultiAccount.Id);

            if (CheckIfAnyOfUserMultiAccountsExistInGivenMatrix(invitingUserMatrixAccounts, userMultiAccountIds))
            {
                // TODO: Probably we should find a random sponsor here instead of throwing an error?
                throw new ValidationException("You cannot have position in matrix with any of your other multi account");
            }

            if (!CheckIfEmptySpaceExistInMatrix(invitingUserMatrixAccounts))
            {
                // TODO: Probably we should find a random sponsor here instead of throwing an error?
                throw new ValidationException("Matrix is full for this reflink");
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

        private async Task<bool> CheckIfAllMultiAccountsAreInMatrixPositions(IEnumerable<Guid> userMultiAccountIds)
        {
            // TODO: Move it to helper
            var allUserMultiAccountsInMatrixPositions = await _context.Set<MatrixPosition>()
                .Where(x => userMultiAccountIds.Contains(x.Id))
                .Select(x => x.UserMultiAccountId.Value)
                .ToListAsync();

            return allUserMultiAccountsInMatrixPositions.ContainsAll(userMultiAccountIds);
        }

        private bool CheckIfAnyOfUserMultiAccountsExistInGivenMatrix(IEnumerable<MatrixPosition> invitingMatrixAccounts, IEnumerable<Guid> userMultiAccountIds)
        {
            return _matrixPositionHelper.CheckIfAnyAccountExistInMatrix(invitingMatrixAccounts, userMultiAccountIds);
        }

        private bool CheckIfEmptySpaceExistInMatrix(IEnumerable<MatrixPosition> invitingUserMatrix)
        {
            return _matrixPositionHelper.CheckIfMatrixHasEmptySpace(invitingUserMatrix);
        }

        private Task<UserMultiAccount> CreateMultiAccount()
        {
            return _userMultiAccountHelper.Create(_command.UserAccountId, _command.RefLink);
        }
    }
}
