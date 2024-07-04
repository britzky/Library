using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using api.Dtos.Transaction;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/transaction")]
    [ApiController]
    // [Authorize]
    public class BookTransactionController : ControllerBase
    {
        private readonly IBookTransactionRepository _bookTransactionRepository;
        private readonly IBookRepository _bookRepository;
        private readonly UserManager<User> _userManager;

        public BookTransactionController(IBookTransactionRepository bookTransactionRepository, IBookRepository bookRepository, UserManager<User> userManager)
        {
            _bookTransactionRepository = bookTransactionRepository;
            _bookRepository = bookRepository;
            _userManager = userManager;
        }

        [HttpPost("checkout/{bookId}")]
        // [Authorize(Roles = "Customer")]
        public async Task<IActionResult> CheckoutBook(int bookId)
        {
            var book = await _bookRepository.GetByIdAsync(bookId);;
            if (book == null || !book.Availability)
            {
                return BadRequest("Book is not available for checkout.");
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }

            var transaction = new BookTransaction
            {
                BookId = bookId,
                UserId = userId,
                CheckoutDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(5),
                IsReturned = false
            };

            var availabilityDto = new AvailabilityUpdateDto { Availability = false };
            await _bookRepository.UpdateAvailabilityAsync(bookId, availabilityDto);
            await _bookTransactionRepository.UpdateTransactionAsync(transaction);

            return Ok();

        }

        [HttpPost("customer-return/{transactionId}")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> ReturnToLibrary(int transactionId)
        {
            var transaction = await _bookTransactionRepository.GetTransactionByIdAsync(transactionId);
            if (transaction == null || transaction.IsReturned)
            {
                return BadRequest("Invalid transaction or book already returned.");
            }

            transaction.IsReturned = true;
            await _bookTransactionRepository.UpdateTransactionAsync(transaction);

           return Ok();
        }

        [HttpPost("librarian-return/{transactionId}")]
        [Authorize(Roles = "Librarian")]
        public async Task<IActionResult> ReturnToCirculation(int transactionId)
        {
            var transaction = await _bookTransactionRepository.GetTransactionByIdAsync(transactionId);
            if (transaction == null || !transaction.IsReturned)
            {
                return BadRequest("Invalud transaction or book not ready for return");
            }

            var book = await _bookRepository.GetByIdAsync(transaction.BookId);
            if (book == null)
            {
                return BadRequest("Book not found.");
            }

            var availabilityDto = new AvailabilityUpdateDto { Availability = true };
            await _bookRepository.UpdateAvailabilityAsync(book.Id, availabilityDto);
            await _bookTransactionRepository.UpdateTransactionAsync(transaction);

            return Ok();
        }
    }
}