using BookStore.Models;

namespace BookStore.Interfaces
{
    public interface IMissionRepo : IGenericRepo<Mission>
    {
        List<Mission> GetMissionsByPlanId(int planId);
    }
}
