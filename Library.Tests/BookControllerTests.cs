using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using api.Controllers;
using api.Data;
using api.Models;
using api.Dtos;
using Bogus;

namespace Library.Tests
{
    public class BookControllerTests
    {
        private readonly BookController _controller;
        private readonly AppDBContext _context;
        public BookControllerTests()
        {
            var options = new DbContextOptionsBuilder<AppDBContext>()
                .UseInMemoryDatabase(databaseName: "BookTestDb")
                .Options;

            _context = new AppDBContext(options);
            _controller = new BookController(_context);

            var seeder = new DataSeeder(_context);
            seeder.SeedBooks();
        }

        // Get By ID Tests

        [Fact]
        public void GetById_BookExists_ReturnsOkResultWithBook()
        {
            var bookId = 1;

            var result = _controller.GetById(bookId);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedBook = Assert.IsType<BookDto>(okResult.Value);
            Assert.Equal(bookId, returnedBook.Id);
        }

        [Fact]
        public void GetById_BookDoesNotExist_ReturnsNotFound()
        {
            var bookId = 101;

            var result = _controller.GetById(bookId);

            Assert.IsType<NotFoundResult>(result);
        }

        // Get Featured Books Tests

        [Fact]
        public void GetFeaturedBooks_NoFilterOrSort_ReturnsFiveRandomBooks()
        {
            var firstResult = _controller.GetFeaturedBooks(null, null);
            var secondResult = _controller.GetFeaturedBooks(null, null);

            var firstOkResult = Assert.IsType<OkObjectResult>(firstResult);
            var secondOkResult = Assert.IsType<OkObjectResult>(secondResult);

            var firstReturnedBooks = Assert.IsType<List<FeaturedBookDto>>(firstOkResult.Value);
            var secondReturnedBooks = Assert.IsType<List<FeaturedBookDto>>(secondOkResult.Value);

            Assert.Equal(5, firstReturnedBooks.Count);
            Assert.Equal(5, secondReturnedBooks.Count);

            bool different = false;
            for (int i = 0; i < 5; i++)
            {
                if (firstReturnedBooks[i].Title != secondReturnedBooks[i].Title ||
                    firstReturnedBooks[i].Author != secondReturnedBooks[i].Author ||
                    firstReturnedBooks[i].Description != secondReturnedBooks[i].Description ||
                    firstReturnedBooks[i].CoverImage != secondReturnedBooks[i].CoverImage)
                {
                    different = true;
                    break;
                }
            }

            Assert.True(different, "The two calls are the same meaning it is not random.");
        }

    }
}