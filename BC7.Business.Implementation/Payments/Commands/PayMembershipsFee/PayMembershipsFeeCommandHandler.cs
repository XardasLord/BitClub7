using System;
using System.Threading;
using System.Threading.Tasks;
using BC7.Infrastructure.CustomExceptions;
using BC7.Infrastructure.Payments;
using BC7.Infrastructure.Payments.BodyModels;
using BC7.Repository;
using MediatR;

namespace BC7.Business.Implementation.Payments.Commands.PayMembershipsFee
{
    public class PayMembershipsFeeCommandHandler : IRequestHandler<PayMembershipsFeeCommand, string>
    {
        private readonly IBitBayPayFacade _bitBayPayFacade;
        private readonly IUserAccountDataRepository _userAccountDataRepository;

        public PayMembershipsFeeCommandHandler(IBitBayPayFacade bitBayPayFacade, IUserAccountDataRepository userAccountDataRepository)
        {
            _bitBayPayFacade = bitBayPayFacade;
            _userAccountDataRepository = userAccountDataRepository;
        }

        public async Task<string> Handle(PayMembershipsFeeCommand command, CancellationToken cancellationToken = default(CancellationToken))
        {
            await ValidateCommand(command);

            var orderId = Guid.NewGuid();
            var paymentResponse = await _bitBayPayFacade.CreatePayment(orderId, command.Amount);

            ValidateResponse(paymentResponse);

            // TODO: Save paymentId and orderId with UserAccountDataId in DB 
            // TODO: Create entity to store payment histories

            return paymentResponse.Data.Url;
        }

        private async Task ValidateCommand(PayMembershipsFeeCommand command)
        {
            var user = await _userAccountDataRepository.GetAsync(command.UserAccountDataId);
            if (user is null)
            {
                throw new ValidationException("User with given ID was not found");
            }
        }

        private static void ValidateResponse(CreatePaymentResponse response)
        {
            if (response.Status != "Ok")
            {
                throw new ValidationException("Status of the BitBayPay API call has failed");
            }
        }
    }
}