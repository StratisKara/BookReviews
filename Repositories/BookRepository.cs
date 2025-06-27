using BookReviews.Database;
using BookReviews.Interfaces;
using BookReviews.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookReviews.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly ApplicationDbContext _context;

        public BookRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Book> GetByIdAsync(int id, bool includeReviews = false)
        {
            Book? book;
            if (includeReviews)
            {
                book = await _context.Books
                    .Include(b => b.Reviews)
                    .FirstOrDefaultAsync(b => b.Id == id);
            }
            else
            {
                book = await _context.Books.FindAsync(id);
            }

            if (book == null)
            {
                throw new KeyNotFoundException($"Book with ID {id} not found.");
            }

            return book;
        }


        public async Task<List<Book>> GetAllAsync(string? genreFilter = null, int? yearFilter = null)
        {
            var query = _context.Books.AsQueryable();

            if (!string.IsNullOrEmpty(genreFilter))
            {
                query = query.Where(b => b.Genre == genreFilter);
            }

            if (yearFilter.HasValue)
            {
                query = query.Where(b => b.PublishedYear == yearFilter.Value);
            }

            return await query.ToListAsync();
        }

        public async Task AddAsync(Book book)
        {
            if (book == null) throw new ArgumentNullException(nameof(book));

            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Book book)
        {
            if (book == null) throw new ArgumentNullException(nameof(book));

            _context.Books.Update(book);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null) return false;

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Books.AnyAsync(b => b.Id == id);
        }

        public async Task<List<Book>> GetTopRatedBooksAsync(int minRating = 4, int count = 10)
        {
            return await _context.Books
                .Where(b => b.Reviews.Any())
                .OrderByDescending(b => b.Reviews.Average(r => r.Rating))
                .Take(count)
                .ToListAsync();
        }

        public async Task<int> GetCountAsync()
        {
            return await _context.Books.CountAsync();
        }
    }
}