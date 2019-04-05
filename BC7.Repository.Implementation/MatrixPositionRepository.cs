using System;
using System.Threading.Tasks;
using BC7.Database;
using BC7.Entity;
using Microsoft.EntityFrameworkCore;

namespace BC7.Repository.Implementation
{
    public class MatrixPositionRepository : IMatrixPositionRepository
    {
        private readonly IBitClub7Context _context;

        public MatrixPositionRepository(IBitClub7Context context)
        {
            _context = context;
        }

        public Task<MatrixPosition> GetAsync(Guid id)
        {
            return _context.Set<MatrixPosition>().SingleOrDefaultAsync(x => x.Id == id);
        }
    }
}
