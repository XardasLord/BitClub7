using System;
using System.Threading.Tasks;
using AutoMapper;
using BC7.Business.Helpers;
using BC7.Business.Implementation.Helpers;
using BC7.Business.Implementation.Users.Commands.RegisterNewUserAccount;
using BC7.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace BC7.Business.Implementation.Tests.Integration
{
    public abstract class BaseIntegration
    {
        protected IBitClub7Context _context;
        protected IMapper _mapper;
        protected IReflinkHelper _reflinkHelper;
        protected IUserMultiAccountHelper _userMultiAccountHelper;
        protected IUserAccountDataHelper _userAccountDataHelper;
        protected IMatrixPositionHelper _matrixPositionHelper;

        [SetUp]
        public async Task SetUp()
        {
            var services = new ServiceCollection();
            services.AddTransient<IReflinkHelper, ReflinkHelper>();
            services.AddTransient<IUserAccountDataHelper, UserAccountDataHelper>();
            services.AddTransient<IMatrixPositionHelper, MatrixPositionHelper>();
            services.AddTransient<IUserMultiAccountHelper, UserMultiAccountHelper>();

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
            _reflinkHelper = serviceProvider.GetService<IReflinkHelper>();
            _userAccountDataHelper = serviceProvider.GetService<IUserAccountDataHelper>();
            _matrixPositionHelper = serviceProvider.GetService<IMatrixPositionHelper>();
            _userMultiAccountHelper = serviceProvider.GetService<IUserMultiAccountHelper>();

            _context.Database.Migrate();
            await ClearDatabase();
        }

        public async Task ClearDatabase()
        {
            var matrices = _context.MatrixPositions;
            var multiAccounts = _context.UserMultiAccounts;
            var users = _context.UserAccountsData;

            _context.MatrixPositions.RemoveRange(matrices);
            _context.UserMultiAccounts.RemoveRange(multiAccounts);
            _context.UserAccountsData.RemoveRange(users);

            await _context.SaveChangesAsync();
        }


        [TearDown]
        public async Task TearDown()
        {
            await _context.Database.EnsureDeletedAsync();
        }
    }
}
