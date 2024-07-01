using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/book")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly AppDBContext _context;
        public BookController(AppDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var books = _context.Books.ToList();

            return Ok(books);
        }

        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int id)
        {
            var book = _context.Books.Find(id);

            if (book == null)
            {
                return NotFound();
            }
            return Ok(book.ToBookDetailDto());
        }

        [HttpGet("featured")]
        public IActionResult GetFeaturedBooks([FromQuery] string? sortBy, [FromQuery] string? filterBy)
        {
            var query = _context.Books.AsQueryable();

            // filter books
            if (!string.IsNullOrEmpty(filterBy))
            {
                query = query.Where(b =>
                    b.Title.Contains(filterBy) ||
                    b.Author.Contains(filterBy) ||
                    (filterBy.Equals("available", StringComparison.OrdinalIgnoreCase) && b.Availability)
                );
            }

            // sort books
            if (!string.IsNullOrEmpty(sortBy))
            {
                query = sortBy.ToLower() switch
                {
                    "title" => query.OrderBy(b => b.Title),
                    "author" => query.OrderBy(b => b.Author),
                    "availability" => query.OrderBy(b => b.Availability),
                    _ => query
                };
            }

            // get 5 random books
            var featuredBooks = query
                .OrderBy(b => Guid.NewGuid())
                .Take(5)
                .Select(b => b.ToFeaturedBookDto())
                .ToList();

            return Ok(featuredBooks);
        }

        [HttpGet("search")]
        public IActionResult SearchBooks([FromQuery] string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return BadRequest("Search term is required");
            }

            var books = _context.Books
                .Where(b => b.Title.Contains(searchTerm))
                .Select(b => b.ToFeaturedBookDto())
                .ToList();

            return Ok(books);
        }
    }
}