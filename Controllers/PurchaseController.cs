using BookStore.DTO;
using BookStore.Services;
using BookStore.Models;
using BookStore.context;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Controllers
{
    [Authorize(Roles = "User")]
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseController : ControllerBase
    {
        private readonly IPurchaseService _purchaseService;
        private readonly BookStoreContext _context;

        public PurchaseController(IPurchaseService purchaseService, BookStoreContext context)
        {
            _purchaseService = purchaseService;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> AddPurchases([FromBody] PurchaseDTO purchaseDTO)
        {
            string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var purchase = new Purchase
            {
                UserId = userId,
                BookId = purchaseDTO.BookId,
                PurchaseDate = purchaseDTO.PurchaseDate
            };

            await _purchaseService.AddAsync(purchase);
            await _purchaseService.SaveAsync();

            return Ok(purchaseDTO);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllPurchases()
        {
            var purchases = await _context.Purchases
                .Where(p => !p.IsDeleted)
                .Include(p => p.Book)
                .Include(p => p.User)
                .Select(p => new PurchaseDisplayDTO
                {
                    Id = p.Id,
                    BookTitle = p.Book.Title,
                    PurchaseDate = p.PurchaseDate
                })
                .ToListAsync();

            return Ok(purchases);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByIdPurchase(int id)
        {
            var purchase = await _context.Purchases
                .Where(r => !r.IsDeleted)
                .Include(r => r.User)
                .Include(r => r.Book)
                .Where(r => r.Id == id)
                .Select(r => new PurchaseDisplayDTO
                {
                    Id = r.Id,
                    BookTitle = r.Book.Title,
                    PurchaseDate = r.PurchaseDate
                }).FirstOrDefaultAsync();
            if (purchase == null)
            {
                return NotFound();
            }

            return Ok(purchase);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> RemovePurchase(int id)
        {
            var purchase = await _purchaseService.GetByIdAsync(id);
            if (purchase == null)
            {
                return NotFound("Purchase not found");
            }

            _purchaseService.DeleteById(id);
            await _purchaseService.SaveAsync();

            return Ok("Purchase Deleted succefully");
        }

        [HttpGet("user")]
        public async Task<IActionResult> GetByUserId()
        {
            string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("User not authenticated");
            }

            var purchases = await _purchaseService.GetAllAsync();
            var userPurchases = purchases.Where(u => u.UserId == userId);
            return Ok(userPurchases);
        }
    }
}
