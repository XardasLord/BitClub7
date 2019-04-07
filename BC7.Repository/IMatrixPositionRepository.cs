using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BC7.Entity;

namespace BC7.Repository
{
    public interface IMatrixPositionRepository
    {
        Task<MatrixPosition> GetAsync(Guid id);
        Task<IEnumerable<MatrixPosition>> GetMatrixAsync(Guid userMultiAccountId, int matrixLevel = 0);
        Task UpdateAsync(MatrixPosition matrixPosition);
    }
}
