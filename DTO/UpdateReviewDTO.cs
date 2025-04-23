using System.ComponentModel.DataAnnotations;

namespace BookStore.DTO
{
    public class UpdateReviewDTO
    {
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int? Rating { get; set; }
        public string? Comment { get; set; }
    }
}
