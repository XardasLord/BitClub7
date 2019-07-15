using AutoMapper;
using BC7.Business.Implementation.Users.Commands.RegisterNewUserAccount;
using BC7.Business.Implementation.Users.Commands.UpdateUser;
using BC7.Business.Models;
using BC7.Domain;

namespace BC7.Api.Mappings
{
    public class BusinessMappers : Profile
    {
        public BusinessMappers()
        {
            CreateMap<RegisterNewUserModel, RegisterNewUserAccountCommand>()
                .ForMember(x => x.SponsorRefLink, opt => opt.Ignore());

            CreateMap<UserMultiAccount, UserMultiAccountModel>()
                .ForMember(x => x.MatrixPositionModels, opt => opt.MapFrom(y => y.MatrixPositions));

            CreateMap<MatrixPosition, MatrixPositionModel>()
                .ForMember(x => x.MultiAccountName, opt => opt.Ignore());

            CreateMap<Article, ArticleModel>()
                .ForMember(x => x.Creator, opt => opt.MapFrom(y => $"{y.Creator.FirstName} {y.Creator.LastName}"));

            CreateMap<Ticket, TicketModel>()
                .ForMember(x => x.TicketNumber, opt => opt.MapFrom(y => y.FullTicketNumber))
                .ForMember(x => x.SenderEmail, opt => opt.MapFrom(y => y.Email));

            CreateMap<UpdateUserModel, UpdateUserCommand>()
                .ForMember(x => x.UserId, opt => opt.Ignore())
                .ForMember(x => x.RequestedUser, opt => opt.Ignore());

            CreateMap<UserAccountData, UserAccountDataModel>()
                .ForMember(x => x.MultiAccountsTotalCount, opt => opt.MapFrom(y => y.UserMultiAccounts.Count));

            CreateMap<PaymentHistory, PaymentHistoryModel>()
                .ForMember(x => x.AccountName, opt => opt.Ignore()); // TODO: PaymentFor better display?

            CreateMap<Withdrawal, WithdrawalModel>()
                .ForMember(x => x.IsWithdrawn, opt => opt.MapFrom(y => y.WithdrawnAt.HasValue))
                .ForMember(x => x.BtcWalletAddress, opt => opt.MapFrom(y => y.UserMultiAccount.UserAccountData.BtcWalletAddress))
                .ForMember(x => x.MultiAccountName, opt => opt.MapFrom(y => y.UserMultiAccount.MultiAccountName))
                .ForMember(x => x.UserAccountDataId, opt => opt.MapFrom(y => y.UserMultiAccount.UserAccountDataId))
                .ForMember(x => x.UserMultiAccountId, opt => opt.MapFrom(y => y.UserMultiAccountId))
                .ForMember(x => x.PaymentFor, opt => opt.Ignore()); // TODO
        }
    }
}
