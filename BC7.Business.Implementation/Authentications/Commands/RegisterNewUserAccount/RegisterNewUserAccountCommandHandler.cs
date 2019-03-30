using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BC7.Database;
using MediatR;

namespace BC7.Business.Implementation.Authentications.Commands.RegisterNewUserAccount
{
    public class RegisterNewUserAccountCommandHandler : IRequestHandler<RegisterNewUserAccountCommand, Guid>
    {
        private readonly IBitClub7Context _context;
        private readonly IMapper _mapper;

        public RegisterNewUserAccountCommandHandler(IBitClub7Context context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public Task<Guid> Handle(RegisterNewUserAccountCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}