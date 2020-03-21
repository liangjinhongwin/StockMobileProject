using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StockMobileProject.Data;
using System;
using System.Linq;
using System.Security.Claims;

namespace StockMobileProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WatchedController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WatchedController(ApplicationDbContext context)
        {
            _context = context;
        }

        // View Model
        public class WatchedModel
        {
            public string Symbol { get; set; }
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult getWatched()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var stocks = _context.UserStocks.Where(u => u.Id == userId && u.IsWatched == true);

            if (stocks == null || stocks.Count() == 0)
            {
                return NotFound(new { status = 404, datail = "No watch list for the user." });
            }

            var watchList = stocks.Select(s => new WatchedModel()
            {
                Symbol = s.Symbol
            });

            return Ok(new { stocks = watchList.ToList(), status = 200, detail = "OK." });
        }
    }
}