using System;
using System.Threading.Tasks;
using BC7.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace BC7.Business.Implementation.Tests.Integration
{
    [TestFixture]
    public class RegisterNewUserAccountTests
    {
        private IBitClub7Context _context;

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
