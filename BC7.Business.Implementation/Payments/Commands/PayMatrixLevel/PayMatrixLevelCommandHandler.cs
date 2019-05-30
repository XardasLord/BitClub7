using System;
using System.Threading;
using System.Threading.Tasks;
using BC7.Domain;
using BC7.Infrastructure.CustomExceptions;
using BC7.Infrastructure.Payments;
using BC7.Infrastructure.Payments.BodyModels;
using BC7.Repository;
using MediatR;

namespace BC7.Business.Implementation.Payments.Commands.PayMatrixLevel
{
    public class PayMatrixLevelCommandHandler : IRequestHandler<PayMatrixLevelCommand, string>
    {
        private readonly IBitBayPayFacade _bitBayPayFacade;
        private readonly IUserMultiAccountRepository _userAccountDataRepository;
        private readonly IPaymentHistoryRepository _paymentHistoryRepository;

        public PayMatrixLevelCommandHandler(IBitBayPayFacade bitBayPayFacade, IUserMultiAccountRepository userAccountDataRepository, IPaymentHistoryRepository paymentHistoryRepository)
        {
            _bitBayPayFacade = bitBayPayFacade;
            _userAccountDataRepository = userAccountDataRepository;
            _paymentHistoryRepository = paymentHistoryRepository;
        }

        public async Task<string> Handle(PayMatrixLevelCommand command, CancellationToken cancellationToken = default(CancellationToken))
        {
            await ValidateCommand(command);

            var orderId = command.UserMultiAccountId;
            var paymentResponse = await _bitBayPayFacade.CreatePayment(orderId, command.Amount);

            ValidateResponse(paymentResponse);

            var paymentHistory = new PaymentHistory(
                id: Guid.NewGuid(),
                paymentId: paymentResponse.Data.PaymentId,
                orderId: orderId,
                amountToPay: command.Amount,
                paymentFor: PaymentForHelper.MatrixLevelPositionsDictionary[command.MatrixLevel]
            );
            await _paymentHistoryRepository.CreateAsync(paymentHistory);

            return paymentResponse.Data.Url;
        }

        private async Task ValidateCommand(PayMatrixLevelCommand command)
        {
            var user = await _userAccountDataRepository.GetAsync(command.UserMultiAccountId);
            if (user is null)
            {
                throw new ValidationException("User multi account with given ID was not found");
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
