using BookStore.DTO;
using BookStore.Services;
using BookStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "User")]
    public class PlanController : ControllerBase
    {
        private readonly IPlanService _planService;

        public PlanController(IPlanService planService)
        {
            _planService = planService;
        }

        [HttpGet]
        public async Task<ActionResult> getAll()
        {
            string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var plans = await _planService.GetAllAsync();
            
            var planDTOs = plans
                .Where(p => p.UserId == userId)
                .Select(p => new PlanDTO()
                {
                    BookId = p.Id,
                    StartDate = p.StartDate,
                    EndDate = p.EndDate
                });
            return Ok(planDTOs);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> getPlanbyId(int id)
        {
            string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var plan = await _planService.GetByIdAsync(id);
            return Ok(plan);
        }

        [HttpGet("book/{id:int}")]
        public async Task<ActionResult> getPlanByBookId(int id)
        {
            var plan = await _planService.GetPlanByBookIdAsync(id);

            if (plan == null)
            {
                return NotFound("There's No Plan With This ID");
            }

            PlanDTO PlanData = new()
            {
                BookId = plan.BookId,
                EndDate = plan.EndDate,
                StartDate = plan.StartDate
            };
            
            return Ok(PlanData);
        }

        [HttpPost]
        public async Task<ActionResult> AddPlan(PlanDTO PlanData)
        {
            string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Plan newPlan = new()
            {
                StartDate = PlanData.StartDate,
                EndDate = PlanData.EndDate,
                UserId = userId,
                BookId = PlanData.BookId,
            };
            await _planService.AddAsync(newPlan);
            await _planService.SaveAsync();
            return Ok(PlanData);
        }

        [HttpPut]
        public async Task<ActionResult> UpdatePlan(int id, PlanDTO planData)
        {
            var plan = await _planService.GetByIdAsync(id);
            if (plan == null)
            {
                return NotFound("There's No Plan With This ID");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

            plan.StartDate = planData.StartDate;
            plan.EndDate = planData.EndDate;
            plan.UserId = userId;
            plan.BookId = planData.BookId;

            _planService.Update(plan);
            await _planService.SaveAsync();
            return Ok(planData);
        }

        [HttpDelete]
        public async Task<ActionResult> DeletePlan(int id)
        {
            var plan = await _planService.GetByIdAsync(id);
            if (plan == null)
            {
                return NotFound("There's No Plan With This ID");
            }

            _planService.DeleteById(id);
            await _planService.SaveAsync();

            return Ok("Plan Deleted succefully");
        }
    }
}
