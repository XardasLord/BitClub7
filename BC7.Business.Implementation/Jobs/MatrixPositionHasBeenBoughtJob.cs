using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BC7.Database;
using BC7.Domain;
using BC7.Infrastructure.Implementation.Hangfire;
using BC7.Repository;
using Hangfire.Console;
using Hangfire.Server;
using Z.EntityFramework.Plus;

namespace BC7.Business.Implementation.Jobs
{
    public class MatrixPositionHasBeenBoughtJob : IJob<Guid>
    {
        private readonly IBitClub7Context _context;
        private readonly IMatrixPositionRepository _matrixPositionRepository;

        public MatrixPositionHasBeenBoughtJob(IBitClub7Context context, IMatrixPositionRepository matrixPositionRepository)
        {
            _context = context;
            _matrixPositionRepository = matrixPositionRepository;
        }

        public async Task Execute(Guid matrixPositionId, PerformContext context)
        {
            context.WriteLine($"MatrixPositionHasBeenBoughtJob started with matrixPositionId - {matrixPositionId}");

            var matrixPositionBought = await _matrixPositionRepository.GetAsync(matrixPositionId);

            await LeftRightValuesReindexation(matrixPositionBought);
            await AddTwoEmptyChildToBoughtPosition(matrixPositionBought);

            context.WriteLine("MatrixPositionHasBeenBoughtJob completed.");
        }

        private async Task LeftRightValuesReindexation(MatrixPosition matrixPositionBought)
        {
            await _context.Set<MatrixPosition>()
                .Where(x => x.Left > matrixPositionBought.Left)
                .Where(x => x.MatrixLevel == matrixPositionBought.MatrixLevel)
                .UpdateAsync(x => new MatrixPosition
                {
                    Left = x.Left + 4
                });

            await _context.Set<MatrixPosition>()
                .Where(x => x.Right >= matrixPositionBought.Right)
                .Where(x => x.MatrixLevel == matrixPositionBought.MatrixLevel)
                .UpdateAsync(x => new MatrixPosition
                {
                    Right = x.Right + 4
                });
        }

        private async Task AddTwoEmptyChildToBoughtPosition(MatrixPosition matrixPositionBought)
        {
            var newDepthLevel = matrixPositionBought.DepthLevel + 1;

            var matricesToAdd = new List<MatrixPosition>()
            {
                new MatrixPosition
                (
                    id: Guid.NewGuid(),
                    matrixLevel: matrixPositionBought.MatrixLevel,
                    depthLevel: newDepthLevel,
                    left: matrixPositionBought.Left + 1,
                    right: matrixPositionBought.Left + 2,
                    parentId: matrixPositionBought.Id,
                    userMultiAccountId: null
                ),
                new MatrixPosition
                (
                    id: Guid.NewGuid(),
                    matrixLevel: matrixPositionBought.MatrixLevel,
                    depthLevel: newDepthLevel,
                    left : matrixPositionBought.Left + 3,
                    right : matrixPositionBought.Left + 4,
                    parentId: matrixPositionBought.Id,
                    userMultiAccountId: null
                )
            };

            _context.Set<MatrixPosition>().AddRange(matricesToAdd);
            await _context.SaveChangesAsync();
        }
    }
}
