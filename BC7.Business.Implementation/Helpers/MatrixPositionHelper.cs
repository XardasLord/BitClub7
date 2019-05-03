using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BC7.Business.Helpers;
using BC7.Database;
using BC7.Domain;
using BC7.Infrastructure.CustomExceptions;
using BC7.Repository;
using Microsoft.EntityFrameworkCore;

namespace BC7.Business.Implementation.Helpers
{
    public class MatrixPositionHelper : IMatrixPositionHelper
    {
        private readonly IBitClub7Context _context;
        private readonly IMatrixPositionRepository _matrixPositionRepository;

        public MatrixPositionHelper(IBitClub7Context context, IMatrixPositionRepository matrixPositionRepository)
        {
            _context = context;
            _matrixPositionRepository = matrixPositionRepository;
        }

        public bool CheckIfAnyAccountExistInMatrix(IEnumerable<MatrixPosition> matrix, IEnumerable<Guid> accountIds)
        {
            return matrix.Any(x => x.UserMultiAccountId != null && accountIds.Contains(x.UserMultiAccountId.Value));
        }

        public bool CheckIfMatrixHasEmptySpace(IEnumerable<MatrixPosition> matrix)
        {
            return matrix.Any(x => x.UserMultiAccountId == null);
        }

        public async Task<MatrixPosition> FindTheNearestEmptyPositionFromGivenAccountWhereInParentsMatrixThereIsNoAnyMultiAccountAsync(
            Guid userMultiAccountId, IReadOnlyCollection<Guid> multiAccountIds, int matrixLevel = 0)
        {
            var userMatrixPosition = await _matrixPositionRepository.GetPositionForAccountAtLevelAsync(userMultiAccountId, matrixLevel);
            if (userMatrixPosition is null) throw new ArgumentNullException(nameof(userMatrixPosition));

            var allEmptyPositions = await _context.Set<MatrixPosition>()
                .Where(x => x.Left >= userMatrixPosition.Left)
                .Where(x => x.Right <= userMatrixPosition.Right)
                .Where(x => x.DepthLevel >= userMatrixPosition.DepthLevel)
                .Where(x => x.UserMultiAccountId == null)
                .ToListAsync();
            
            foreach (var emptyPosition in allEmptyPositions)
            {
                var matrix = await GetMatrixPositionWhereGivenPositionIsInLineBAsync(emptyPosition, matrixLevel);
                if (!CheckIfAnyAccountExistInMatrix(matrix, multiAccountIds))
                {
                    return emptyPosition;
                }
            }

            return null;
        }

        public async Task<MatrixPosition> FindHighestAdminPositionAsync(Guid userMultiAccountId, int matrixLevel)
        {
            var position = await _matrixPositionRepository.GetPositionForAccountAtLevelAsync(userMultiAccountId, matrixLevel);
            if (position is null)
            {
                throw new ValidationException($"There is no user multi account position - {userMultiAccountId} - in matrix lvl: {matrixLevel}");
            }

            return await _context.Set<MatrixPosition>()
                .Where(x => x.Left <= position.Left)
                .Where(x => x.Right >= position.Right)
                .Where(x => x.DepthLevel == 2) // Czy możemy zawsze przyjąć, że na DepthLevel = 2 będzie admin ?
                .SingleOrDefaultAsync();
        }

        public Task<MatrixPosition> FindEmptyPositionForHighestAdminAsync(int matrixLevel)
        {
            return _context.Set<MatrixPosition>()
                .Where(x => x.MatrixLevel == matrixLevel)
                .Where(x => x.UserMultiAccountId == null)
                .Where(x => x.DepthLevel == 2)
                .FirstOrDefaultAsync();
        }

        private async Task<IEnumerable<MatrixPosition>> GetMatrixPositionWhereGivenPositionIsInLineBAsync(MatrixPosition matrixPosition, int matrixLevel = 0)
        {
            var parentPosition = await _matrixPositionRepository.GetTopParentAsync(matrixPosition, matrixLevel);
            var parentsMatrix = await _matrixPositionRepository.GetMatrixAsync(parentPosition, matrixLevel);

            return parentsMatrix;
        }
    }
}
