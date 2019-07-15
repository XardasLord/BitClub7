using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BC7.Database;
using BC7.Domain;
using Microsoft.EntityFrameworkCore;

namespace BC7.Repository.Implementation
{
    public class WithdrawalRepository : IWithdrawalRepository
    {
        private readonly IBitClub7Context _context;

        public WithdrawalRepository(IBitClub7Context context)
        {
            _context = context;
        }

        public Task<Withdrawal> GetAsync(Guid id)
        {
            return _context.Set<Withdrawal>().SingleAsync(x => x.Id == id);
        }

        public Task<List<Withdrawal>> GetAllAsync()
        {
            return _context.Set<Withdrawal>()
                .Include(x => x.UserMultiAccount)
                .ThenInclude(x => x.UserAccountData)
                .ToListAsync();
        }

        public Task CreateAsync(Withdrawal withdrawal)
        {
            _context.Set<Withdrawal>().Add(withdrawal);
            return _context.SaveChangesAsync();
        }

        public Task UpdateAsync(Withdrawal withdrawal)
        {
            _context.Set<Withdrawal>().Attach(withdrawal);
            return _context.SaveChangesAsync();
        }
    }
}