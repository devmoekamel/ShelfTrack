using BookStore.context;
using BookStore.Interfaces;
using BookStore.Models;

namespace BookStore.Reporisatory
{
    public class MissionRepo : GenericRepo<Mission>, IMissionRepo
    {
        private readonly BookStoreContext _context;

        public MissionRepo(BookStoreContext context) : base(context)
        {
            _context = context;
        }

        public List<Mission> GetMissionsByPlanId(int planId)
        {
            return _context.Missions.Where(m => m.PlanId == planId).ToList();
        }
    }
}
