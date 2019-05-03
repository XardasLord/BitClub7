using System;

namespace BC7.Business.Implementation.MatrixPositions.Commands.UpgradeMatrix
{
    public class UpgradeMatrixResult
    {
        public Guid UpgradedMatrixPositionId { get; private set; }
        public string ErrorMsg { get; private set; }

        public UpgradeMatrixResult(Guid upgradedMatrixPositionId, string errorMsg = "")
        {
            UpgradedMatrixPositionId = upgradedMatrixPositionId;
            ErrorMsg = errorMsg;
        }

    }
}
