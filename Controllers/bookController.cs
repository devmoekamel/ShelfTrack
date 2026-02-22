using BookStore.DTO;
using BookStore.Services;
using BookStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class bookController : ControllerBase
    {
        private readonly IBookService _bookService;
        private readonly IPurchaseService _purchaseService;
        private readonly IReviewService _reviewService;

        public bookController(IBookService bookService, IPurchaseService purchaseService, IReviewService reviewService)
        {
            _bookService = bookService;
            _purchaseService = purchaseService;
            _reviewService = reviewService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> add(BookDTO bookdata)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var book = new Book
            {
                Title = bookdata.Title,
                Author = bookdata.Author,
                Description = bookdata.Description,
                Genre = bookdata.Genre,
                ISBN = bookdata.ISBN,
                PageCount = bookdata.PageCount,
                Price = bookdata.Price,
                CoverImageURL = bookdata.CoverImageURL,
                PublishedDate = bookdata.PublishedDate,
                Categoryid = bookdata.Categoryid
            };
            await _bookService.AddAsync(book);
            await _bookService.SaveAsync();
            return Ok(book);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var books = await _bookService.GetAllAsync();

            var bookList = books.Select(b => new BookDTO
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author,
                Description = b.Description,
                Genre = b.Genre,
                ISBN = b.ISBN,
                PageCount = b.PageCount,
                Price = b.Price,
                CoverImageURL = b.CoverImageURL,
                PublishedDate = b.PublishedDate,
                Categoryid = b.Categoryid
            });

            return Ok(bookList);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, BookDTO bookdata)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingBook = await _bookService.GetByIdAsync(id);
            if (existingBook == null)
            {
                return NotFound();
            }

            existingBook.Title = bookdata.Title;
            existingBook.Author = bookdata.Author;
            existingBook.Description = bookdata.Description;
            existingBook.Genre = bookdata.Genre;
            existingBook.ISBN = bookdata.ISBN;
            existingBook.PageCount = bookdata.PageCount;
            existingBook.Price = bookdata.Price;
            existingBook.CoverImageURL = bookdata.CoverImageURL;
            existingBook.PublishedDate = bookdata.PublishedDate;
            existingBook.Categoryid = bookdata.Categoryid;

            _bookService.Update(existingBook);
            await _bookService.SaveAsync();

            return Ok(existingBook);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var book = await _bookService.GetByIdAsync(id);
            if (book == null)
            {
                return NotFound("Book not found");
            }

            _bookService.Delete(book);
            await _bookService.SaveAsync();

            return Ok(new { message = "Book deleted successfully" });
        }

        [HttpGet("filter")]
        public async Task<IActionResult> FilterBooks(double? minPrice, double? maxPrice, int? categoryId)
        {
            var books = (await _bookService.GetAllAsync()).ToList();

            if (minPrice != null)
            {
                books = books.Where(b => b.Price >= minPrice).ToList();
            }

            if (maxPrice != null)
            {
                books = books.Where(b => b.Price <= maxPrice).ToList();
            }

            if (categoryId != null)
            {
                books = books.Where(b => b.Categoryid == categoryId).ToList();
            }

            List<BookDTO> result = books.Select(b => new BookDTO
            {
                Title = b.Title,
                Author = b.Author,
                Description = b.Description,
                Genre = b.Genre,
                ISBN = b.ISBN,
                PageCount = b.PageCount,
                Price = b.Price,
                CoverImageURL = b.CoverImageURL,
                PublishedDate = b.PublishedDate,
                Categoryid = b.Categoryid
            }).ToList();

            return Ok(result);
        }

        [HttpGet("sortbyprice")]
        public async Task<IActionResult> SortByPrice()
        {
            var books = (await _bookService.GetAllAsync()).OrderBy(b => b.Price).ToList();

            var result = books.Select(b => new BookDTO
            {
                Title = b.Title,
                Author = b.Author,
                Description = b.Description,
                Genre = b.Genre,
                ISBN = b.ISBN,
                PageCount = b.PageCount,
                Price = b.Price,
                CoverImageURL = b.CoverImageURL,
                PublishedDate = b.PublishedDate,
                Categoryid = b.Categoryid
            }).ToList();

            return Ok(result);
        }

        [HttpGet("freebooks")]
        public async Task<IActionResult> GetFreeBooks()
        {
            var books = (await _bookService.GetAllAsync()).Where(b => b.Price == 0).ToList();

            var result = books.Select(b => new BookDTO
            {
                Title = b.Title,
                Author = b.Author,
                Description = b.Description,
                Genre = b.Genre,
                ISBN = b.ISBN,
                PageCount = b.PageCount,
                Price = b.Price,
                CoverImageURL = b.CoverImageURL,
                PublishedDate = b.PublishedDate,
                Categoryid = b.Categoryid
            }).ToList();

            return Ok(result);
        }

        [HttpGet("paginated")]
        public async Task<IActionResult> GetPaginatedBooks(int pageNumber = 1, int pageSize = 5)
        {
            var allBooks = await _bookService.GetAllAsync();
            var books = allBooks
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var result = books.Select(b => new BookDTO
            {
                Title = b.Title,
                Author = b.Author,
                Description = b.Description,
                Genre = b.Genre,
                ISBN = b.ISBN,
                PageCount = b.PageCount,
                Price = b.Price,
                CoverImageURL = b.CoverImageURL,
                PublishedDate = b.PublishedDate,
                Categoryid = b.Categoryid
            }).ToList();

            return Ok(result);
        }

        [HttpGet("top5rated")]
        public async Task<IActionResult> Top5RatedBooks()
        {
            var reviews = await _reviewService.GetAllAsync();
            
            if (reviews == null || !reviews.Any())
            {
                return NotFound("No reviews found.");
            }

            var goodReviews = reviews
                .Where(r => r.Rating >= 3)
                .OrderByDescending(r => r.Rating)
                .Take(5)
                .ToList();

            var books = new List<BookDTO>();

            foreach (var review in goodReviews)
            {
                var book = await _bookService.GetByIdAsync(review.BookId);
                if (book != null)
                {
                    books.Add(new BookDTO
                    {
                        Title = book.Title,
                        Author = book.Author,
                        Description = book.Description,
                        Genre = book.Genre,
                        ISBN = book.ISBN,
                        PageCount = book.PageCount,
                        Price = book.Price,
                        CoverImageURL = book.CoverImageURL,
                        PublishedDate = book.PublishedDate,
                        Categoryid = book.Categoryid
                    });
                }
            }

            if (books.Count == 0)
            {
                return NotFound("No books found with high ratings.");
            }

            return Ok(books);
        }
    }
}
