using System;
using System.Collections.Generic;
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

        public Task<IEnumerable<MatrixPosition>> GetMatrixForUserMultiAccount(Guid userMultiAccountId)
        {
            throw new NotImplementedException();
        }
    }
}
