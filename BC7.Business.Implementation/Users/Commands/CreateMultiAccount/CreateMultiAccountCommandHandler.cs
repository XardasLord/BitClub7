using System;
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

            var multiAccount = await _userMultiAccountHelper.GetByReflink(_command.RefLink);
            if (multiAccount == null)
            {
                throw new AccountNotFoundException("Account with given reflink does not exist");
            }

            if (CheckIfReflinkBelongsToRequestedUser(multiAccount))
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

            // TODO: Check if multi account can be created from this reflink:
            // - multi account cannot be in its main account matrix (other way explanation from the system documentation below)
            //  "Nasze multikonto nie może znaleźć się w jakiejkolwiek naszej wykupionej matrycy.
            //   Można stworzyć multikonto jedynie zakładając je z reflinka osoby z poziomu B w naszej matrycy.
            //   Niedozwolone jest tworzenie multikont z reflinków osób z poziomu A którejkolwiek z naszych matryc."
            // Musimy tutaj sprawdzić, czy zakładane multikonto nie będzie się przypadkiem znajdować w którejkolwiek z istniejącej matrycy multikonta użytkownika
            // (np. w pętli po każdym multikoncie patrzymy 2 poziomy w dół czy tu nie będzie przypadkiem założone multikonto dla tego reflinka)
            // -----------------------------------------------------------------------------------------------------------------------------------------------------
            // -----------------------------------------------------------------------------------------------------------------------------------------------------
            // MÓJ POMYSŁ NA ALGORYTM PONIŻEJ :)
            // 1. pobranie pozycji multikonta w matrycy dla reflinka z którego się rejestrujemy
            // 2. bierzemy rodzica, który znajduje się 2 poziomy w górę od tego znalezionego konta w matrycy i pobieramy dla niego wszystkich potomków (2 poziomy w dół) - powinniśmy mieć w sumie 7 kont/pozycji
            // 3. sprawdzamy czy istnieje w tych siedmiu pozycjach ID multikonta należące do któregokolwiek z kont użytkownika, który chce założyć teraz multikonto
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

        private Task<UserMultiAccount> CreateMultiAccount()
        {
            return _userMultiAccountHelper.Create(_command.UserAccountId, _command.RefLink);
        }
    }
}
