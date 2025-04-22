using BookStore.Interfaces;
using BookStore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseController : ControllerBase
    {
        private readonly IPurchaseRepository purchaseRepository;
        public PurchaseController(IPurchaseRepository purchaseRepository)
        {
            this.purchaseRepository = purchaseRepository;
        }

        //post
        [HttpPost]
        public IActionResult AddPurchases([FromBody] Purchase purchase)
        {
            purchase.PurchaseDate = DateTime.Now;
            purchaseRepository.Add(purchase);
            purchaseRepository.Save();
            return Ok(purchase);
        }

        //Get

        [HttpGet]
        public IActionResult GetAllPurchases()
        {
            var purchases = purchaseRepository.GetAll();
            return Ok(purchases);
        }

        //GetById
        [HttpGet("{id:int}")]
        public IActionResult GetByIdPurchase(int id)
        {
            var purchase = purchaseRepository.GetById(id);
            if (purchase == null)
                return NotFound();
            return Ok(purchase);
        }

        //Delete
        [HttpDelete("{id:int}")]
        public IActionResult RemovePurchase(int id)
        {
            var purchase = purchaseRepository.GetById(id);
            if (purchase == null)
                return NotFound();
            purchaseRepository.RemoveById(id);
            purchaseRepository.Save();
            return NoContent();
        }
        //GetByUserId
        [HttpGet("user/{userId:int}")]
        public IActionResult GetByUserId(int userId)
        {
            var purchase = purchaseRepository.GetAll().Where(u => u.UserId == userId);
            return Ok(purchase);
        }

    }
}