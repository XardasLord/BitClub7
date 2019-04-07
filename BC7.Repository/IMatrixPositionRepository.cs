using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BC7.Entity;

namespace BC7.Repository
{
    public interface IMatrixPositionRepository
    {
        Task<MatrixPosition> GetAsync(Guid id);
        Task<IEnumerable<MatrixPosition>> GetMatrix(Guid userMultiAccountId, int matrixLevel = 0);
    }
}
