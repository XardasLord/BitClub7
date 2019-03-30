using MediatR;

namespace BC7.Business.Implementation.Authentications.Commands.Login
{
    public class LoginCommand : IRequest<string>
    {
        public string LoginOrEmail { get; set; }
        public string Password { get; set; }
    }
}
