using System;
using System.Threading.Tasks;
using BC7.Entity;

namespace BC7.Repository
{
    public interface IMatrixPositionRepository
    {
        Task<MatrixPosition> GetAsync(Guid id);
    }
}
