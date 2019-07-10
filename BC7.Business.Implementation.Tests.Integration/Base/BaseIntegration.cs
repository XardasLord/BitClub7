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
        // TODO Do the base test abstract prettier ;)
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
        protected IArticleRepository _articleRepository;
        protected ITicketRepository _ticketRepository;
        protected IWithdrawalRepository _withdrawalRepository;
        protected IOptions<JwtSettings> _jwtSettings;
        protected IOptions<BitBayPayApiSettings> _bitBayPayApiSettings;
        protected IOptions<MatrixStructureSettings> _matrixStructureSettings;

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
            services.AddTransient<IArticleRepository, ArticleRepository>();
            services.AddTransient<ITicketRepository, TicketRepository>();
            services.AddTransient<IWithdrawalRepository, WithdrawalRepository>();
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

            services.AddAutoMapper();
            services.AddMediatR(typeof(RegisterNewUserAccountCommand).Assembly);

            var connectionString = $@"Server=XARDASLORD\SQLEXPRESS;Database=BitClub7_integration_tests_{Guid.NewGuid()};Integrated Security=SSPI";
            services.AddDbContext<IBitClub7Context, BitClub7Context>(
                opts => opts.UseSqlServer(connectionString,
                    b => b.MigrationsAssembly(typeof(IBitClub7Context).Namespace))
            );

            var serviceProvider = services.AddEntityFrameworkSqlServer()
                .BuildServiceProvider();

            _context = serviceProvider.GetRequiredService<IBitClub7Context>();
            _mapper = serviceProvider.GetRequiredService<IMapper>();
            _mediator = serviceProvider.GetRequiredService<IMediator>();
            _reflinkHelper = serviceProvider.GetRequiredService<IReflinkHelper>();
            _userAccountDataHelper = serviceProvider.GetRequiredService<IUserAccountDataHelper>();
            _matrixPositionHelper = serviceProvider.GetRequiredService<IMatrixPositionHelper>();
            _userMultiAccountHelper = serviceProvider.GetRequiredService<IUserMultiAccountHelper>();
            _paymentHistoryHelper = serviceProvider.GetRequiredService<IPaymentHistoryHelper>();
            _userAccountDataRepository = serviceProvider.GetRequiredService<IUserAccountDataRepository>();
            _userMultiAccountRepository = serviceProvider.GetRequiredService<IUserMultiAccountRepository>();
            _matrixPositionRepository = serviceProvider.GetRequiredService<IMatrixPositionRepository>();
            _paymentHistoryRepository = serviceProvider.GetRequiredService<IPaymentHistoryRepository>();
            _articleRepository = serviceProvider.GetRequiredService<IArticleRepository>();
            _ticketRepository = serviceProvider.GetRequiredService<ITicketRepository>();
            _withdrawalRepository = serviceProvider.GetRequiredService<IWithdrawalRepository>();
            _jwtSettings = serviceProvider.GetRequiredService<IOptions<JwtSettings>>();
            _bitBayPayApiSettings = serviceProvider.GetRequiredService<IOptions<BitBayPayApiSettings>>();
            _matrixStructureSettings = serviceProvider.GetRequiredService<IOptions<MatrixStructureSettings>>();

            _context.Database.Migrate();
            await ClearDatabase();
        }

        public async Task ClearDatabase()
        {
            var matrices = _context.MatrixPositions;
            var multiAccounts = _context.UserMultiAccounts;
            var users = _context.UserAccountsData;
            var paymentHistories = _context.PaymentHistories;
            var articles = _context.Articles;

            _context.MatrixPositions.RemoveRange(matrices);
            _context.UserMultiAccounts.RemoveRange(multiAccounts);
            _context.UserAccountsData.RemoveRange(users);
            _context.PaymentHistories.RemoveRange(paymentHistories);
            _context.Articles.RemoveRange(articles);

            await _context.SaveChangesAsync();
        }

        [TearDown]
        public async Task TearDown()
        {
            await _context.Database.EnsureDeletedAsync();
        }
    }
}