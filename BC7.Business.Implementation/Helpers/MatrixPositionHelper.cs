using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BC7.Business.Helpers;
using BC7.Entity;

namespace BC7.Business.Implementation.Helpers
{
    public class MatrixPositionHelper : IMatrixPositionHelper
    {
        public MatrixPositionHelper()
        {
        }

        public bool CheckIfAnyAccountExistInMatrix(IEnumerable<MatrixPosition> matrix, IEnumerable<Guid> accountIds)
        {
            return matrix.Any(x => accountIds.Contains(x.Id));
        }

        public bool CheckIfMatrixHasEmptySpace(IEnumerable<MatrixPosition> matrix)
        {
            return matrix.Any(x => x.UserMultiAccountId == null);
        }

        public Task<MatrixPosition> FindTheNearestEmptyPosition(Guid userMultiAccountId, int matrixLevel = 0)
        {
            throw new NotImplementedException();
        }
    }
}
