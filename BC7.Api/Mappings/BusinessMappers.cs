using AutoMapper;
using BC7.Business.Implementation.Authentications.Commands.Login;
using BC7.Business.Implementation.Authentications.Commands.RegisterNewUserAccount;
using BC7.Business.Models;
using BC7.Entity;

namespace BC7.Api.Mappings
{
    public class BusinessMappers : Profile
    {
        public BusinessMappers()
        {
            CreateMap<RegisterNewUserModel, RegisterNewUserAccountCommand>();
            CreateMap<LoginModel, LoginCommand>();

            CreateMap<RegisterNewUserAccountCommand, UserAccountData>();
        }
    }
}
