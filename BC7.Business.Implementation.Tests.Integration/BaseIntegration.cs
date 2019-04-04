using System;
using System.Threading.Tasks;
using AutoMapper;
using BC7.Business.Helpers;
using BC7.Business.Implementation.Helpers;
using BC7.Database;
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
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkSqlServer()
                .BuildServiceProvider();
           
            var builder = new DbContextOptionsBuilder<BitClub7Context>();
            builder.UseSqlServer($@"Server=XARDASLORD\SQLEXPRESS;Database=BitClub7_integration_tests_{Guid.NewGuid()};Integrated Security=SSPI")
                .UseInternalServiceProvider(serviceProvider);

            _context = new BitClub7Context(builder.Options);
            _context.Database.Migrate();

            await ClearDatabase();
            
            using (var scope = serviceProvider.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;

                _mapper = scopedServices.GetRequiredService<Mapper>();
                _reflinkHelper = scopedServices.GetRequiredService<ReflinkHelper>();
                _userMultiAccountHelper = scopedServices.GetRequiredService<UserMultiAccountHelper>();
                _userAccountDataHelper = scopedServices.GetRequiredService<UserAccountDataHelper>();
                _matrixPositionHelper = scopedServices.GetRequiredService<MatrixPositionHelper>();
            }

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
