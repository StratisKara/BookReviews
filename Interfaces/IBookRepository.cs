using BookReviews.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookReviews.Interfaces
{
    public interface IBookRepository
    {
        // Basic CRUD operations
        Task<Book> GetByIdAsync(int id, bool includeReviews = false);
        Task<List<Book>> GetAllAsync(string? genreFilter = null, int? yearFilter = null);
        Task AddAsync(Book book);
        Task UpdateAsync(Book book);
        Task<bool> DeleteAsync(int id);  // Returns success status

        // Extended functionality
        Task<bool> ExistsAsync(int id);
        Task<List<Book>> GetTopRatedBooksAsync(int minRating = 4, int count = 10);
        Task<int> GetCountAsync();
    }
}