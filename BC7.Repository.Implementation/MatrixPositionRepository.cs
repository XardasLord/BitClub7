using System;
using System.Collections.Generic;
using System.Linq;
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

        public Task<MatrixPosition> GetPositionForAccountAtLevel(Guid userMultiAccountId, int matrixLevel = 0)
        {
            return _context.Set<MatrixPosition>()
                .Where(x => x.UserMultiAccountId == userMultiAccountId)
                .Where(x => x.MatrixLevel == matrixLevel)
                .SingleOrDefaultAsync(); // Cycles available later
        }

        public async Task<IEnumerable<MatrixPosition>> GetMatrixAsync(Guid userMultiAccountId, int matrixLevel = 0)
        {
            var userMatrixPosition = await GetPositionForAccountAtLevel(userMultiAccountId, matrixLevel);
            if (userMatrixPosition == null)
            {
                return null;
            }

            var matrixAccounts = await _context.Set<MatrixPosition>()
                .Where(x => x.Left >= userMatrixPosition.Left)
                .Where(x => x.Right <= userMatrixPosition.Right)
                .Where(x => x.DepthLevel >= userMatrixPosition.DepthLevel)
                .Where(x => x.DepthLevel <= userMatrixPosition.DepthLevel + 2) // Each matrix has 2 depth level
                .ToListAsync();

            return matrixAccounts;
        }

        public async Task UpdateAsync(MatrixPosition matrixPosition)
        {
            _context.Set<MatrixPosition>().Attach(matrixPosition);
            await _context.SaveChangesAsync();
        }
    }
}
