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
using StockMobileProject.Models;

namespace StockMobileProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SellController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SellController (ApplicationDbContext context)
        {
            _context = context;
        }

        // View Model
        public class SellModel
        {
            public string Symbol { get; set; }
            public int Count { get; set; }
            public decimal CurrentPrice { get; set; }
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult SetBuy ([FromBody]SellModel purchaseOrder)
        {
            var email = HttpContext.User.Claims.ElementAt(0).Value;
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var stock = _context.UserStocks.Where(u => u.Id == id).FirstOrDefault(u => u.Symbol == purchaseOrder.Symbol);
            ApplicationUser user = _context.Users.Where(u => u.Id == id).FirstOrDefault(u => u.Cash >= 0);

            return null;
        }
    }
}