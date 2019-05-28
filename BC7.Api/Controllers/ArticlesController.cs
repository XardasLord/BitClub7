using System;
using System.Threading.Tasks;
using BC7.Business.Implementation.Articles.Commands.CreateArticle;
using BC7.Business.Implementation.Articles.Commands.DeleteArticle;
using BC7.Business.Implementation.Articles.Commands.UpdateArticle;
using BC7.Business.Implementation.Articles.Requests;
using BC7.Business.Models;
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
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _mediator.Send(new GetArticlesRequest()));
        }

        /// <summary>
        /// Create new article available for root user only
        /// </summary>
        /// <param name="command">Command object contains user ID, title and text of the article</param>
        /// <returns>Returns OK (200)</returns>
        /// <response code="200">Returns ID of the newly created article</response>
        /// <response code="403">Failed - Only root users have access</response>
        [HttpPost]
        [Authorize(Roles = "Root")]
        public async Task<IActionResult> Create(CreateArticleCommand command)
        {
            // TODO: Maybe 201 created?
            return Ok(new { Id = await _mediator.Send(command) });
        }

        /// <summary>
        /// Update existing article
        /// </summary>
        /// <param name="id">ID of the article to update</param>
        /// <param name="model">Model with new title and text of article</param>
        /// <returns>Returns NoContent (204)</returns>
        /// <response code="204">Success - article updated</response>
        /// <response code="403">Failed - Only root users have access</response>
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
    }
}