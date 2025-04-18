using BookStore.DTO;
using BookStore.Interfaces;
using BookStore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlanController : ControllerBase
    {
        private readonly IPlanRepo planRepo;

        public PlanController(IPlanRepo planRepo)
        {
            this.planRepo = planRepo;
        }

        [HttpGet]
        public ActionResult getAll()
        {
            string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var plans = planRepo.GetAll().Where(p => p.UserId == userId);
            return Ok(plans);
        }


        [HttpGet("{id:int}")]
        public ActionResult getPlanbyId(int id)
        {
            string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var plan = planRepo.GetById(id);
            return Ok(plan);
        }

        [HttpPost]

        public ActionResult AddPlan (PlanDTO PlanData)
        {
            string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Plan newPlan  = new() {
            StartDate = PlanData.StartDate,
            EndDate = PlanData.EndDate,
            UserId = userId,
            BookId = PlanData.BookId,
            };
            planRepo.Add(newPlan);
            planRepo.Save();
            return Ok(PlanData);
        }


        [HttpPut]
        public ActionResult UpdatePlan (int id,PlanDTO planData) {
        
            Plan plan = planRepo.GetById(id);
            if(plan is null)
            {
                return NotFound("There's No Plan With This ID");
            }
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

            Plan newPlan = new()
            {
                StartDate = planData.StartDate,
                EndDate = planData.EndDate,
                UserId = userId,
                BookId = planData.BookId,
            };
            planRepo.Update(id, plan);
            planRepo.Save();
            return Ok(planData);
        }

        [HttpDelete]
        public ActionResult DeletePlan (int id)
        {
            Plan plan = planRepo.GetById(id);
            if (plan is null)
            {
                return NotFound("There's No Plan With This ID");
            }

            planRepo.RemoveById(id);
            planRepo.Save();

            return Ok("Plan Deleted succefully");
        }

    }
}
