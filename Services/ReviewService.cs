using BookReviews.Interfaces;
using BookReviews.Models;
using System;
using System.Threading.Tasks;

namespace BookReviews.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepo;
        private readonly IBookRepository _bookRepo;

        public ReviewService(IReviewRepository reviewRepo, IBookRepository bookRepo)
        {
            _reviewRepo = reviewRepo;
            _bookRepo = bookRepo;
        }

        public async Task<ReviewSubmissionResult> SubmitReviewAsync(Review review, string userId)
        {
            if (review == null)
                return new ReviewSubmissionResult(false, null, "Review cannot be null");

            if (string.IsNullOrEmpty(userId))
                return new ReviewSubmissionResult(false, null, "UserId cannot be empty");

            try
            {
                var hasReviewed = await _reviewRepo.UserHasReviewedBookAsync(userId, review.BookId);
                if (hasReviewed)
                {
                    return new ReviewSubmissionResult(false, null, "You've already reviewed this book");
                }

                if (review.Rating < 1 || review.Rating > 5)
                {
                    return new ReviewSubmissionResult(false, null, "Rating must be between 1-5");
                }

                review.UserId = userId;
                review.DateCreated = DateTime.UtcNow;

                await _reviewRepo.AddAsync(review);
                return new ReviewSubmissionResult(true, review, null);
            }
            catch (Exception ex)
            {
                return new ReviewSubmissionResult(false, null, ex.Message);
            }
        }


        public async Task<VoteResult> ProcessVoteAsync(int reviewId, string userId, bool isUpvote)
        {
            try
            {
                var success = await _reviewRepo.AddVoteAsync(reviewId, userId, isUpvote);
                if (!success)
                {
                    return new VoteResult(false, null, "You've already voted on this review");
                }

                var newCount = await _reviewRepo.GetVoteCountAsync(reviewId);
                return new VoteResult(true, newCount, null);
            }
            catch (Exception ex)
            {
                return new VoteResult(false, null, ex.Message);
            }
        }

        public async Task<bool> DeleteReviewAsync(int reviewId, string userId)
        {
            var review = await _reviewRepo.GetByIdAsync(reviewId);
            if (review == null || review.UserId != userId) return false;

            return await _reviewRepo.DeleteAsync(reviewId);
        }

        public async Task<bool> UserHasReviewedBookAsync(string userId, int bookId)
        {
            return await _reviewRepo.UserHasReviewedBookAsync(userId, bookId);
        }

    }
}