using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.Models
{
    public class Plan
    {

        public int Id { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; }
        public int BookId { get; set; }
        public  Book Book { get; set; }


        public List<Mission> Missions { get; set; }
         
    }
}
