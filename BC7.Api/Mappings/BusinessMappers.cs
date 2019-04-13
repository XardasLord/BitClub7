﻿using AutoMapper;
using BC7.Business.Implementation.Users.Commands.RegisterNewUserAccount;
using BC7.Business.Models;
using BC7.Domain;

namespace BC7.Api.Mappings
{
    public class BusinessMappers : Profile
    {
        public BusinessMappers()
        {
            CreateMap<RegisterNewUserModel, RegisterNewUserAccountCommand>();

            CreateMap<UserMultiAccount, UserMultiAccountModel>()
                .ForMember(x => x.MatrixPositionModels, opt => opt.MapFrom(y => y.MatrixPositions));

            CreateMap<MatrixPosition, MatrixPositionModel>();
        }
    }
}
