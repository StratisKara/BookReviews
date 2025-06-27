using System.ComponentModel.DataAnnotations.Schema;

namespace BookReviews.Models
{
    public class ReviewVote
    {
        public int Id { get; set; }
        public bool IsUpvote { get; set; }

        // Relationships
        public int ReviewId { get; set; }
        public Review? Review { get; set; }

        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }
    }
}   