namespace BookStore.DTO
{
    public class AddReviewDto
    {
        public int UserId { get; set; }
        public int BookId { get; set; }
        public string Comment { get; set; }
        public  int Rating { get; set; }
      
    }
}
