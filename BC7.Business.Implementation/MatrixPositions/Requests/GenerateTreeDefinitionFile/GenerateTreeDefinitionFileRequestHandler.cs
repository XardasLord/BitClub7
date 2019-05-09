using System.IO;
using System.Threading;
using System.Threading.Tasks;
using BC7.Business.Helpers;
using BC7.Common.Settings;
using MediatR;
using Microsoft.Extensions.Options;

namespace BC7.Business.Implementation.MatrixPositions.Requests.GenerateTreeDefinitionFile
{
    public class GenerateTreeDefinitionFileRequestHandler : IRequestHandler<GenerateTreeDefinitionFileRequest>
    {
        private readonly IMatrixPositionHelper _matrixPositionHelper;
        private readonly IOptions<MatrixStructureSettings> _matrixStructureSettings;

        public GenerateTreeDefinitionFileRequestHandler(IMatrixPositionHelper matrixPositionHelper, IOptions<MatrixStructureSettings> matrixStructureSettings)
        {
            _matrixPositionHelper = matrixPositionHelper;
            _matrixStructureSettings = matrixStructureSettings;
        }

        public async Task<Unit> Handle(GenerateTreeDefinitionFileRequest request, CancellationToken cancellationToken = default(CancellationToken))
        {
            var fileDefinition = await _matrixPositionHelper.GenerateMatrixStructureTreeFileDefinition(request.MatrixLevel);
            
            using (var sw = new StreamWriter(_matrixStructureSettings.Value.PathToFileDefinition))
            {
                sw.WriteLine("digraph G {");
                sw.Write(fileDefinition);
                sw.WriteLine("}");
            }

            return Unit.Value;
        }
    }
}
