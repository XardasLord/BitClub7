using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BC7.Business.Helpers;
using BC7.Database;
using BC7.Entity;
using Microsoft.EntityFrameworkCore;

namespace BC7.Business.Implementation.Helpers
{
    public class MatrixPositionHelper : IMatrixPositionHelper
    {
        private const int MatrixDepthLevel = 2;

        private readonly IBitClub7Context _context;

        public MatrixPositionHelper(IBitClub7Context context)
        {
            _context = context;
        }

#warning Move to repository
        public async Task<IEnumerable<MatrixPosition>> GetMatrix(Guid userMultiAccountId, int matrixLevel = 0)
        {
            var userMatrixPosition = await _context.Set<MatrixPosition>()
                .Where(x => x.UserMultiAccountId == userMultiAccountId)
                .Where(x => x.MatrixLevel == matrixLevel)
                .SingleOrDefaultAsync();

            if (userMatrixPosition == null)
            {
                return null;
            }

            var matrixAccounts = await _context.Set<MatrixPosition>()
                .Where(x => x.Left >= userMatrixPosition.Left)
                .Where(x => x.Right <= userMatrixPosition.Right)
                .Where(x => x.DepthLevel >= userMatrixPosition.DepthLevel)
                .Where(x => x.DepthLevel <= userMatrixPosition.DepthLevel + MatrixDepthLevel)
                .ToListAsync();

            return matrixAccounts;
        }

        public bool CheckIfAnyAccountExistInMatrix(IEnumerable<MatrixPosition> matrix, IEnumerable<Guid> accountIds)
        {
            return matrix.Any(x => accountIds.Contains(x.Id));
        }

        public bool CheckIfMatrixHasEmptySpace(IEnumerable<MatrixPosition> matrix)
        {
            return matrix.Any(x => x.UserMultiAccountId == null);
        }
    }
}
