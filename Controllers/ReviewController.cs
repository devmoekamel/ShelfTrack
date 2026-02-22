using BookStore.DTO;
using BookStore.Services;
using BookStore.Models;
using BookStore.context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BookStore.Controllers
{
    [Authorize(Roles = "User")]
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;
        private readonly BookStoreContext _context;

        public ReviewController(IReviewService reviewService, BookStoreContext context)
        {
            _reviewService = reviewService;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> AddReview([FromBody] ReviewDTO reviewDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("User not authenticated");
            }

            var review = new Review
            {
                UserId = userId,
                BookId = reviewDTO.BookId,
                Rating = reviewDTO.Rating,
                Comment = reviewDTO.Comment,
                ReviewDate = DateTime.Now
            };

            await _reviewService.AddAsync(review);
            await _reviewService.SaveAsync();

            return Ok(reviewDTO);
        }

        [HttpPut("{id:int}")]
        [Authorize]
        public async Task<IActionResult> UpdateReview(int id, [FromBody] UpdateReviewDTO updateDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("User not authenticated");
            }

            var existingReview = await _reviewService.GetByIdAsync(id);
            if (existingReview == null)
                return NotFound("Review not found");

            if (existingReview.UserId != userId)
                return Forbid("You can only update your own review");

            if (updateDTO.Rating.HasValue)
                existingReview.Rating = updateDTO.Rating.Value;

            if (!string.IsNullOrWhiteSpace(updateDTO.Comment))
                existingReview.Comment = updateDTO.Comment;

            existingReview.ReviewDate = DateTime.Now;

            _reviewService.Update(existingReview);
            await _reviewService.SaveAsync();

            return Ok(existingReview);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllReview()
        {
            var reviews = await _context.Reviews
                .Where(r => !r.IsDeleted)
                .Include(r => r.Book)
                .Include(r => r.User)
                .Select(r => new
                {
                    Id = r.Id,
                    BookTitle = r.Book.Title,
                    UserName = r.User.UserName,
                    Rating = r.Rating,
                    Comment = r.Comment,
                    ReviewDate = r.ReviewDate
                })
                .ToListAsync();

            return Ok(reviews);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var review = await _context.Reviews
                .Where(r => !r.IsDeleted)
                .Include(r => r.Book)
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (review == null)
                return NotFound();

            var result = new
            {
                Id = review.Id,
                BookTitle = review.Book.Title,
                UserName = review.User.UserName,
                Rating = review.Rating,
                Comment = review.Comment,
                ReviewDate = review.ReviewDate
            };

            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> RemoveReview(int id)
        {
            var review = await _reviewService.GetByIdAsync(id);
            if (review == null)
            {
                return NotFound("Review not found");
            }

            _reviewService.DeleteById(id);
            await _reviewService.SaveAsync();

            return Ok("Review Deleted succefully");
        }

        [HttpGet("book/{bookId:int}")]
        public async Task<IActionResult> GetReviewsByBook(int bookId)
        {
            var reviews = await _context.Reviews
                .Where(r => r.BookId == bookId && !r.IsDeleted)
                .Select(r => new DisplayReviewDTO
                {
                    Id = r.Id,
                    Rate = r.Rating,
                    Comment = r.Comment,
                    ReviewDate = r.ReviewDate
                }).ToListAsync();

            return Ok(reviews);
        }

        [HttpGet("top-books")]
        public async Task<IActionResult> GetTopRatedBooks()
        {
            var topBooks = await _context.Reviews
                .Where(r => !r.IsDeleted)
                .GroupBy(r => r.BookId)
                .Select(g => new
                {
                    BookId = g.Key,
                    AverageRating = g.Average(r => r.Rating),
                    ReviewCount = g.Count(),
                    BookTitle = g.First().Book.Title
                })
                .OrderByDescending(b => b.AverageRating)
                .ThenByDescending(b => b.ReviewCount)
                .Take(5)
                .ToListAsync();

            return Ok(topBooks);
        }
    }
}
