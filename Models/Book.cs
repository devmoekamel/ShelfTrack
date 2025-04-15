using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.Models
{
    public class Book
    {
        public int BookID { get; set; }

        public string Title { get; set; }

       
        public string Author { get; set; }

        public string Description { get; set; }
        public string Genre { get; set; }

        public string ISBN { get; set; }

        public int PageCount { get; set; }

        public decimal Price { get; set; }

        public string CoverImageURL { get; set; }

        public DateTime PublishedDate { get; set; }

        public bool IsFree { get; set; }

        [ForeignKey("Category")]
        public int Categoryid { get; set; }
        public Category? Category { get; set; }
    }
}
