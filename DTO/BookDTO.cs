namespace BookStore.DTO
{
    public class BookDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public string Author { get; set; }

        public string Description { get; set; }

        public string Genre { get; set; }

        public string ISBN { get; set; }

        public int PageCount { get; set; }

        public double Price { get; set; }

        public string CoverImageURL { get; set; }

        public DateTime PublishedDate { get; set; }
        public int Categoryid { get; set; }

    }
}
