using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.Models
{
    public class Purchase:BaseModel
    {
        public int Id { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        [ForeignKey("Book")]
        public int BookId { get; set; }
        public DateTime PurchaseDate { get; set; }

        //-----------

        public ApplicationUser User { get; set; }
        public Book Book { get; set; }


    }
}
