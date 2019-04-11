using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BC7.Domain;

namespace BC7.Business.Helpers
{
    public interface IMatrixPositionHelper
    {
        bool CheckIfAnyAccountExistInMatrix(IEnumerable<MatrixPosition> matrix, IEnumerable<Guid> accountIds);
        bool CheckIfMatrixHasEmptySpace(IEnumerable<MatrixPosition> matrix);
        Task<MatrixPosition> FindTheNearestEmptyPositionFromGivenAccountAsync(Guid userMultiAccountId, int matrixLevel = 0);
        Task<IEnumerable<MatrixPosition>> GetMatrixPositionWhereGivenPositionIsInLineBAsync(MatrixPosition matrixPosition, int matrixLevel = 0);
    }
}
