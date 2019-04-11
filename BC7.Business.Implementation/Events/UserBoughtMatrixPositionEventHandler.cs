using System.Threading;
using System.Threading.Tasks;
using BC7.Business.Helpers;
using BC7.Repository;
using MediatR;

namespace BC7.Business.Implementation.Events
{
    public class UserBoughtMatrixPositionEventHandler : INotificationHandler<UserBoughtMatrixPositionEvent>
    {
        private readonly IUserMultiAccountRepository _userMultiAccountRepository;
        private readonly IReflinkHelper _reflinkHelper;

        public UserBoughtMatrixPositionEventHandler(IUserMultiAccountRepository userMultiAccountRepository, IReflinkHelper reflinkHelper)
        {
            _userMultiAccountRepository = userMultiAccountRepository;
            _reflinkHelper = reflinkHelper;
        }

        public async Task Handle(UserBoughtMatrixPositionEvent notification, CancellationToken cancellationToken = default(CancellationToken))
        {
            var multiAccount = await _userMultiAccountRepository.GetAsync(notification.MultiAccountId);

            multiAccount.SetReflink(_reflinkHelper.GenerateReflink());

            await _userMultiAccountRepository.UpdateAsync(multiAccount);
        }
    }
}
