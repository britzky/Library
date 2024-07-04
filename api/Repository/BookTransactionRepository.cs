using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class BookTransactionRepository : IBookTransactionRepository
    {
        private readonly AppDBContext _context;
        public BookTransactionRepository(AppDBContext context)
        {
            _context = context;
        }

        public async Task<List<BookTransaction>> GetBooksPendingCirculationAsync()
        {
            return await _context.BookTransactions
                                .Include(bt => bt.Book)
                                .Where(bt => bt.IsReturned && !bt.Book.Availability)
                                .ToListAsync();
        }

        public async Task<List<BookTransaction>> GetCustomerCheckedOutBooksAsync(string userId)
        {
            return await _context.BookTransactions
                                .Include(bt => bt.Book)
                                .Where(bt => bt.UserId == userId && !bt.IsReturned)
                                .ToListAsync();
        }

        public async Task<BookTransaction?> GetTransactionByIdAsync(int transactionId)
        {
            return await _context.BookTransactions
                .Include(bt => bt.Book)
                .Include(bt => bt.User)
                .FirstOrDefaultAsync(bt => bt.Id == transactionId);
        }

        public async Task UpdateTransactionAsync(BookTransaction transaction)
        {
            _context.BookTransactions.Update(transaction);
            await _context.SaveChangesAsync();
        }
    }
}