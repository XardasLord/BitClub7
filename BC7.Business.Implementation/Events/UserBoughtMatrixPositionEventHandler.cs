using System.Threading;
using System.Threading.Tasks;
using BC7.Business.Helpers;
using BC7.Database;
using BC7.Repository;
using MediatR;

namespace BC7.Business.Implementation.Events
{
    public class UserBoughtMatrixPositionEventHandler : INotificationHandler<UserBoughtMatrixPositionEvent>
    {
        private readonly IBitClub7Context _context;
        private readonly IUserMultiAccountRepository _userMultiAccountRepository;
        private readonly IReflinkHelper _reflinkHelper;

        public UserBoughtMatrixPositionEventHandler(IBitClub7Context context, IUserMultiAccountRepository userMultiAccountRepository, IReflinkHelper reflinkHelper)
        {
            _context = context;
            _userMultiAccountRepository = userMultiAccountRepository;
            _reflinkHelper = reflinkHelper;
        }

        public async Task Handle(UserBoughtMatrixPositionEvent notification, CancellationToken cancellationToken = default(CancellationToken))
        {
            var multiAccount = await _userMultiAccountRepository.GetAsync(notification.MultiAccountId);
            multiAccount.RefLink = _reflinkHelper.GenerateReflink();

            // TODO: Update in repository
            await _context.SaveChangesAsync();
        }
    }
}
