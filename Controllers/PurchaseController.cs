using BookStore.DTO;
using BookStore.Interfaces;
using BookStore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Controllers
{
    [Authorize (Roles ="User")]
    
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseController : ControllerBase
    {
        private readonly IPurchaseRepository purchaseRepository;
        private readonly string userId;
        public PurchaseController(IPurchaseRepository purchaseRepository)
        {
            this.purchaseRepository = purchaseRepository;
           
        }

        // POST: api/Purchase
        [HttpPost]
        public IActionResult AddPurchases([FromBody] PurchaseDTO purchaseDTO)
        {
          string  userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); 
            }

            var purchase = new Purchase
            {
                //UserId = purchaseDTO.UserId,
                UserId = userId,
                BookId = purchaseDTO.BookId,
                PurchaseDate = purchaseDTO.PurchaseDate
            };

            purchaseRepository.Add(purchase);
            purchaseRepository.Save();

            return Ok(purchaseDTO); 
        }

        // // GET: api/Purchase
        // [HttpGet]
        //// [Authorize(Roles ="Admin")]
        // public IActionResult GetAllPurchases()
        // {
        //     string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        //     if (string.IsNullOrEmpty(userId))
        //     {
        //         return BadRequest("User not authenticated");
        //     }

        //     var purchases = purchaseRepository.GetAll().

        //         //.Where(u => u.UserId == userId);
        //     return Ok(purchases); 
        // }

        [HttpGet]
        [Authorize(Roles ="Admin")] 
        public IActionResult GetAllPurchases()
        {
           // string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            //if (string.IsNullOrEmpty(userId))
            //    return BadRequest("User not authenticated");

            var purchases = purchaseRepository.GetAll()
                .Include(p => p.Book)
                .Include(p => p.User)
                //.Where(p => p.UserId == userId)
                .Select(p => new PurchaseDisplayDTO
                {
                    Id = p.Id,
                    BookTitle = p.Book.Title,
                    //UserName = p.User.UserName,
                    PurchaseDate = p.PurchaseDate
                })
                .ToList();

            return Ok(purchases);
        }


        // GET: api/Purchase/{id}
        [HttpGet("{id:int}")]
        public IActionResult GetByIdPurchase(int id)
        {
            var purchase = purchaseRepository.GetAll()
                .Include(r => r.User)
                .Include(r => r.Book)
                .Where(r => r.Id == id)
                .Select(r => new PurchaseDisplayDTO
                {
                    Id = r.Id,
                    BookTitle = r.Book.Title,
                    PurchaseDate = r.PurchaseDate
                }).FirstOrDefault();
            if (purchase == null)
            {
                return NotFound();
            }

            return Ok(purchase);
        }



        // DELETE: api/Purchase/{id}
        [HttpDelete("{id:int}")]
        public IActionResult RemovePurchase(int id)
        {
            var purchase = purchaseRepository.GetById(id);
            if (purchase == null)
            {
                return NotFound("Purchase not found");
            }

            purchaseRepository.RemoveById(id);
            purchaseRepository.Save();


            return Ok("Purchase Deleted succefully");
        }

        // GET: api/Purchase/user
        [HttpGet("user")]

        public IActionResult GetByUserId()
        {
             string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;


            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("User not authenticated");
            }

            var purchases = purchaseRepository.GetAll().Where(u => u.UserId == userId);
            return Ok(purchases); // 
        }
    }
}