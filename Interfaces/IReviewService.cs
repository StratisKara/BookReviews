using BookReviews.Models;
using System.Threading.Tasks;

namespace BookReviews.Interfaces
{
    public interface IReviewService
    {
        Task<ReviewSubmissionResult> SubmitReviewAsync(Review review, string userId);
        Task<VoteResult> ProcessVoteAsync(int reviewId, string userId, bool isUpvote);
        Task<bool> DeleteReviewAsync(int reviewId, string userId);
        Task<bool> UserHasReviewedBookAsync(string userId, int bookId);
    }

    public record ReviewSubmissionResult(bool Success, Review? Review, string? ErrorMessage);
    public record VoteResult(bool Success, int? NewVoteCount, string? ErrorMessage);
}