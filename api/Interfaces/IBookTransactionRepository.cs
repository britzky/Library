using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.Interfaces
{
    public interface IBookTransactionRepository
    {
        Task <List<BookTransaction>> GetCustomerCheckedOutBooksAsync(string userId);
        Task <List<BookTransaction>> GetBooksPendingCirculationAsync();
        Task UpdateTransactionAsync(BookTransaction transaction);
        Task<BookTransaction?> GetTransactionByIdAsync(int transactionId);
    }
}