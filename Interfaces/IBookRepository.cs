using BookReviews.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookReviews.Interfaces
{
    public interface IBookRepository
    {
        Task<Book> GetByIdAsync(int id, bool includeReviews = false);
        Task<List<Book>> GetAllAsync(string? genreFilter = null, int? yearFilter = null);
        Task AddAsync(Book book);
        Task UpdateAsync(Book book);
        Task<bool> DeleteAsync(int id);  

        Task<bool> ExistsAsync(int id);
        Task<List<Book>> GetTopRatedBooksAsync(int minRating = 4, int count = 10);
        Task<int> GetCountAsync();
    }
}