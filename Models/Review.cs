using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookReviews.Models
{
    public class Review
    {
        public int Id { get; set; }

        [Required]
        [StringLength(2000)]
        public string? Content { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        public int BookId { get; set; }
        public Book? Book { get; set; }

        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }

        public ICollection<ReviewVote> Votes { get; set; } = new List<ReviewVote>();
    }
}