using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.Models
{
    public class Review:BaseModel
    {
        public int Id { get; set; }
        [ForeignKey(nameof(User))]
        public string UserId { get; set; }
        [ForeignKey(nameof(Book))]
        public int BookId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime ReviewDate { get; set; }

        //------------
        public ApplicationUser User { get; set; }
        public Book Book { get; set; }
            
    }
}
