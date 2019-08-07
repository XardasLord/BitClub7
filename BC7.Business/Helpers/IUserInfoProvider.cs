using BC7.Business.Models;

namespace BC7.Business.Helpers
{
	public interface IUserInfoProvider
	{
		LoggedUserModel GetUserInfo();
	}
}