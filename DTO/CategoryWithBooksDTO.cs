namespace BookStore.DTO
{
    public class CategoryWithBooksDTO
    {
        public string Name { get; set; }
        public List<BOOkDTO> Books { get; set; }
    }

    public class BOOkDTO
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public int PageCount { get; set; }

        public double Price { get; set; }
    }
}
