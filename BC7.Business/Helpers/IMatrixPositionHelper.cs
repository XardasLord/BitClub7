using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BC7.Entity;

namespace BC7.Business.Helpers
{
    public interface IMatrixPositionHelper
    {
        Task<IEnumerable<MatrixPosition>> GetMatrixForUserMultiAccount(Guid userMultiAccountId);
    }
}
