using System.Collections.Generic;

namespace BookReviews.Models
{
    public class BookListViewModel
    {
        public List<Book> Books { get; set; } = new();
        public string? SelectedGenre { get; set; }
        public int? SelectedYear { get; set; }
        public string? SelectedAuthor { get; set; }  // new

        // Dropdown sources
        public List<string> Genres { get; set; } = new();
        public List<int> Years { get; set; } = new();
        public List<string> Authors { get; set; } = new();  // new
    }

}
