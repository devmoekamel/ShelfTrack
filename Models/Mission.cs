using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.Models
{
    public class Mission : BaseModel
    {
        public int NumOfPages { get; set; }

        public DateTime Date { get; set; }

        public int PlanId { get; set; }

        [ForeignKey(nameof(PlanId))]    
        public Plan plan { get; set; }
    }

}
