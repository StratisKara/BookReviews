using BookReviews.Database;
using BookReviews.Interfaces;
using BookReviews.Models;
using BookReviews.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BookReviews.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IBookService _bookService;
        private readonly IReviewRepository _reviewRepo;
        private readonly IReviewService _reviewService;

        public BooksController(ApplicationDbContext context, IBookService bookService, IReviewRepository reviewRepository, IReviewService reviewService)
        {
            _context = context;
            _bookService = bookService;
            _reviewRepo = reviewRepository;
            _reviewService = reviewService;
        }

        // GET: Books
        public async Task<IActionResult> Index()
        {
            return View(await _context.Books.ToListAsync());
        }

        // GET: Books/Details/5
        // GET: Books/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var book = await _bookService.GetBookDetailsAsync(id);
            if (book == null) return NotFound();

            var vm = new BookDetailsViewModel
            {
                Book = book,
                Reviews = await _reviewRepo.GetByBookIdAsync(id),
                InputReview = new Review()
            };

            return View(vm);
        }



        // GET: Books/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Author,PublishedYear,Genre")] Book book)
        {
            if (ModelState.IsValid)
            {
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var book = await _context.Books.FindAsync(id);
            if (book == null) return NotFound();

            return View(book);
        }


        // POST: Books/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Author,PublishedYear,Genre")] Book book)
        {
            if (id != book.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(book);

            try
            {
                _context.Update(book);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(book.Id))
                    return NotFound();
                else
                    throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.Id == id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(BookDetailsViewModel vm)
        {

            try
            {
                if (!User.Identity.IsAuthenticated)
                    return RedirectToAction("Login", "Account");

                // Re-fetch the book to repopulate full details
                vm.Book = await _bookService.GetBookDetailsAsync(vm.Book.Id)
                    ?? throw new InvalidOperationException("Book not found");

                if (!ModelState.IsValid)
                {
                    // Optional: log errors for debugging
                    //foreach (var kvp in ModelState)
                    //{
                    //    foreach (var error in kvp.Value.Errors)
                    //    {
                    //        Console.WriteLine($"Field: {kvp.Key}, Error: {error.ErrorMessage}");
                    //    }
                    //}

                    vm.Reviews = await _reviewRepo.GetByBookIdAsync(vm.Book.Id);
                    return View(vm);
                }

                vm.InputReview.BookId = vm.Book.Id;

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var result = await _reviewService.SubmitReviewAsync(vm.InputReview, userId);

                if (!result.Success)
                {
                    ModelState.AddModelError("", result.ErrorMessage);
                    vm.Reviews = await _reviewRepo.GetByBookIdAsync(vm.Book.Id);
                    return View(vm);
                }

                return RedirectToAction(nameof(Details), new { id = vm.Book.Id });
            }
            catch (Exception ex)
            {
                // Log the error (you can also use a real logger like ILogger)
                Console.WriteLine($"Error in Details POST: {ex.Message}");

                ModelState.AddModelError("", "An unexpected error occurred while submitting your review.");
                vm.Reviews = await _reviewRepo.GetByBookIdAsync(vm.Book.Id);
                return View(vm);
            }
        }


    }
}
