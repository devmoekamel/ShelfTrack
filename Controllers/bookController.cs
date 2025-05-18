using BookStore.DTO;
using BookStore.Interfaces;
using BookStore.Models;
using BookStore.Reporisatory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class bookController : ControllerBase
    {

        private readonly IBookReporisatory bookrepo;
        private readonly IPurchaseRepository ipurchase;
        private readonly IReviewRepository reivew;
        public bookController(IBookReporisatory bookrepo, IPurchaseRepository ipurchase)
        {
            this.bookrepo = bookrepo;
            this.ipurchase= ipurchase;
            this.reivew = reivew;
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult add(BookDTO bookdata)
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
            bookrepo.Add(book);
            bookrepo.Save();
            return Ok(book);



        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var books = bookrepo.GetAll();

            var bookList = books.Select(b => new BookDTO
            {
                Id=b.Id,
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
        public IActionResult Update(int id, BookDTO bookdata)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingBook = bookrepo.GetById(id);
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

            bookrepo.Update(id,existingBook);
            bookrepo.Save();

            return Ok(existingBook);
        }


        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var book = bookrepo.GetById(id);
            if (book == null)
            {
                return NotFound("Book not found");
            }

            bookrepo.RemoveByObj(book);  
            bookrepo.Save();

            return Ok(new { message = "Book deleted successfully" });
        }

        [HttpGet("filter")]
        public IActionResult FilterBooks(double? minPrice, double? maxPrice, int? categoryId)
        {
            var books = bookrepo.GetAll().ToList();

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

            List<BookDTO> result = new List<BookDTO>();

            foreach (var b in books)
            {
                result.Add(new BookDTO
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
                });
            }

            return Ok(result);
        }

        [HttpGet("sortbyprice")]
        public IActionResult SortByPrice()
        {
            var books = bookrepo.GetAll().OrderBy(b => b.Price).ToList();

            List<BookDTO> result = new List<BookDTO>();

            foreach (var b in books)
            {
                result.Add(new BookDTO
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
                });
            }

            return Ok(result);
        }

        [HttpGet("freebooks")]
        public IActionResult GetFreeBooks()
        {
            var books = bookrepo.GetAll().Where(b => b.Price == 0).ToList();

            List<BookDTO> result = new List<BookDTO>();

            foreach (var b in books)
            {
                result.Add(new BookDTO
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
                });
            }

            return Ok(result);
        }

        [HttpGet("paginated")]
        public IActionResult GetPaginatedBooks(int pageNumber = 1, int pageSize = 5)
        {
            var books = bookrepo.GetAll()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            List<BookDTO> result = new List<BookDTO>();

            foreach (var b in books)
            {
                result.Add(new BookDTO
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
                });
            }

            return Ok(result);
        }

        [HttpGet("top5rated")]
        public IActionResult Top5RatedBooks()
        {
            var reviews = reivew?.GetAll();
            if (reviews == null)
            {
                return NotFound("No reviews found.");
            }

            var goodReviews = reviews
                .Where(r => r.Rating == 5 || r.Rating == 4 || r.Rating == 3)
                .OrderByDescending(r => r.Rating)
                .Take(5)
                .ToList();

            var books = new List<BookDTO>();

            foreach (var review in goodReviews)
            {
                var book = bookrepo?.GetById(review.BookId);
                if (book != null) // check if book is found
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
