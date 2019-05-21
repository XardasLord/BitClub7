using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BC7.Business.Models;
using BC7.Domain;

namespace BC7.Business.Helpers
{
    public interface IMatrixPositionHelper
    {
        bool CheckIfAnyAccountExistInMatrix(IEnumerable<MatrixPosition> matrix, IEnumerable<Guid> accountIds);
        bool CheckIfMatrixHasEmptySpace(IEnumerable<MatrixPosition> matrix);
        Task<MatrixPosition> GetTopParentAsync(MatrixPosition matrixPosition, int matrixLevel = 0);
        Task<MatrixPosition> FindTheNearestEmptyPositionFromGivenAccountWhereInParentsMatrixThereIsNoAnyMultiAccountAsync
            (Guid userMultiAccountId, IReadOnlyCollection<Guid> multiAccountIds, int matrixLevel, AdminStructureSide adminStructureSide = AdminStructureSide.Skipped);
        Task<MatrixPosition> FindHighestAdminPositionAsync(Guid userMultiAccountId, int matrixLevel);
        Task<AdminStructureSide> GetAdminStructureSide(Guid userMultiAccountId, int matrixLevel, MatrixPosition admin = null);
        Task<MatrixPosition> FindEmptyPositionForHighestAdminAsync(int matrixLevel);
        Task<MatrixPosition> GetPositionForAccountAtLevelAsync(Guid userMultiAccountId, int matrixLevel = 0);
        Task<IEnumerable<MatrixPosition>> GetMatrixForGivenMultiAccountAsync(Guid userMultiAccountId, int matrixLevel = 0);
        Task<string> GenerateMatrixStructureTreeFileDefinition(int matrixLevel);
    }
}
