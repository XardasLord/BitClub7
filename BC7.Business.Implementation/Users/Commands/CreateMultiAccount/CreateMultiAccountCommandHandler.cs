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

            var sponsor = await GetSponsor();

            var multiAccountName = await _userMultiAccountHelper.GenerateNextMultiAccountName(_command.UserAccountId);
            var userMultiAccount = new UserMultiAccount
            (
                id: Guid.NewGuid(),
                userAccountDataId: _command.UserAccountId,
                sponsorId: sponsor.Id,
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

            var sponsor = await _userMultiAccountRepository.GetByReflinkAsync(_command.RefLink);
            if (sponsor is null)
            {
                throw new AccountNotFoundException("Account with given reflink does not exist");
            }

            if (CheckIfReflinkBelongsToRequestedUser(sponsor))
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
        }

        private async Task<UserMultiAccount> GetSponsor()
        {
            var userAccount = await _userAccountDataRepository.GetAsync(_command.UserAccountId);
            var userMultiAccountIds = userAccount.UserMultiAccounts.Select(x => x.Id).ToList();

            // TODO: How to verify the reflink user's matrix level? Is it 0, 1...9?
            var sponsor = await _userMultiAccountRepository.GetByReflinkAsync(_command.RefLink);
            var sponsorsMatrix = await _matrixPositionRepository.GetMatrixForGivenMultiAccountAsync(sponsor.Id);

            if (_matrixPositionHelper.CheckIfAnyAccountExistInMatrix(sponsorsMatrix, userMultiAccountIds) ||
                !_matrixPositionHelper.CheckIfMatrixHasEmptySpace(sponsorsMatrix))
            {
                // Nie ma miejsca w matrycy sponsora lub w matrycy sponsora jest któreś z kont użytkownika
                // Wtedy szukamy nowego sponsora
                var emptyMatrixPositionUnderAvailableSponsor = await _matrixPositionHelper
                    .FindTheNearestEmptyPositionFromGivenAccountWhereInParentsMatrixThereIsNoAnyMultiAccountAsync(sponsor.Id, userMultiAccountIds);

                if (emptyMatrixPositionUnderAvailableSponsor == null)
                {
                    throw new ValidationException("There is no space in this matrix level to put this account.");
                }

                sponsor = await _userMultiAccountRepository.GetAsync(emptyMatrixPositionUnderAvailableSponsor.ParentId.Value);
            }

            return sponsor;
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
