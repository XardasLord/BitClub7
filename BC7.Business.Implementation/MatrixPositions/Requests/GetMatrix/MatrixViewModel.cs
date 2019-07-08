using System.Collections.Generic;
using BC7.Business.Models;

namespace BC7.Business.Implementation.MatrixPositions.Requests.GetMatrix
{
    public class MatrixViewModel
    {
        public IEnumerable<MatrixPositionModel> Matrix { get; set; }
    }
}
