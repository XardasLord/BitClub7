using System.Threading;
using System.Threading.Tasks;
using BC7.Infrastructure.CustomExceptions;
using BC7.Repository;
using MediatR;

namespace BC7.Business.Implementation.Users.Commands.ChangeAvatar
{
    public class ChangeAvatarCommandHandler : IRequestHandler<ChangeAvatarCommand>
    {
        private readonly IUserAccountDataRepository _userAccountDataRepository;

        public ChangeAvatarCommandHandler(IUserAccountDataRepository userAccountDataRepository)
        {
            _userAccountDataRepository = userAccountDataRepository;
        }

        public async Task<Unit> Handle(ChangeAvatarCommand request, CancellationToken cancellationToken = default(CancellationToken))
        {
            var user = await _userAccountDataRepository.GetAsync(request.UserAccountDataId);
            if (user is null)
            {
                throw new AccountNotFoundException($"User with given ID cannot be found - {request.UserAccountDataId}");
            }

            user.ChangeAvatar(request.AvatarPath);

            await _userAccountDataRepository.UpdateAsync(user);

            return Unit.Value;
        }
    }
}