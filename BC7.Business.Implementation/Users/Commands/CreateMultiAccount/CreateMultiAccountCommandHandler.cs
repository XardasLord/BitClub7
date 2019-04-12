using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BC7.Business.Helpers;
using BC7.Common.Extensions;
using BC7.Database;
using BC7.Domain;
using BC7.Infrastructure.CustomExceptions;
using BC7.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BC7.Business.Implementation.Users.Commands.CreateMultiAccount
{
    public class CreateMultiAccountCommandHandler : IRequestHandler<CreateMultiAccountCommand, Guid>
    {
        private CreateMultiAccountCommand _command;
        private readonly IBitClub7Context _context;
        private readonly IUserAccountDataRepository _userAccountDataRepository;
        private readonly IUserMultiAccountRepository _userMultiAccountRepository;
        private readonly IMatrixPositionRepository _matrixPositionRepository;
        private readonly IUserMultiAccountHelper _userMultiAccountHelper;
        private readonly IMatrixPositionHelper _matrixPositionHelper;

        public CreateMultiAccountCommandHandler(
            IBitClub7Context context, 
            IUserAccountDataRepository userAccountDataRepository,
            IUserMultiAccountRepository userMultiAccountRepository,
            IMatrixPositionRepository matrixPositionRepository,
            IUserMultiAccountHelper userMultiAccountHelper, 
            IMatrixPositionHelper matrixPositionHelper)
        {
            _context = context;
            _userAccountDataRepository = userAccountDataRepository;
            _userMultiAccountRepository = userMultiAccountRepository;
            _matrixPositionRepository = matrixPositionRepository;
            _userMultiAccountHelper = userMultiAccountHelper;
            _matrixPositionHelper = matrixPositionHelper;
        }

        public async Task<Guid> Handle(CreateMultiAccountCommand command, CancellationToken cancellationToken = default(CancellationToken))
        {
            _command = command;

            await ValidateIfMultiAccountCanBeCreated();

            var userMultiAccountInviting = await _userMultiAccountRepository.GetByReflinkAsync(_command.RefLink);
            var multiAccountName = await _userMultiAccountHelper.GenerateNextMultiAccountName(_command.UserAccountId);

            var userMultiAccount = new UserMultiAccount
            (
                id: Guid.NewGuid(),
                userAccountDataId: _command.UserAccountId,
                sponsorId: userMultiAccountInviting.Id,
                multiAccountName: multiAccountName
            );

            await _userMultiAccountRepository.CreateAsync(userMultiAccount);

            return userMultiAccount.Id;
        }

        private async Task ValidateIfMultiAccountCanBeCreated()
        {
            var userAccount = await _userAccountDataRepository.GetAsync(_command.UserAccountId);
            if (userAccount is null)
            {
                throw new AccountNotFoundException("User with given ID does not exist");
            }

            var invitingMultiAccount = await _userMultiAccountRepository.GetByReflinkAsync(_command.RefLink);
            if (invitingMultiAccount is null)
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

            // TODO: How to verify the reflink user's matrix level? Is it 0, 1...9?
            var invitingUserMatrixAccounts = await _matrixPositionRepository.GetMatrixForGivenMultiAccountAsync(invitingMultiAccount.Id);

            if (_matrixPositionHelper.CheckIfAnyAccountExistInMatrix(invitingUserMatrixAccounts, userMultiAccountIds))
            {
                // TODO: Probably we should find a random sponsor here instead of throwing an error?
                throw new ValidationException("You cannot have position in matrix with any of your other multi account");
            }

            if (!_matrixPositionHelper.CheckIfMatrixHasEmptySpace(invitingUserMatrixAccounts))
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
                .Where(x => userMultiAccountIds.Contains(x.UserMultiAccountId.Value))
                .Select(x => x.UserMultiAccountId.Value)
                .ToListAsync();

            return allUserMultiAccountsInMatrixPositions.ContainsAll(userMultiAccountIds);
        }
    }
}
