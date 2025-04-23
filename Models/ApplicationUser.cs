using Microsoft.AspNetCore.Identity;

namespace BookStore.Models
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime JoinDate { get; set; }
        public int Streak { get; set; }  

        public ICollection<Purchase> Purchases { get; set; }
        public ICollection<Plan> Plans { get; set; }
        public ICollection<Review> Reviews { get; set; }

    }
}
