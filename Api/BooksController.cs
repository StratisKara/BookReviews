using BookReviews.Interfaces;
using BookReviews.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BookReviews.Controllers.Api
{
    [Authorize]
    [Route("api/books")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        // GET: api/books
        [HttpGet]
        public async Task<ActionResult<List<Book>>> GetBooks([FromQuery] string query, [FromQuery] string? genre)
        {
            var books = await _bookService.SearchBooksAsync(query, genre);
            if (books == null || books.Count == 0)
                return NotFound("No books found matching the criteria.");
            return Ok(books);
        }

        // GET: api/books/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            var book = await _bookService.GetBookDetailsAsync(id);
            if (book == null)
                return NotFound($"Book with ID {id} not found.");
            return Ok(book);
        }

        // POST: api/books
        [HttpPost]
        public async Task<ActionResult<Book>> CreateBook([FromBody] Book book)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("You must be logged in to create a book.");

            var result = await _bookService.CreateBookAsync(book, userId);

            if (!result.Success)
                return BadRequest(result.ErrorMessage);

            return CreatedAtAction(nameof(GetBook), new { id = result.Book!.Id }, result.Book);
        }


        // GET: api/books/{id}/reviews
        [HttpGet("{id}/reviews")]
        public async Task<ActionResult<List<Review>>> GetReviewsForBook(int id)
        {
            var reviews = await _bookService.GetReviewsForBookAsync(id);
            if (reviews == null || reviews.Count == 0)
                return NotFound("No reviews found for this book.");
            return Ok(reviews);
        }
    }
}
