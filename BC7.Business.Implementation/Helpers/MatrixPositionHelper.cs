using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BC7.Business.Helpers;
using BC7.Entity;
using BC7.Repository;

namespace BC7.Business.Implementation.Helpers
{
    public class MatrixPositionHelper : IMatrixPositionHelper
    {
        private readonly IMatrixPositionRepository _matrixPositionRepository;

        public MatrixPositionHelper(IMatrixPositionRepository matrixPositionRepository)
        {
            _matrixPositionRepository = matrixPositionRepository;
        }

        public bool CheckIfAnyAccountExistInMatrix(IEnumerable<MatrixPosition> matrix, IEnumerable<Guid> accountIds)
        {
            return matrix.Any(x => accountIds.Contains(x.Id));
        }

        public bool CheckIfMatrixHasEmptySpace(IEnumerable<MatrixPosition> matrix)
        {
            return matrix.Any(x => x.UserMultiAccountId == null);
        }

        public async Task<MatrixPosition> FindTheNearestEmptyPositionFromGivenAccount(Guid userMultiAccountId, int matrixLevel = 0)
        {
            var userMatrixPosition = await _matrixPositionRepository.GetPositionForAccountAtLevel(userMultiAccountId, matrixLevel);
            if (userMatrixPosition == null) throw new ArgumentNullException(nameof(userMatrixPosition));
            
            // TODO: Find the nearest empty position from the given account :)

            throw new NotImplementedException();
        }
    }
}
