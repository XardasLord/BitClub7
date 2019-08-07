using System;
using System.Linq;
using System.Security.Claims;
using BC7.Business.Helpers;
using BC7.Business.Models;
using Microsoft.AspNetCore.Http;

namespace BC7.Business.Implementation.Helpers
{
	public class UserInfoProvider : IUserInfoProvider
	{
		private readonly IHttpContextAccessor _httpContextAccessor;

		public UserInfoProvider(IHttpContextAccessor httpContextAccessor)
		{
			_httpContextAccessor = httpContextAccessor;
		}

		public LoggedUserModel GetUserInfo()
		{
			var user = _httpContextAccessor?.HttpContext?.User;

			if (user is null)
			{
				return new LoggedUserModel();
			}

			Guid.TryParse(user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid)?.Value, out var id);
			var email = user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
			var role = user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;

			return new LoggedUserModel(id, email, role);
		}
	}
}