using BookReviews.Models;
using System.Threading.Tasks;

namespace BookReviews.Interfaces
{
    public interface IReviewRepository
    {
        // Basic CRUD
        Task<Review?> GetByIdAsync(int id);
        Task<List<Review>> GetByBookIdAsync(int bookId);
        Task AddAsync(Review review);
        Task UpdateAsync(Review review);
        Task<bool> DeleteAsync(int id);

        // Voting
        Task<bool> AddVoteAsync(int reviewId, string userId, bool isUpvote);
        Task<bool> RemoveVoteAsync(int reviewId, string userId);
        Task<int> GetVoteCountAsync(int reviewId);

        // Validation
        Task<bool> ExistsAsync(int id);
        Task<bool> UserHasReviewedBookAsync(string userId, int bookId);
    }
}