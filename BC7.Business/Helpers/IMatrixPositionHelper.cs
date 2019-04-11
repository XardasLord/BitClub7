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
        Task<MatrixPosition> FindTheNearestEmptyPositionFromGivenAccountWhereInParentsMatrixThereIsNoAnyMultiAccountAsync(Guid userMultiAccountId, IReadOnlyCollection<Guid> multiAccountIds, int matrixLevel = 0);
    }
}
