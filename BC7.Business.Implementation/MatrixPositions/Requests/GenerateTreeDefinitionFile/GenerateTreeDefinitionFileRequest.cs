using MediatR;

namespace BC7.Business.Implementation.MatrixPositions.Requests.GenerateTreeDefinitionFile
{
    public class GenerateTreeDefinitionFileRequest : IRequest
    {
        public int MatrixLevel { get; }

        public GenerateTreeDefinitionFileRequest(int matrixLevel)
        {
            MatrixLevel = matrixLevel;
        }
    }
}