using AutoMapper;
using BC7.Business.Implementation.Authentications.Commands.Login;
using BC7.Business.Implementation.Users.Commands.RegisterNewUserAccount;
using BC7.Business.Models;
using BC7.Domain;

namespace BC7.Business.Implementation.Tests.Integration.Mappings
{
    public class BusinessMappersTest : Profile
    {
        public BusinessMappersTest()
        {
            CreateMap<RegisterNewUserModel, RegisterNewUserAccountCommand>();
            CreateMap<LoginModel, LoginCommand>();

            CreateMap<RegisterNewUserAccountCommand, UserAccountData>(); // TODO: Maybe commands -> entities should be mapper manually?

            CreateMap<UserMultiAccount, UserMultiAccountModel>();
            CreateMap<MatrixPosition, MatrixPositionModel>();
        }
    }
}
