using Microsoft.AspNetCore.Identity;

namespace BookStore.Models
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime JoinDate { get; set; }
        public int Streak { get; set; }
    }
}
