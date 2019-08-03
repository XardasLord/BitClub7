using System.Threading;
using System.Threading.Tasks;
using BC7.Infrastructure.CustomExceptions;
using BC7.Repository;
using MediatR;

namespace BC7.Business.Implementation.Users.Requests.GetInitiativeDescriptionForMultiAccount
{
    public class GetInitiativeDescriptionRequestHandler : IRequestHandler<GetInitiativeDescriptionRequest, GetInitiativeDescriptionViewModel>
    {
        private readonly IUserMultiAccountRepository _userMultiAccountRepository;

        public GetInitiativeDescriptionRequestHandler(IUserMultiAccountRepository userMultiAccountRepository)
        {
            _userMultiAccountRepository = userMultiAccountRepository;
        }

        public async Task<GetInitiativeDescriptionViewModel> Handle(GetInitiativeDescriptionRequest request, CancellationToken cancellationToken)
        {
            var multiAccount = await _userMultiAccountRepository.GetAsync(request.MultiAccountId);

            if (multiAccount is null)
            {
                throw new ValidationException($"Multi Account does not exist with given ID: {request.MultiAccountId}");
            }

            return new GetInitiativeDescriptionViewModel
            {
                Initiative = multiAccount.UserAccountData.InitiativeDescription,
                ProjectId = multiAccount.MultiAccountName,
                ProjectCode = multiAccount.RefLink,
                Avatar =  multiAccount.UserAccountData.Avatar
            };
        }
    }
}