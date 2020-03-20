using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StockMobileProject.Data;

namespace StockMobileProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchasedController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PurchasedController(ApplicationDbContext context)
        {
            _context = context;
        }

        // View Model
        public class PurchasedModel
        {
            public string Symbol { get; set; }
            public int Count { get; set; }
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult getPurchased()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var stocks = _context.UserStocks.Where(u => u.Id == userId && u.PurchasedCount > 0);

            if (stocks == null || stocks.Count() == 0)
            {
                return NotFound(new { status = 404, datail = "No purchased list for the user." });
            }

            var purchasedList = stocks.Select(s => new PurchasedModel()
            {
                Symbol = s.Symbol,
                Count = s.PurchasedCount
            });

            return Ok(new { stocks = purchasedList.ToList(), status = 200, detail = "OK." });
        }
    }
}