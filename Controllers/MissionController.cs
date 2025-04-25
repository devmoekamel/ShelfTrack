using BookStore.DTO;
using BookStore.Interfaces;
using BookStore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MissionController : ControllerBase
    {
        private readonly IMissionRepo _missionRepo;
        private readonly UserManager<ApplicationUser> userManager;

        public MissionController(IMissionRepo missionRepo,UserManager<ApplicationUser> userManager)
        {
            _missionRepo = missionRepo;
            this.userManager = userManager;
        }
        [HttpGet("Plan/{planId}")]
        public ActionResult GetMissionsByPlan(int planId)
        {
            var missions = _missionRepo.GetMissionsByPlanId(planId);
            return Ok(missions);
        }

        [HttpGet("{id:int}")]
        public ActionResult GetById(int id)
        {
            var mission = _missionRepo.GetById(id);
            if (mission == null) return NotFound();
            return Ok(mission);
        }

        [HttpPost]
        public async Task<ActionResult> Add(MissionDTO dto)
        {
            if (!ModelState.IsValid) 
                return BadRequest(ModelState);
            string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

            ApplicationUser applicationUser = await userManager.FindByIdAsync(userId);
            var today = DateTime.Today;
            var lastDate = applicationUser.LastMissionDate.Date;
            if(lastDate == today)
            {
                return Ok("Your already added mission today");
            }
            else if(lastDate == today.AddDays(-1))
            {
                applicationUser.Streak++;
                await userManager.UpdateAsync(applicationUser);
            }else if (lastDate<today.AddDays(-1))
            {
                applicationUser.Streak=1;
                await userManager.UpdateAsync(applicationUser);
            }

            applicationUser.LastMissionDate = today;
            var mission = new Mission
            {
                NumOfPages = dto.NumOfPages,
                Date = dto.Date,
                PlanId = dto.PlanId
            };

            _missionRepo.Add(mission);
            _missionRepo.Save();
            return Ok(mission);
        }

        [HttpPut("{id:int}")]
        public ActionResult Update(int id, MissionDTO dto)
        {
            var mission = _missionRepo.GetById(id);
            if (mission == null) 
                return NotFound();

            mission.NumOfPages = dto.NumOfPages;
            mission.Date = dto.Date;
            mission.PlanId = dto.PlanId;

            _missionRepo.Update(id, mission);
            _missionRepo.Save();
            return Ok(mission);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var mission = _missionRepo.GetById(id);
            if (mission == null) return NotFound();

            _missionRepo.RemoveById(id);
            _missionRepo.Save();
            return Ok("Mission deleted successfully");
        }
        //--------------------------------------------------------------------

       
       
    }
}
