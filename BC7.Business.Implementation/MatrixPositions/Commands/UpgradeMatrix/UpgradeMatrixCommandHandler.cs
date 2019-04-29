using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace BC7.Business.Implementation.MatrixPositions.Commands.UpgradeMatrix
{
    public class UpgradeMatrixCommandHandler : IRequestHandler<UpgradeMatrixCommand, Guid>
    {
        public UpgradeMatrixCommandHandler()
        {
        }

        public Task<Guid> Handle(UpgradeMatrixCommand request, CancellationToken cancellationToken)
        {
            // 1. Sprawdzenie czy ID multikonta istnieje
            // 2. Sprawdzenie czy użytkownik posiada wykupione miejsce w matrycy na levelu niżej
            // 3. Sprzedzenie czy użytkownik opłacił miejsce w matrycy, do której chce zrobić upgrade
            // 4. Sprawdzenie czy admin (znajdujący się na samej górze matrycy o level niższej) ma wykupione już miejsce w matrycy, do której chcemy zrobić upgrade

            throw new NotImplementedException();
        }
    }
}
