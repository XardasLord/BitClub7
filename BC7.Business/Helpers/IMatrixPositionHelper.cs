using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BC7.Entity;

namespace BC7.Business.Helpers
{
    public interface IMatrixPositionHelper
    {
        bool CheckIfAnyAccountExistInMatrix(IEnumerable<MatrixPosition> matrix, IEnumerable<Guid> accountIds);
        bool CheckIfMatrixHasEmptySpace(IEnumerable<MatrixPosition> matrix);
        Task<MatrixPosition> FindTheNearestEmptyPositionFromGivenAccount(Guid userMultiAccountId, int matrixLevel = 0);
    }
}
