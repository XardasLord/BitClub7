using System.IO;
using System.Threading;
using System.Threading.Tasks;
using BC7.Business.Helpers;
using MediatR;

namespace BC7.Business.Implementation.MatrixPositions.Requests.GenerateTreeDefinitionFile
{
    public class GenerateTreeDefinitionFileRequestHandler : IRequestHandler<GenerateTreeDefinitionFileRequest>
    {
        private readonly IMatrixPositionHelper _matrixPositionHelper;

        public GenerateTreeDefinitionFileRequestHandler(IMatrixPositionHelper matrixPositionHelper)
        {
            _matrixPositionHelper = matrixPositionHelper;
        }

        public async Task<Unit> Handle(GenerateTreeDefinitionFileRequest request, CancellationToken cancellationToken = default(CancellationToken))
        {
            var fileDefinition = await _matrixPositionHelper.GenerateMatrixStructureTreeFileDefinition(request.MatrixLevel);
            
            using (var sw = new StreamWriter(@"c:\temp\matrixTreeDefinition.dot")) // TODO: Move the path to the configuration file
            {
                sw.WriteLine("digraph G {");
                sw.Write(fileDefinition);
                sw.WriteLine("}");
            }

            throw new System.NotImplementedException();
        }
    }
}
