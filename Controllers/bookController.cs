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
        public bookController(IBookReporisatory bookrepo, IPurchaseRepository ipurchase)
        {
            this.bookrepo = bookrepo;
            this.ipurchase= ipurchase;
        }
        //[Authorize(Roles = "Admin")]
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
        //[Authorize(Roles = "User")]

        [HttpGet,Authorize(Roles ="User")]
        public IActionResult GetAll()
        {
            var books = bookrepo.GetAll();

            var bookList = books.Select(b => new BookDTO
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

        [HttpPost("buy/{id}")]
        //[Authorize(Roles = "User")]
        public IActionResult Buy(int id)
        {
            var book = bookrepo.GetById(id);
            if (book == null)
            {
                return NotFound("Book not found");
            }

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var purchase = new Purchase
            {
                BookId = id,
                UserId = userId,
                PurchaseDate = DateTime.Now
            };

            ipurchase.Add(purchase);
            ipurchase.Save();

            return Ok(new { message = "Purchase recorded successfully", book });
        }
        ////[Authorize(Roles = "Admin")]
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


    }
}
