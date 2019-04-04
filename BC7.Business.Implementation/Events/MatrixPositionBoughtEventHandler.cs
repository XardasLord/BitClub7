﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BC7.Business.Helpers;
using BC7.Database;
using BC7.Entity;
using MediatR;
using Z.EntityFramework.Plus;

namespace BC7.Business.Implementation.Events
{
    public class MatrixPositionBoughtEventHandler : INotificationHandler<MatrixPositionBoughtEvent>
    {
        private readonly IBitClub7Context _context;
        private readonly IMatrixPositionHelper _matrixPositionHelper;

        public MatrixPositionBoughtEventHandler(IBitClub7Context context, IMatrixPositionHelper matrixPositionHelper)
        {
            _context = context;
            _matrixPositionHelper = matrixPositionHelper;
        }

        public async Task Handle(MatrixPositionBoughtEvent notification, CancellationToken cancellationToken)
        {
            var matrixPositionBought = await _matrixPositionHelper.Get(notification.MatrixPositionId);

            await LeftRightValuesReindexation(matrixPositionBought);
            await AddTwoEmptyChildToBoughtPosition(matrixPositionBought);
        }

        private async Task LeftRightValuesReindexation(MatrixPosition matrixPositionBought)
        {
            await _context.Set<MatrixPosition>()
                .Where(x => x.Left > matrixPositionBought.Left)
                .UpdateAsync(x => new MatrixPosition()
                {
                    Left = x.Left + 4
                });

            await _context.Set<MatrixPosition>()
                .Where(x => x.Right >= matrixPositionBought.Right)
                .UpdateAsync(x => new MatrixPosition()
                {
                    Right = x.Right + 4
                });
        }

        private async Task AddTwoEmptyChildToBoughtPosition(MatrixPosition matrixPositionBought)
        {
            var newDepthLevel = matrixPositionBought.DepthLevel + 1;

            var matricesToAdd = new List<MatrixPosition>()
            {
                new MatrixPosition()
                {
                    MatrixLevel = matrixPositionBought.MatrixLevel,
                    DepthLevel = newDepthLevel,
                    Left = matrixPositionBought.Left + 1,
                    Right = matrixPositionBought.Left + 2,
                    ParentId = matrixPositionBought.Id,
                    UserMultiAccountId = null
                },
                new MatrixPosition()
                {
                    MatrixLevel = matrixPositionBought.MatrixLevel,
                    DepthLevel = newDepthLevel,
                    Left = matrixPositionBought.Left + 3,
                    Right = matrixPositionBought.Left + 4,
                    ParentId = matrixPositionBought.Id,
                    UserMultiAccountId = null
                }
            };

            await _context.Set<MatrixPosition>().AddRangeAsync(matricesToAdd);
            await _context.SaveChangesAsync();
        }
    }
}