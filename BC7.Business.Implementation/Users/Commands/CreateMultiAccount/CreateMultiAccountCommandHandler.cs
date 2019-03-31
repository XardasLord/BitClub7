﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BC7.Business.Helpers;
using BC7.Common.Extensions;
using BC7.Database;
using BC7.Entity;
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
            // TODO: Custom exception
            var userAccount = await _context.Set<UserAccountData>()
                .Include(x => x.UserMultiAccounts)
                .SingleOrDefaultAsync(x => x.Id == _command.UserAccountId);
            if (userAccount == null)
            {
                throw new InvalidOperationException("User with given ID does not exist");
            }

            var multiAccount = await _userMultiAccountHelper.GetByReflink(_command.RefLink);
            if (multiAccount == null)
            {
                throw new InvalidOperationException("Account with given reflink does not exist");
            }

            if (CheckIfReflinkBelongsToRequestedUser(multiAccount))
            {
                throw new InvalidOperationException("Given reflink belongs to the requested user account");
            }

#warning this validation has to be uncomment in the ETAP 1
            //if (!CheckIfUserPaidMembershipsFee(userAccount))
            //{
            //    throw new InvalidOperationException("The main account did not pay the membership's fee yet");
            //}

            if (!await CheckIfAllMultiAccountsAreInMatrixPositions(userAccount))
            {
                throw new InvalidOperationException("Not all user multi accounts are available in matrix positions");
            }

            if (userAccount.UserMultiAccounts.Count > 20)
            {
                throw new InvalidOperationException("You cannot have more than 20 multi accounts attached to the main account");
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
            // TODO: Move it to helper
            var userMultiAccountIds = userAccount.UserMultiAccounts
                .Select(x => x.Id)
                .ToList();

            var allUserMultiAccountsInMatrixPositions = await _context.Set<MatrixPosition>()
                .Where(x => userMultiAccountIds.Contains(x.Id))
                .Select(x => x.UserMultiAccountId)
                .ToListAsync();

            return allUserMultiAccountsInMatrixPositions.ContainsAll(userMultiAccountIds);
        }

        private Task<UserMultiAccount> CreateMultiAccount()
        {
            return _userMultiAccountHelper.Create(_command.UserAccountId, _command.RefLink);
        }
    }
}