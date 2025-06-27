using BookReviews.Database;
using BookReviews.Interfaces;
using BookReviews.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookReviews.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepo;
        private readonly IReviewRepository _reviewRepo;

        public BookService(IBookRepository bookRepo, IReviewRepository reviewRepo)
        {
            _bookRepo = bookRepo;
            _reviewRepo = reviewRepo;
        }

        public async Task<Book?> GetBookDetailsAsync(int id)
        {
            return await _bookRepo.GetByIdAsync(id, includeReviews: true);
        }

        public async Task<List<Book>> SearchBooksAsync(string query, string? genre = null)
        {
            var books = await _bookRepo.GetAllAsync(genre);

            return books.Where(b =>
                (b.Title?.Contains(query, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (b.Author?.Contains(query, StringComparison.OrdinalIgnoreCase) ?? false)
            ).ToList();
        }

        public async Task<BookCreationResult> CreateBookAsync(Book book, string userId)
        {
            try
            {
                // Business rule: Prevent duplicate titles
                var existingBooks = await _bookRepo.GetAllAsync();
                if (existingBooks.Any(b =>
                    b.Title != null && b.Title.Equals(book.Title, StringComparison.OrdinalIgnoreCase) &&
                    b.Author != null && b.Author.Equals(book.Author, StringComparison.OrdinalIgnoreCase)))
                {
                    return new BookCreationResult(false, null, "Book with the same title and author already exists");
                }


                await _bookRepo.AddAsync(book);
                return new BookCreationResult(true, book, null);
            }
            catch (Exception ex)
            {
                // Log error here
                return new BookCreationResult(false, null, ex.Message);
            }
        }

        public async Task<bool> DeleteBookAsync(int id, string userId)
        {
            var book = await _bookRepo.GetByIdAsync(id);
            if (book == null) return false;

            // Business rule: Only allow deletion if no reviews exist
            var hasReviews = await _reviewRepo.GetByBookIdAsync(id);
            if (hasReviews.Any()) return false;

            return await _bookRepo.DeleteAsync(id);
        }

        public async Task<List<Review>> GetReviewsForBookAsync(int bookId)
        {
            return await _reviewRepo.GetByBookIdAsync(bookId);
        }

    }
}