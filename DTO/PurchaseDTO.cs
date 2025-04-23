namespace BookStore.DTO
{
    public class PurchaseDTO
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int BookId { get; set; }

        public DateTime PurchaseDate { get; set; }
    }
}