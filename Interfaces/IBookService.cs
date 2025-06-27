using BookReviews.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookReviews.Interfaces
{
    public interface IBookService
    {
        Task<Book?> GetBookDetailsAsync(int id);
        Task<List<Book>> SearchBooksAsync(string query, string? genre = null);
        Task<BookCreationResult> CreateBookAsync(Book book, string userId);
        Task<bool> DeleteBookAsync(int id, string userId);
        Task<List<Review>> GetReviewsForBookAsync(int bookId);

    }

    public record BookCreationResult(bool Success, Book? Book, string? ErrorMessage);
}