using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using BC7.Business.Implementation.Files.Commands.UploadFile;
using BC7.Business.Implementation.Users.Commands.ChangeAvatar;
using BC7.Business.Implementation.Users.Commands.CreateMultiAccount;
using BC7.Business.Implementation.Users.Commands.RegisterNewUserAccount;
using BC7.Business.Implementation.Users.Commands.UpdateUser;
using BC7.Business.Implementation.Users.Requests.GetInitiativeDescriptionForMultiAccount;
using BC7.Business.Implementation.Users.Requests.GetMultiAccounts;
using BC7.Business.Implementation.Users.Requests.GetPaymentHistories;
using BC7.Business.Implementation.Users.Requests.GetUser;
using BC7.Business.Implementation.Users.Requests.GetUsers;
using BC7.Business.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BC7.Api.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public UsersController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all users available in the system
        /// </summary>
        /// <returns>Returns list of users in the system</returns>
        /// <response code="200">Success - returns list of users in the system</response>
        /// <response code="403">Fail - only root users have access</response>
        [HttpGet]
        [Authorize(Roles = "Root")]
        public async Task<IActionResult> GetAllUsers()
        {
            return Ok(await _mediator.Send(new GetUsersRequest()));
        }


        /// <summary>
        /// Register new user account
        /// </summary>
        /// <param name="model">A model with all user account data</param>
        /// <param name="reflink">Optional query parameter. If send it will attach the main account to this reflink's user</param>
        /// <returns>Returns the Id of the created user account</returns>
        /// <response code="201">Returns the Id of the newly created user account</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> RegisterNewAccount([FromBody] RegisterNewUserModel model, [FromQuery] string reflink = null)
        {
            var command = _mapper.Map<RegisterNewUserAccountCommand>(model);
            command.SponsorRefLink = reflink;

            var userId = await _mediator.Send(command);

            return CreatedAtAction(nameof(GetUser), new { userId = userId }, new { Id = userId });
        }

        /// <summary>
        /// Get user's data by ID
        /// </summary>
        /// <param name="userId">User ID to get</param>
        /// <returns>Returns the user model with his data</returns>
        /// <response code="200">Returns the user model with his data</response>
        /// <response code="401">Failed - authorization is required</response>
        [HttpGet("{userId}")]
        [Authorize]
        public async Task<IActionResult> GetUser(Guid userId)
        {
            return Ok(await _mediator.Send(new GetUserRequest { UserId = userId }));
        }

        /// <summary>
        /// Create new multi account for the user
        /// </summary>
        /// <param name="model">A model with the reflink of user who invites the requested user</param>
        /// <param name="userId">User account ID</param>
        /// <returns>Returns the Id of the newly created user multi account</returns>
        /// <response code="200">Returns the Id of the newly created user multi account</response>
        /// <response code="401">Failed - only logged in users have access</response>
        [HttpPost("{userId}/multiAccounts")]
        [Authorize]
        public async Task<IActionResult> CreateMultiAccount([FromBody] CreateMultiAccountModel model, [FromRoute] Guid userId)
        {
            var command = new CreateMultiAccountCommand
            {
                UserAccountId = userId,
                SponsorReflink = model.SponsorReflink
            };

            // TODO: Maybe 201 created?
            return Ok(new { Id = await _mediator.Send(command) });
        }

        /// <summary>
        /// GetAsync multi accounts for given User ID
        /// </summary>
        /// <param name="userId">User ID for whom multi accounts will be returned</param>
        /// <returns>Returns list of multi accounts</returns>
        /// <response code="200">Returns list of multi accounts</response>
        /// <response code="401">Failed - only logged in users have access</response>
        [HttpGet("{userId}/multiAccounts")]
        [Authorize]
        public async Task<IActionResult> GetAllMultiAccounts([FromRoute] Guid userId)
        {
            var request = new GetMultiAccountsRequest { UserAccountId = userId };

            return Ok(await _mediator.Send(request));
        }

        /// <summary>
        /// Get payments history for the user ID
        /// </summary>
        /// <param name="userId">User ID for whom payments history will be returned</param>
        /// <returns>Returns list of user payments</returns>
        /// <response code="200">Returns list of user payments</response>
        /// <response code="401">Failed - only logged in users have access</response>
        [HttpGet("{userId}/payments")]
        [Authorize]
        public async Task<IActionResult> GetAllPayments([FromRoute] Guid userId)
        {
            var request = new GetPaymentHistoriesRequest { UserAccountId = userId };

            return Ok(await _mediator.Send(request));
        }

        /// <summary>
        /// Get initiative description for the multiAccount ID
        /// </summary>
        /// <param name="multiAccountId">User multiAccount ID whose initiative description will be returned</param>
        /// <returns>Returns an initiative description for the multiAccount ID</returns>
        /// <response code="200">Returns an initiative description for the multiAccount ID</response>
        [HttpGet("{multiAccountId}/initiative")]
        public async Task<IActionResult> GetUserInitiative([FromRoute] Guid multiAccountId)
        {
            var request = new GetInitiativeDescriptionRequest { MultiAccountId = multiAccountId };

            return Ok(await _mediator.Send(request));
        }

        /// <summary>
        /// Upload avatar file for the user
        /// </summary>
        /// <param name="userId">User account data ID for whom avatar is uploaded</param>
        /// <param name="file">An avatar file</param>
        /// <returns>Returns NoContent status code</returns>
        /// <response code="204">Returns NoContent status code</response>
        /// <response code="401">Failed - only logged in users have access</response>
        [HttpPatch("{userId}/avatar")]
        [Authorize]
        public async Task<IActionResult> AvatarUpload(Guid userId, IFormFile file)
        {
            // TODO: Allow only .jpg, .png files

            var uploadFileCommand = new UploadFileCommand { File = file };
            var result = await _mediator.Send(uploadFileCommand);

            var changeAvatarCommand = new ChangeAvatarCommand
            {
                UserAccountDataId = userId,
                AvatarPath = result.PathToFile
            };
            await _mediator.Send(changeAvatarCommand);

            return NoContent();
        }

        /// <summary>
        /// Update user data
        /// </summary>
        /// <param name="id">User ID to update</param>
        /// <param name="model">Model with properties to update</param>
        /// <returns>Returns NoContent(204)</returns>
        /// <response code="204">Success - user updated</response>
        /// <response code="401">Failed - only logged in users have access</response>
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUserModel model)
        {
            var command = _mapper.Map<UpdateUserCommand>(model);

            command.UserId = id;
            command.RequestedUser = GetLoggerUserFromJwt();

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