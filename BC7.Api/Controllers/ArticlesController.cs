using System.Threading.Tasks;
using BC7.Business.Implementation.Articles.Commands.CreateArticle;
using BC7.Business.Implementation.Articles.Requests;
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
        /// Get all articles
        /// </summary>
        /// <returns>Returns model with list of articles</returns>
        /// <response code="200">Returns model with list of articles</response>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllArticles()
        {
            return Ok(await _mediator.Send(new GetArticlesRequest()));
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
            // TODO: 201 maybe?
            await _mediator.Send(command);
            return Ok();
        }
    }
}