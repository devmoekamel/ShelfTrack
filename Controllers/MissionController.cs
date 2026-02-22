using BookStore.DTO;
using BookStore.Services;
using BookStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookStore.Controllers
{
    [Authorize(Roles = "User")]
    [Route("api/[controller]")]
    [ApiController]
    public class MissionController : ControllerBase
    {
        private readonly IMissionService _missionService;
        private readonly UserManager<ApplicationUser> userManager;

        public MissionController(IMissionService missionService, UserManager<ApplicationUser> userManager)
        {
            _missionService = missionService;
            this.userManager = userManager;
        }

        [HttpGet("Plan/{planId}")]
        public async Task<ActionResult> GetMissionsByPlan(int planId)
        {
            var missions = await _missionService.GetMissionsByPlanIdAsync(planId);
            var missionDtos = missions.Select(m => new MissionDTO
            {
                NumOfPages = m.NumOfPages,
                Date = m.Date
            }).ToList();

            return Ok(missionDtos);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetById(int id)
        {
            var mission = await _missionService.GetByIdAsync(id);
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
            }else if (lastDate < today.AddDays(-1))
            {
                applicationUser.Streak = 1;
                await userManager.UpdateAsync(applicationUser);
            }

            applicationUser.LastMissionDate = today;
            var mission = new Mission
            {
                NumOfPages = dto.NumOfPages,
                Date = dto.Date,
                PlanId = dto.PlanId
            };

            await _missionService.AddAsync(mission);
            await _missionService.SaveAsync();
            return Ok(dto);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update(int id, MissionDTO dto)
        {
            var mission = await _missionService.GetByIdAsync(id);
            if (mission == null)
                return NotFound($"Mission with ID {id} not found.");

            mission.NumOfPages = dto.NumOfPages;
            mission.Date = dto.Date;

            _missionService.Update(mission);
            await _missionService.SaveAsync();
            return Ok(dto);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var mission = await _missionService.GetByIdAsync(id);
            if (mission == null) return NotFound();

            _missionService.Delete(mission);
            await _missionService.SaveAsync();
            return Ok("Mission deleted successfully");
        }
    }
}
