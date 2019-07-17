using System.Threading.Tasks;
using BC7.Business.Implementation.Files.Commands.UploadFile;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BC7.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FilesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Upload file
        /// </summary>
        /// <param name="file">A file to upload</param>
        /// <returns>Returns object with path to stored file</returns>
        /// <response code="200">Success - returns object with path to stored file</response>
        /// <response code="401">Failed - only logged in users have access</response>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            var command = new UploadFileCommand { File = file };
            return Ok(await _mediator.Send(command));
        }
    }
}