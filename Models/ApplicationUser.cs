using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace BookReviews.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<ReviewVote> Votes { get; set; } = new List<ReviewVote>();
    }
}