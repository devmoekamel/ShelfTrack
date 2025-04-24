namespace BookStore.DTO
{
    public class PurchaseDTO
    {
        public int BookId { get; set; }
        public DateTime PurchaseDate { get; set; } = DateTime.Now;

    }
}