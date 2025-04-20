namespace BookStore.DTO
{
    public class CategoryWithBooksDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<BookDTO> Books { get; set; }
    }

    public class BookDTO
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public double Price { get; set; }


    }
}
