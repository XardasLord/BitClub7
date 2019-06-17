using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BC7.Business.Models;
using BC7.Infrastructure.CustomExceptions;
using BC7.Repository;
using MediatR;

namespace BC7.Business.Implementation.Users.Requests.GetUser
{
    public class GetUserRequestHandler : IRequestHandler<GetUserRequest, UserAccountDataModel>
    {
        private readonly IUserAccountDataRepository _userAccountDataRepository;
        private readonly IMapper _mapper;

        public GetUserRequestHandler(IUserAccountDataRepository userAccountDataRepository, IMapper mapper)
        {
            _userAccountDataRepository = userAccountDataRepository;
            _mapper = mapper;
        }

        public async Task<UserAccountDataModel> Handle(GetUserRequest request, CancellationToken cancellationToken = default(CancellationToken))
        {
            ValidateRequest(request);

            var user = await _userAccountDataRepository.GetAsync(request.UserId);

            var userModel = _mapper.Map<UserAccountDataModel>(user);

            return userModel;
        }

        private static void ValidateRequest(GetUserRequest request)
        {
            if (request is null)
            {
                throw new ValidationException("Request has not been provided.");
            }
            if (request.UserId == Guid.Empty)
            {
                throw new ValidationException("UserId cannot be empty.");
            }
        }
    }
}