using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BC7.Business.Models;
using BC7.Repository;
using MediatR;

namespace BC7.Business.Implementation.MatrixPositions.Requests.GetMatrix
{
    public class GetMatrixRequestHandler : IRequestHandler<GetMatrixRequest, MatrixViewModel>
    {
        private readonly IMatrixPositionRepository _matrixPositionRepository;
        private readonly IUserMultiAccountRepository _multiAccountRepository;
        private readonly IMapper _mapper;

        public GetMatrixRequestHandler(IMatrixPositionRepository matrixPositionRepository, IUserMultiAccountRepository multiAccountRepository, IMapper mapper)
        {
            _matrixPositionRepository = matrixPositionRepository;
            _multiAccountRepository = multiAccountRepository;
            _mapper = mapper;
        }

        public async Task<MatrixViewModel> Handle(GetMatrixRequest request, CancellationToken cancellationToken = default(CancellationToken))
        {
            var matrixPosition = await _matrixPositionRepository.GetAsync(request.MatrixPositionId);
            var matrix = await _matrixPositionRepository.GetMatrixAsync(matrixPosition);

            var matrixModel = _mapper.Map<IEnumerable<MatrixPositionModel>>(matrix);

            // TODO: Refactor in the future
            var matrixModelsList = matrixModel.ToList();
            foreach (var matrixPositionModel in matrixModelsList)
            {
                var tmpPosition = matrix.Single(x => x.Id == matrixPositionModel.Id);

                if (tmpPosition.UserMultiAccountId.HasValue)
                {
                    var userMultiAccount = await _multiAccountRepository.GetAsync(tmpPosition.UserMultiAccountId.Value);
                    matrixPositionModel.MultiAccountName = userMultiAccount.MultiAccountName;
                }
                else
                {
                    matrixPositionModel.MultiAccountName = null;
                }

                if (tmpPosition.Id == request.MatrixPositionId)
                {
                    // It's the owner matrix position, so for frontend I set ParentId value to null
                    matrixPositionModel.ParentId = null;
                }
            }

            return new MatrixViewModel
            {
                Matrix = matrixModelsList
            };
        }
    }
}