using System.Threading;
using System.Threading.Tasks;
using BC7.Domain;
using BC7.Infrastructure.CustomExceptions;
using BC7.Repository;
using BC7.Security;
using MediatR;

namespace BC7.Business.Implementation.Users.Commands.UpdateUser
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand>
    {
        private readonly IUserAccountDataRepository _userAccountDataRepository;

        public UpdateUserCommandHandler(IUserAccountDataRepository userAccountDataRepository)
        {
            _userAccountDataRepository = userAccountDataRepository;
        }

        public async Task<Unit> Handle(UpdateUserCommand command, CancellationToken cancellationToken = default(CancellationToken))
        {
            var userToUpdate = await _userAccountDataRepository.GetAsync(command.UserId);
            var requestedUser = await _userAccountDataRepository.GetAsync(command.RequestedUserId);

            ValidatePermission(userToUpdate, requestedUser, command.Role);

            userToUpdate.UpdateInformation(command.FirstName, command.LastName, command.Street, command.City, command.ZipCode, command.Country, command.BtcWalletAddress);
            userToUpdate.UpdateRole(command.Role);

            await _userAccountDataRepository.UpdateAsync(userToUpdate);

            return Unit.Value;
        }

        private static void ValidatePermission(UserAccountData userToUpdate, UserAccountData requestedUser, string newRole)
        {
            if (requestedUser.Id != userToUpdate.Id && requestedUser.Role != UserRolesHelper.Root)
            {
                throw new ValidationException("Only user itself or user with role `root` can edit information of other user");
            }

            if (requestedUser.Id == userToUpdate.Id && requestedUser.Role != UserRolesHelper.Root && userToUpdate.Role != newRole)
            {
                throw new ValidationException("You don't have permission to change your own role");
            }
        }
    }
}