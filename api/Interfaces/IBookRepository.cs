using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos;
using api.Models;

namespace api.Interfaces
{
    public interface IBookRepository
    {
       Task<List<Book>> GetAllAsync();
       Task<Book?> GetByIdAsync(int id);
       Task<Book> CreateAsync(Book bookModel);
       Task<Book?> UpdateAsync(int id, UpdateBookDto bookDto);
       Task<Book?> DeleteAsync(int id);
       Task<List<Book>> SearchAsync(string searchTerm);
       Task<List<Book>> GetFeaturedBooksAsync(string? sortBy, string? filterBy);
    }
}