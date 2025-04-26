using BookStore.DTO;
using BookStore.Interfaces;
using BookStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Security.Claims;

namespace BookStore.Controllers
{
    [Authorize(Roles = "User")]

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
            var missionDtos = missions.Select(m => new MissionDTO
            {
                NumOfPages = m.NumOfPages,
                Date = m.Date
            }).ToList();

            return Ok(missionDtos);

        }

        [HttpGet("{id:int}")]
        public ActionResult GetById(int id)
        {
            var mission = _missionRepo.GetById(id);
            if (mission == null) 
                return NotFound($"Mission with ID {id} not found.");

            var missionDto = new MissionDTO
            {
                NumOfPages = mission.NumOfPages,
                Date = mission.Date
            };

            return Ok(missionDto);
        }

        [HttpPost]
        public async Task<ActionResult> Add(MissionAddDTO dto)
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
            return Ok(dto);
        }

        [HttpPut("{id:int}")]
        public ActionResult Update(int id, MissionDTO dto)
        {
            var mission = _missionRepo.GetById(id);
            if (mission == null)
                return NotFound($"Mission with ID {id} not found.");

            mission.NumOfPages = dto.NumOfPages;
            mission.Date = dto.Date;
           

            _missionRepo.Update(id, mission);
            _missionRepo.Save();
            return Ok(dto);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var mission = _missionRepo.GetById(id);
            if (mission == null) return NotFound();

            _missionRepo.RemoveByObj(mission);
            _missionRepo.Save();
            return Ok("Mission deleted successfully");
        }
        //--------------------------------------------------------------------

       
       
    }
}
