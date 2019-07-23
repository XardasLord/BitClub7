using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using BC7.Common.Settings;
using BC7.Infrastructure.CustomExceptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace BC7.Business.Implementation.Files.Commands.UploadFile
{
    public class UploadFileCommandHandler : IRequestHandler<UploadFileCommand, UploadFileViewModel>
    {
        private readonly IOptions<StorageSettings> _storageSettingsOptions;

        public UploadFileCommandHandler(IOptions<StorageSettings> storageSettingsOptions)
        {
            _storageSettingsOptions = storageSettingsOptions;
        }

        public async Task<UploadFileViewModel> Handle(UploadFileCommand request, CancellationToken cancellationToken = default(CancellationToken))
        {
            PreValidation(request);

            var filePath = await UploadFile(request.File);

            return new UploadFileViewModel
            {
                PathToFile = filePath
            };
        }

        private static void PreValidation(UploadFileCommand request)
        {
            if (request.File is null)
            {
                throw new ValidationException("File to upload cannot be null.");
            }

            if (request.File.Length == 0)
            {
                throw new ValidationException("File to upload is not provided.");
            }
        }

        private async Task<string> UploadFile(IFormFile file)
        {
            var newFileName = CreateFileName(file);

            var absolutePath = Path.GetFullPath(Directory.GetCurrentDirectory() + _storageSettingsOptions.Value.Files);
            var filePath = Path.Combine(absolutePath, newFileName);
            using (var fileSteam = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileSteam);
            }

            return filePath;
        }

        private static string CreateFileName(IFormFile file)
        {
            var fileName = Path.GetFileNameWithoutExtension(file.FileName);
            var extension = Path.GetExtension(file.FileName);
            var newFileName = $"{fileName}_{Guid.NewGuid()}{extension}";

            return newFileName;
        }
    }
}