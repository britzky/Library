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

    }
}