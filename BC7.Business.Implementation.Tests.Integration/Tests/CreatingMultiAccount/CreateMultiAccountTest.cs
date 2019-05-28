using System;
using System.Linq;
using System.Threading.Tasks;
using BC7.Business.Implementation.Tests.Integration.Base;
using BC7.Business.Implementation.Tests.Integration.FakerSeedGenerator;
using BC7.Business.Implementation.Users.Commands.CreateMultiAccount;
using BC7.Domain;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace BC7.Business.Implementation.Tests.Integration.Tests.CreatingMultiAccount
{
    public class CreateMultiAccountTest : BaseIntegration
    {
        private CreateMultiAccountCommandHandler _sut;

        [Test]
        public async Task CreateMultiAccountCommandHandler_WhenHandle_CreateMultiAccountForTheUser()
        {
            _sut = new CreateMultiAccountCommandHandler(_context, _userAccountDataRepository, _userMultiAccountRepository, _matrixPositionRepository, _userMultiAccountHelper, _matrixPositionHelper);
            await CreateUserAndMultiAccountAndMatrixPositionsInDatabase();
            var command = new CreateMultiAccountCommand
            {
                UserAccountId = Guid.Parse("042d748c-9cef-4a5a-92bd-3fd9a4a0e499"),
                SponsorReflink = "otherUserReflink12345"
            };

            var result = await _sut.Handle(command);

            var multiAccounts = _context.UserMultiAccounts.Where(x => x.UserAccountDataId == Guid.Parse("042d748c-9cef-4a5a-92bd-3fd9a4a0e499")).ToList();
            result.Should().NotBe(Guid.Empty);
            multiAccounts.Count.Should().Be(2);

            var multiAccount = await _context.UserMultiAccounts.Include(x => x.Sponsor).SingleOrDefaultAsync(x => x.Id == result);
            multiAccount.Sponsor.RefLink.Should().Be(command.SponsorReflink);
        }

        private async Task CreateUserAndMultiAccountAndMatrixPositionsInDatabase()
        {
            var fakerGenerator = new FakerGenerator();

            var existingUserAccountData = fakerGenerator.GetUserAccountDataFakerGenerator()
                .RuleFor(x => x.Id, f => Guid.Parse("042d748c-9cef-4a5a-92bd-3fd9a4a0e499"))
                .RuleFor(x => x.IsMembershipFeePaid, true)
                .Generate();

            var otherUser = fakerGenerator.GetUserAccountDataFakerGenerator()
                .RuleFor(x => x.IsMembershipFeePaid, true)
                .Generate();

            _context.UserAccountsData.AddRange(existingUserAccountData, otherUser);
            await _context.SaveChangesAsync();

            var myMultiAccount = new UserMultiAccount
            (
                id: Guid.NewGuid(),
                userAccountDataId: existingUserAccountData.Id,
                sponsorId: null,
                multiAccountName: "myMultiAccountName"
            );
            myMultiAccount.SetReflink("myReflink12345");
            myMultiAccount.SetAsMainAccount();

            var otherMultiAccount = new UserMultiAccount
            (
                id: Guid.NewGuid(),
                userAccountDataId: otherUser.Id,
                sponsorId: null,
                multiAccountName: "otherMultiAccountName"
            );
            otherMultiAccount.SetReflink("otherUserReflink12345");
            otherMultiAccount.SetAsMainAccount();

            _context.UserMultiAccounts.AddRange(myMultiAccount, otherMultiAccount);
            await _context.SaveChangesAsync();

            // Matrices
            var myMatrixPosition = new MatrixPosition
            (
                id: Guid.NewGuid(),
                userMultiAccountId: myMultiAccount.Id,
                parentId: null,
                matrixLevel: 0,
                depthLevel: 0,
                left: 1,
                right: 6
            );
            _context.MatrixPositions.Add(myMatrixPosition);

            var otherMatrixPosition = new MatrixPosition
            (
                id: Guid.NewGuid(),
                userMultiAccountId: otherMultiAccount.Id,
                parentId: myMatrixPosition.Id,
                matrixLevel: 0,
                depthLevel: 2, // Level 2 (Line B of the main account so it's ok)
                left: 2,
                right: 5
            );
            _context.MatrixPositions.Add(otherMatrixPosition);

            var otherMatrixPosition2 = new MatrixPosition
            (
                id: Guid.NewGuid(),
                userMultiAccountId: null,
                parentId: otherMatrixPosition.Id,
                matrixLevel: 0,
                depthLevel: 3, // Line C
                left: 3,
                right: 4
            );
            _context.MatrixPositions.Add(otherMatrixPosition2);
            await _context.SaveChangesAsync();
        }
    }
}
