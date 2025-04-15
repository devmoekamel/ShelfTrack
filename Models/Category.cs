using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }

        public string Name { get; set; }

       
        public List<Book>? Books { get; set; }
    }
}
