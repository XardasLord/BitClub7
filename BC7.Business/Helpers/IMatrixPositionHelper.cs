using System;
using System.Collections.Generic;
using BC7.Entity;

namespace BC7.Business.Helpers
{
    public interface IMatrixPositionHelper
    {
        bool CheckIfAnyAccountExistInMatrix(IEnumerable<MatrixPosition> matrix, IEnumerable<Guid> accountIds);

        bool CheckIfMatrixHasEmptySpace(IEnumerable<MatrixPosition> matrix);
    }
}
