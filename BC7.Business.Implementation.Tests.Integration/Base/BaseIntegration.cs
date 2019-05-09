using System;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using BC7.Business.Helpers;
using BC7.Business.Implementation.Helpers;
using BC7.Business.Implementation.Users.Commands.RegisterNewUserAccount;
using BC7.Common.Settings;
using BC7.Database;
using BC7.Repository;
using BC7.Repository.Implementation;
using BC7.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NUnit.Framework;

namespace BC7.Business.Implementation.Tests.Integration.Base
{
    public abstract class BaseIntegration
    {
        protected IBitClub7Context _context;
        protected IMapper _mapper;
        protected IMediator _mediator;
        protected IReflinkHelper _reflinkHelper;
        protected IUserMultiAccountHelper _userMultiAccountHelper;
        protected IUserAccountDataHelper _userAccountDataHelper;
        protected IMatrixPositionHelper _matrixPositionHelper;
        protected IPaymentHistoryHelper _paymentHistoryHelper;
        protected IUserAccountDataRepository _userAccountDataRepository;
        protected IUserMultiAccountRepository _userMultiAccountRepository;
        protected IMatrixPositionRepository _matrixPositionRepository;
        protected IPaymentHistoryRepository _paymentHistoryRepository;
        protected IOptions<JwtSettings> _jwtSettings;
        protected IOptions<BitBayPayApiSettings> _bitBayPayApiSettings;

        [SetUp]
        public async Task SetUp()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var services = new ServiceCollection();
            services.AddTransient<IReflinkHelper, ReflinkHelper>();
            services.AddTransient<IUserAccountDataHelper, UserAccountDataHelper>();
            services.AddTransient<IMatrixPositionHelper, MatrixPositionHelper>();
            services.AddTransient<IUserMultiAccountHelper, UserMultiAccountHelper>();
            services.AddTransient<IPaymentHistoryHelper, PaymentHistoryHelper>();
            services.AddTransient<IUserAccountDataRepository, UserAccountDataRepository>();
            services.AddTransient<IUserMultiAccountRepository, UserMultiAccountRepository>();
            services.AddTransient<IMatrixPositionRepository, MatrixPositionRepository>();
            services.AddTransient<IPaymentHistoryRepository, PaymentHistoryRepository>();
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

            services.AddAutoMapper();
            services.AddMediatR(typeof(RegisterNewUserAccountCommand).Assembly);

            services.AddDbContext<IBitClub7Context, BitClub7Context>(
                opts => opts.UseSqlServer($@"Server=XARDASLORD\SQLEXPRESS;Database=BitClub7_integration_tests_{Guid.NewGuid()};Integrated Security=SSPI",
                    b => b.MigrationsAssembly(typeof(IBitClub7Context).Namespace))
            );

            var serviceProvider = services.AddEntityFrameworkSqlServer()
                .BuildServiceProvider();

            _context = serviceProvider.GetService<IBitClub7Context>();
            _mapper = serviceProvider.GetService<IMapper>();
            _mediator = serviceProvider.GetService<IMediator>();
            _reflinkHelper = serviceProvider.GetService<IReflinkHelper>();
            _userAccountDataHelper = serviceProvider.GetService<IUserAccountDataHelper>();
            _matrixPositionHelper = serviceProvider.GetService<IMatrixPositionHelper>();
            _userMultiAccountHelper = serviceProvider.GetService<IUserMultiAccountHelper>();
            _paymentHistoryHelper = serviceProvider.GetService<IPaymentHistoryHelper>();
            _userAccountDataRepository = serviceProvider.GetService<IUserAccountDataRepository>();
            _userMultiAccountRepository = serviceProvider.GetService<IUserMultiAccountRepository>();
            _matrixPositionRepository = serviceProvider.GetService<IMatrixPositionRepository>();
            _paymentHistoryRepository = serviceProvider.GetService<IPaymentHistoryRepository>();
            _jwtSettings = serviceProvider.GetService<IOptions<JwtSettings>>();
            _bitBayPayApiSettings = serviceProvider.GetService<IOptions<BitBayPayApiSettings>>();

            _context.Database.Migrate();
            await ClearDatabase();
        }

        public async Task ClearDatabase()
        {
            var matrices = _context.MatrixPositions;
            var multiAccounts = _context.UserMultiAccounts;
            var users = _context.UserAccountsData;
            var paymentHistories = _context.PaymentHistories;

            _context.MatrixPositions.RemoveRange(matrices);
            _context.UserMultiAccounts.RemoveRange(multiAccounts);
            _context.UserAccountsData.RemoveRange(users);
            _context.PaymentHistories.RemoveRange(paymentHistories);

            await _context.SaveChangesAsync();
        }


        [TearDown]
        public async Task TearDown()
        {
            await _context.Database.EnsureDeletedAsync();
        }
    }
}
