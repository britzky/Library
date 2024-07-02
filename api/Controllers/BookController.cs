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
            return Ok(book.ToBookDto());
        }

        [HttpGet("featured")]
        public IActionResult GetFeaturedBooks([FromQuery] string? sortBy, [FromQuery] string? filterBy)
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

            var featuredBooks = randomBooks.Select(b => new FeaturedBookDto
            {
                Title = b.Title,
                Author = b.Author,
                Description = b.Description,
                CoverImage = b.CoverImage
            }).ToList();

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

        [HttpPost]
        public IActionResult Create([FromBody] CreateBookDto bookDto)
        {
            var bookModel = bookDto.ToCreateBookDto();
            _context.Books.Add(bookModel);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetById), new { id = bookModel.Id}, bookModel.ToBookDto());
        }
    }
}