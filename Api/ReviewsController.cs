using BookReviews.Interfaces;
using BookReviews.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BookReviews.Controllers.Api
{
    [Route("api/reviews")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewsController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        /// <summary>
        /// POST /api/reviews
        /// Adds a new review. Requires authentication.
        /// </summary>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Review>> SubmitReview([FromBody] Review review)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var result = await _reviewService.SubmitReviewAsync(review, userId);
            if (!result.Success)
                return BadRequest(result.ErrorMessage);

            return CreatedAtAction(
                nameof(SubmitReview),
                new { id = result.Review!.Id },
                result.Review
            );
        }

        /// <summary>
        /// POST /api/reviews/{id}/vote?isUpvote=true|false
        /// Registers an upvote/downvote on a review. Requires authentication.
        /// </summary>
        [HttpPost("{id}/vote")]
        [Authorize]
        public async Task<ActionResult<int?>> VoteOnReview(int id, [FromQuery] bool isUpvote)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var voteResult = await _reviewService.ProcessVoteAsync(id, userId, isUpvote);
            if (!voteResult.Success)
                return BadRequest(voteResult.ErrorMessage);

            return Ok(voteResult.NewVoteCount);
        }
    }
}
