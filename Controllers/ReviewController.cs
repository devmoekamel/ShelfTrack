//using BookStore.Interfaces;
//using BookStore.Models;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;

//namespace BookStore.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class ReviewController : ControllerBase
//    {
//        private readonly IReviewRepository reviewRepository;
//        public ReviewController(IReviewRepository reviewRepository)
//        {
//           this.reviewRepository = reviewRepository;
//        }

//        //add review
//        [HttpPost]
//        public IActionResult AddReview([FromBody]Review review)
//        {
//            if (review == null)
//                return BadRequest();

//            review.ReviewDate = DateTime.Now;
//            reviewRepository.Add(review);
//            reviewRepository.Save();
//            return Ok(review);
//        }

//        //Get all review
//        [HttpGet]
//        public IActionResult GetAllReview()
//        {
//            var reviews = reviewRepository.GetAll();
//            return Ok(reviews);
//        }

//        //GetReview by Id
//        [HttpGet("{id:int}")]
//        public IActionResult GetById(int id)
//        {
//            var review = reviewRepository.GetById(id);
//            if (review == null)
//                return NotFound();
//            return Ok(review);
//        }

//        //
//    }
//}












using BookStore.DTO;
using BookStore.Interfaces;
using BookStore.Models;
using BookStore.Reporisatory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewRepository reviewRepository;

        public ReviewController(IReviewRepository reviewRepository)
        {
            this.reviewRepository = reviewRepository;
        }

        // POST: api/Review
        [HttpPost]
       // [Authorize] 
        public IActionResult AddReview([FromBody] ReviewDTO reviewDTO)
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

            reviewRepository.Add(review);
            reviewRepository.Save();

            return Ok(review);
        }

        // PUT: api/Review/{id}
        [HttpPut("{id:int}")]
        [Authorize]
        public IActionResult UpdateReview(int id, [FromBody] UpdateReviewDTO updateDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("User not authenticated");
            }


            var existingReview = reviewRepository.GetById(id);
            if (existingReview == null)
                return NotFound("Review not found");

            if (existingReview.UserId != userId)
                return Forbid("You can only update your own review");

            if (updateDTO.Rating.HasValue)
                existingReview.Rating = updateDTO.Rating.Value;

            if (!string.IsNullOrWhiteSpace(updateDTO.Comment))
                existingReview.Comment = updateDTO.Comment;

            existingReview.ReviewDate = DateTime.Now;

            reviewRepository.Update(existingReview.Id, existingReview);
            reviewRepository.Save();

            return Ok(existingReview);
        }


        // GET: api/Review
        [HttpGet]
        public IActionResult GetAllReview()
        {
            var reviews = reviewRepository.GetAll()
                .Include(r => r.Book)
                .Include(r => r.User)
                .Select(r => new ReviewDTO
                {
                    Id = r.Id,
                  
                     BookId = r.BookId,
                    Rating = r.Rating,
                    Comment = r.Comment,
                    ReviewDate = r.ReviewDate
                })
                .ToList();

            return Ok(reviews);
        }

        // GET: api/Review/{id}
        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var review = reviewRepository.GetAll()
                .Include(r => r.Book)
                .Include(r => r.User)
                .FirstOrDefault(r => r.Id == id);

            if (review == null)
                return NotFound();

            var reviewDTO = new ReviewDTO
            {
                Id = review.Id,
               
                BookId = review.BookId,
                Rating = review.Rating,
                Comment = review.Comment,
                ReviewDate = review.ReviewDate
            };

            return Ok(reviewDTO);
        }


        // DELETE: api/Review/{id}
        [HttpDelete("{id:int}")]
        public IActionResult RemoveReview(int id)
        {
            var review = reviewRepository.GetById(id);
            if (review == null)
            {
                return NotFound("Review not found");
            }

           reviewRepository.RemoveById(id);
            reviewRepository.Save();


            return Ok("Review Deleted succefully");
        }

    }
}
