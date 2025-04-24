namespace BookStore.DTO
{
    public class DisplayReviewDTO
    {
        public  int Id { get; set; }

        public int   Rate { get; set; }

        public  string Comment { get; set; }
        public DateTime ReviewDate { get;set; }
    }
}
