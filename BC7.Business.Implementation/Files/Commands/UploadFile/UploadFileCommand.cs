using MediatR;
using Microsoft.AspNetCore.Http;

namespace BC7.Business.Implementation.Files.Commands.UploadFile
{
    public class UploadFileCommand : IRequest<UploadFileViewModel>
    {
        public IFormFile File { get; set; }
    }
}