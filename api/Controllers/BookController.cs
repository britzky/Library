using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/book")]
    [ApiController]
    [Authorize]
    public class BookController : ControllerBase
    {
        private readonly AppDBContext _context;
        private readonly IBookRepository _bookRepo;
        public BookController(AppDBContext context, IBookRepository bookRepo)
        {
            _bookRepo = bookRepo;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var books = await _bookRepo.GetAllAsync();

            var bookDto = books.Select(s => s.ToBookDto());

            return Ok(books);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var book = await _bookRepo.GetByIdAsync(id);

            if (book == null)
            {
                return NotFound();
            }
            return Ok(book.ToBookDto());
        }

        [HttpGet("featured")]
        public async Task<IActionResult> GetFeaturedBooks([FromQuery] string? sortBy, [FromQuery] string? filterBy)
        {
            var featuredBooks = await _bookRepo.GetFeaturedBooksAsync(sortBy, filterBy);
            var featuredBookDtos = featuredBooks.Select(b => new FeaturedBookDto
            {
                Title = b.Title,
                Author = b.Author,
                Description = b.Description,
                CoverImage = b.CoverImage
            }).ToList();

            return Ok(featuredBooks);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchBooks([FromQuery] string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return BadRequest("Search term is required");
            }

            var books = await _bookRepo.SearchAsync(searchTerm);
            var featuredBookDtos = books.Select(b => b.ToFeaturedBookDto());

            return Ok(featuredBookDtos);
        }

        [HttpPost]
        [Authorize(Roles = "Librarian")]
        public async Task<IActionResult> Create([FromBody] CreateBookDto bookDto)
        {
            var bookModel = bookDto.ToCreateBookDto();
            await _bookRepo.CreateAsync(bookModel);
            return CreatedAtAction(nameof(GetById), new { id = bookModel.Id}, bookModel.ToBookDto());
        }

        [HttpPut]
        [Route("{id}")]
        [Authorize(Roles = "Librarian")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateBookDto updateDto)
        {
            var bookModel = await _bookRepo.UpdateAsync(id, updateDto);

            if (bookModel == null)
            {
                return NotFound();
            }

            return Ok(bookModel.ToBookDto());
        }

        [HttpDelete]
        [Route("{id}")]
        [Authorize(Roles = "Librarian")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var bookModel = await _bookRepo.DeleteAsync(id);
            if (bookModel == null)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}