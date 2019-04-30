using System;
using System.Threading;
using System.Threading.Tasks;
using BC7.Infrastructure.CustomExceptions;
using BC7.Repository;
using MediatR;

namespace BC7.Business.Implementation.MatrixPositions.Commands.UpgradeMatrix
{
    public class UpgradeMatrixCommandHandler : IRequestHandler<UpgradeMatrixCommand, Guid>
    {
        private readonly IUserMultiAccountRepository _userMultiAccountRepository;

        public UpgradeMatrixCommandHandler(IUserMultiAccountRepository userMultiAccountRepository)
        {
            _userMultiAccountRepository = userMultiAccountRepository;
        }

        public async Task<Guid> Handle(UpgradeMatrixCommand command, CancellationToken cancellationToken = default(CancellationToken))
        {
            // 1. Sprawdzenie czy ID multikonta istnieje
            var user = await _userMultiAccountRepository.GetAsync(command.UserMultiAccountId);
            if (user is null)
            {
                throw new ValidationException("User with given ID was not found");
            }

            // 2. Sprawdzenie czy użytkownik posiada wykupione miejsce w matrycy na levelu niżej
            // 3. Sprzedzenie czy użytkownik opłacił miejsce w matrycy, do której chce zrobić upgrade
            // 4. Sprawdzenie czy admin (znajdujący się na samej górze matrycy o level niższej) ma wykupione już miejsce w matrycy, do której chcemy zrobić upgrade

            throw new NotImplementedException();
        }
    }
}
