using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class BookRepository : IBookRepository
    {
        private readonly AppDBContext _context;
        public BookRepository(AppDBContext context)
        {
            _context = context;
        }

        public async Task<Book> CreateAsync(Book bookModel)
        {
            await _context.Books.AddAsync(bookModel);
            await _context.SaveChangesAsync();
            return bookModel;
        }

        public async Task<List<Book>> GetAllAsync()
        {
            return await _context.Books.ToListAsync();
        }

        public async Task<Book?> GetByIdAsync(int id)
        {
            return await _context.Books.FindAsync(id);
        }
        public async Task<List<Book>> SearchAsync(string searchTerm)
        {
            var books = await _context.Books
                                        .Where(b => b.Title.Contains(searchTerm))
                                        .ToListAsync();
            return books;
        }

        public async Task<List<Book>> GetFeaturedBooksAsync(string? sortBy, string? filterBy)
        {
                        // get 20 random books from the database
            var randomBooks = _context.Books
                                        .OrderBy(b => Guid.NewGuid())
                                        .Take(20)
                                        .AsQueryable();

            // filter books
            if (!string.IsNullOrEmpty(filterBy))
            {
                randomBooks = randomBooks.Where(b =>
                    b.Title.Contains(filterBy) ||
                    b.Author.Contains(filterBy) ||
                    (filterBy.Equals("available", StringComparison.OrdinalIgnoreCase) && b.Availability)
                );
            }

            // sort books
            if (!string.IsNullOrEmpty(sortBy))
            {
                randomBooks = sortBy.ToLower() switch
                {
                    "title" => randomBooks.OrderBy(b => b.Title),
                    "author" => randomBooks.OrderBy(b => b.Author),
                    "availability" => randomBooks.OrderBy(b => b.Availability),
                    _ => randomBooks
                };
            }

            return await randomBooks.ToListAsync();
        }

        public async Task<Book?> UpdateAsync(int id, UpdateBookDto bookDto)
        {
            var exisitingBook = await _context.Books.FirstOrDefaultAsync(x => x.Id == id);

            if (exisitingBook == null)
            {
                return null;
            }

            exisitingBook.ISBN = bookDto.ISBN;
            exisitingBook.Title = bookDto.Title;
            exisitingBook.Author = bookDto.Author;
            exisitingBook.Description = bookDto.Description;
            exisitingBook.CoverImage = bookDto.CoverImage;
            exisitingBook.Publisher = bookDto.Publisher;
            exisitingBook.PublicationDate = bookDto.PublicationDate;
            exisitingBook.Category = bookDto.Category;
            exisitingBook.PageCount = bookDto.PageCount;
            exisitingBook.Availability = bookDto.Availability;

            await _context.SaveChangesAsync();

            return exisitingBook;

        }

        public async Task<Book?> DeleteAsync(int id)
        {
            var bookModel = await _context.Books.FirstOrDefaultAsync(x => x.Id == id);

            if (bookModel == null)
            {
                return null;
            }

            _context.Books.Remove(bookModel);
            await _context.SaveChangesAsync();
            return bookModel;
        }
    }
}