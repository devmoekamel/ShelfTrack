using BookStore.Interfaces;
using BookStore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        //add review
        [HttpPost]
        public IActionResult AddReview([FromBody]Review review)
        {
            if (review == null)
                return BadRequest();
          
            review.ReviewDate = DateTime.Now;
            reviewRepository.Add(review);
            reviewRepository.Save();
            return Ok(review);
        }

        //Get all review
        [HttpGet]
        public IActionResult GetAllReview()
        {
            var reviews = reviewRepository.GetAll();
            return Ok(reviews);
        }

        //GetReview by Id
        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var review = reviewRepository.GetById(id);
            if (review == null)
                return NotFound();
            return Ok(review);
        }

        //
    }
}
