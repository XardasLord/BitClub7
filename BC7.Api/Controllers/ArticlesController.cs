using System.Threading.Tasks;
using BC7.Business.Implementation.Articles.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BC7.Api.Controllers
{
    [Route("api/[controller]")]
    public class ArticlesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ArticlesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Create new article available for root user only
        /// </summary>
        /// <param name="command">Command object contains user ID, title and text of the article</param>
        /// <returns>Returns OK (200)</returns>
        [HttpPost]
        [Authorize(Roles = "Root")]
        public async Task<IActionResult> CreateArticle(CreateArticleCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }
    }
}