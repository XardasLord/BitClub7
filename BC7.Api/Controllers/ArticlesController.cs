using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BC7.Business.Implementation.Articles.Commands.CreateArticle;
using BC7.Business.Implementation.Articles.Commands.DeleteArticle;
using BC7.Business.Implementation.Articles.Commands.UpdateArticle;
using BC7.Business.Implementation.Articles.Requests.GetArticle;
using BC7.Business.Implementation.Articles.Requests.GetArticles;
using BC7.Business.Models;
using BC7.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        /// Get all standard articles
        /// </summary>
        /// <returns>Returns model with list of articles</returns>
        /// <response code="200">Success - returns model with list of articles</response>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllStandardArticles()
        {
            return Ok(await _mediator.Send(new GetArticlesRequest { ArticleType = ArticleType.Standard }));
        }

        /// <summary>
        /// Get all articles marked as cryptoblog
        /// </summary>
        /// <returns>Returns model with list of cryptoblog articles</returns>
        /// <response code="200">Success - returns model with list of cryptoblog articles</response>
        /// <response code="401">Failed - only logged in users have access</response>
        [HttpGet("cryptoBlogs")]
        [Authorize]
        public async Task<IActionResult> GetAllCryptoBlogs()
        {
            var request = new GetArticlesRequest
            {
                ArticleType = ArticleType.CryptoBlog,
                RequestedUser = GetLoggerUserFromJwt()
            };

            return Ok(await _mediator.Send(request));
        }

        /// <summary>
        /// Get article by ID
        /// </summary>
        /// <param name="id">ID of an article</param>
        /// <returns>Returns article</returns>
        /// <response code="200">Success - returns article</response>
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(Guid id)
        {
            var request = new GetArticleRequest { Id = id };
            return Ok(await _mediator.Send(request));
        }

        /// <summary>
        /// Create new article available for root user only
        /// </summary>
        /// <param name="command">Command object contains user ID, title and text of the article</param>
        /// <returns>Returns OK (200)</returns>
        /// <response code="201">Success - returns ID of the newly created article</response>
        /// <response code="403">Failed - only root users have access</response>
        [HttpPost]
        [Authorize(Roles = "Root")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Create(CreateArticleCommand command)
        {
            var articleId = await _mediator.Send(command);

            return CreatedAtAction(nameof(Get), new { Id = articleId }, new { Id = articleId });
        }

        /// <summary>
        /// Update existing article
        /// </summary>
        /// <param name="id">ID of the article to update</param>
        /// <param name="model">Model with new title and text of article</param>
        /// <returns>Returns NoContent (204)</returns>
        /// <response code="204">Success - article updated</response>
        /// <response code="403">Failed - only root users have access</response>
        [HttpPut("{id}")]
        [Authorize(Roles = "Root")]
        public async Task<IActionResult> Update(Guid id, UpdateArticleModel model)
        {
            var command = new UpdateArticleCommand
            {
                ArticleId = id,
                Title = model.Title,
                Text = model.Text
            };

            await _mediator.Send(command);

            return NoContent();
        }

        /// <summary>
        /// Delete article
        /// </summary>
        /// <param name="id">ID of the article to delete</param>
        /// <returns>Returns NoContent (204)</returns>
        /// <response code="204">Success - article deleted</response>
        /// <response code="403">Failed - Only root users have access</response>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Root")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var command = new DeleteArticleCommand
            {
                ArticleId = id
            };

            await _mediator.Send(command);

            return NoContent();
        }

        private LoggedUserModel GetLoggerUserFromJwt()
        {
            var claims = HttpContext.User.Claims.ToList();

            var id = claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid)?.Value;
            var email = claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            var role = claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;

            return new LoggedUserModel(Guid.Parse(id), email, role);
        }
    }
}