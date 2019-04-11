using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BC7.Domain;

namespace BC7.Repository
{
    public interface IMatrixPositionRepository
    {
        Task<MatrixPosition> GetAsync(Guid id);
        Task<MatrixPosition> GetPositionForAccountAtLevelAsync(Guid userMultiAccountId, int matrixLevel = 0);
        Task<MatrixPosition> GetTopParentAsync(MatrixPosition matrixPosition, int matrixLevel = 0);
        Task<IEnumerable<MatrixPosition>> GetMatrixForGivenMultiAccountAsync(Guid userMultiAccountId, int matrixLevel = 0);
        Task UpdateAsync(MatrixPosition matrixPosition);
    }
}
