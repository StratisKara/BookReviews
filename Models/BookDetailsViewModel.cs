using BookReviews.Models;
using Microsoft.AspNetCore.Mvc;


namespace BookReviews.Models
{
    public class BookDetailsViewModel
    {
        public required Book Book { get; set; }
        public IEnumerable<Review> Reviews { get; set; } = Enumerable.Empty<Review>();
        public Review InputReview { get; set; } = new Review();
    }
}


