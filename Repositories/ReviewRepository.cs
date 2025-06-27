using BookReviews.Database;
using BookReviews.Interfaces;
using BookReviews.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookReviews.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly ApplicationDbContext _context;

        public ReviewRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Review?> GetByIdAsync(int id)
        {
            return await _context.Reviews
                .Include(r => r.User)
                .Include(r => r.Votes)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<List<Review>> GetByBookIdAsync(int bookId)
        {
            try
            {
                return await _context.Reviews
                    .Where(r => r.BookId == bookId)
                    .Include(r => r.User)
                    .Include(r => r.Votes)
                    .OrderByDescending(r => r.DateCreated)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                // Log the exception (consider using ILogger)
                Console.WriteLine($"Error retrieving reviews: {ex.Message}");

                // Return an empty list or handle accordingly
                return new List<Review>();
            }
        }


        public async Task AddAsync(Review review)
        {
            if (review == null) throw new ArgumentNullException(nameof(review));

            await _context.Reviews.AddAsync(review);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Review review)
        {
            if (review == null) throw new ArgumentNullException(nameof(review));

            _context.Reviews.Update(review);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null) return false;

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddVoteAsync(int reviewId, string userId, bool isUpvote)
        {
            // Check for existing vote
            var existingVote = await _context.ReviewVotes
                .FirstOrDefaultAsync(v => v.ReviewId == reviewId && v.UserId == userId);

            if (existingVote != null) return false;

            var vote = new ReviewVote
            {
                ReviewId = reviewId,
                UserId = userId,
                IsUpvote = isUpvote
            };

            await _context.ReviewVotes.AddAsync(vote);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveVoteAsync(int reviewId, string userId)
        {
            var vote = await _context.ReviewVotes
                .FirstOrDefaultAsync(v => v.ReviewId == reviewId && v.UserId == userId);

            if (vote == null) return false;

            _context.ReviewVotes.Remove(vote);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> GetVoteCountAsync(int reviewId)
        {
            return await _context.ReviewVotes
                .Where(v => v.ReviewId == reviewId)
                .SumAsync(v => v.IsUpvote ? 1 : -1);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Reviews.AnyAsync(r => r.Id == id);
        }

        public async Task<bool> UserHasReviewedBookAsync(string userId, int bookId)
        {
            return await _context.Reviews
                .AnyAsync(r => r.UserId == userId && r.BookId == bookId);
        }
    }
}