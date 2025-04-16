using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.Models
{
    public class Review
    {
        public int Id { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        [ForeignKey("Book")]
        public int BookId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime ReviewDate { get; set; }

        //------------
        public ApplicationUser User { get; set; }
        public Book Book { get; set; }
            
    }
}
