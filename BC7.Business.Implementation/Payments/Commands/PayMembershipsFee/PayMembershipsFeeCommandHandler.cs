using System;
using System.Threading;
using System.Threading.Tasks;
using BC7.Infrastructure.CustomExceptions;
using BC7.Infrastructure.Payments;
using BC7.Repository;
using MediatR;

namespace BC7.Business.Implementation.Payments.Commands.PayMembershipsFee
{
    public class PayMembershipsFeeCommandHandler : IRequestHandler<PayMembershipsFeeCommand>
    {
        private readonly IBitBayPayFacade _bitBayPayFacade;
        private readonly IUserAccountDataRepository _userAccountDataRepository;

        public PayMembershipsFeeCommandHandler(IBitBayPayFacade bitBayPayFacade, IUserAccountDataRepository userAccountDataRepository)
        {
            _bitBayPayFacade = bitBayPayFacade;
            _userAccountDataRepository = userAccountDataRepository;
        }

        public async Task<Unit> Handle(PayMembershipsFeeCommand command, CancellationToken cancellationToken = default(CancellationToken))
        {
            await ValidateCommand(command);

            //TODO: Tmp store orderId in DB with UserAccountDataId
            var orderId = Guid.NewGuid();

            await _bitBayPayFacade.CreatePayment(orderId, command.Amount);

            //TODO: Logic after payment call

            throw new NotImplementedException();
        }

        private async Task ValidateCommand(PayMembershipsFeeCommand command)
        {
            var user = await _userAccountDataRepository.GetAsync(command.UserAccountDataId);
            if (user is null)
            {
                throw new ValidationException("User with given ID not found");
            }
        }
    }
}