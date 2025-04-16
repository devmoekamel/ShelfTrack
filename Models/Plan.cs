using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.Models
{
    public class Plan : BaseModel
    {


        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; }
        [ForeignKey(nameof(Book))]
        public int BookId { get; set; }
        public  Book Book { get; set; }


        public List<Mission> Missions { get; set; }
         
    }
}
