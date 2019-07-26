using System;
using System.Threading;
using System.Threading.Tasks;
using BC7.Domain;
using BC7.Infrastructure.CustomExceptions;
using BC7.Infrastructure.Payments;
using BC7.Infrastructure.Payments.BitBayPay;
using BC7.Infrastructure.Payments.BitBayPay.BodyModels;
using BC7.Repository;
using MediatR;

namespace BC7.Business.Implementation.Payments.Commands.PayMembershipsFee
{
    public class PayMembershipsFeeCommandHandler : IRequestHandler<PayMembershipsFeeCommand, string>
    {
        private readonly IBitBayPayFacade _bitBayPayFacade;
        private readonly IUserAccountDataRepository _userAccountDataRepository;
        private readonly IPaymentHistoryRepository _paymentHistoryRepository;

        public PayMembershipsFeeCommandHandler(IBitBayPayFacade bitBayPayFacade, IUserAccountDataRepository userAccountDataRepository, IPaymentHistoryRepository paymentHistoryRepository)
        {
            _bitBayPayFacade = bitBayPayFacade;
            _userAccountDataRepository = userAccountDataRepository;
            _paymentHistoryRepository = paymentHistoryRepository;
        }

        public async Task<string> Handle(PayMembershipsFeeCommand command, CancellationToken cancellationToken = default(CancellationToken))
        {
            await ValidateCommand(command);

            var orderId = command.UserAccountDataId;
            var paymentResponse = await _bitBayPayFacade.CreatePayment(orderId, command.Amount);

            ValidateResponse(paymentResponse);
            
            var paymentHistory = new PaymentHistory(
                id: Guid.NewGuid(),
                paymentId: paymentResponse.Data.PaymentId,
                orderId: orderId,
                amountToPay: command.Amount,
                paymentFor: PaymentForHelper.MembershipsFee
            );
            await _paymentHistoryRepository.CreateAsync(paymentHistory);

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