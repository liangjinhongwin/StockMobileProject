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
    public class BuyController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BuyController (ApplicationDbContext context)
        {
            _context = context;
        }

        // View Model
        public class BuyModel
        {
            public string Symbol { get; set; }
            public int Count { get; set; }
            public decimal CurrentPrice { get; set; }
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult SetBuy ([FromBody]BuyModel purchaseOrder)
        {
            var email = HttpContext.User.Claims.ElementAt(0).Value;
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var stock = _context.UserStocks.Where(u => u.Id == id).FirstOrDefault(u => u.Symbol == purchaseOrder.Symbol);
            ApplicationUser user = _context.Users.Where(u => u.Id == id).FirstOrDefault(u => u.Cash >= 0);

            if (user.Cash >= purchaseOrder.CurrentPrice * purchaseOrder.Count)
            {
                if ( stock == null )
                {
                    try
                    {
                        _context.UserStocks.Add(new Models.UserStock
                        {
                            Id = id,
                            Symbol = purchaseOrder.Symbol,
                            IsWatched = true,
                            PurchasedCount = purchaseOrder.Count
                        });
                        user.Cash -= ( purchaseOrder.Count * purchaseOrder.CurrentPrice );
                        _context.SaveChanges();

                        return Ok(new
                        {
                            CurrentCash = user.Cash,
                            Symbol = purchaseOrder.Symbol,
                            Purchased = purchaseOrder.Count,
                            TotalPurchased = purchaseOrder.Count,
                            status = 200,
                            detail = "Your purchase order has been processed"
                        });
                    }
                    catch ( Exception )
                    {
                        return BadRequest(new 
                        { 
                            status = 400, 
                            detail = "Failed to purchase stock." 
                        });
                    }
                }
                else
                {
                    try
                    {
                        stock.PurchasedCount += purchaseOrder.Count;
                        user.Cash -= ( purchaseOrder.Count * purchaseOrder.CurrentPrice );
                        _context.SaveChanges();

                        return Ok(new
                        {
                            CurrentCash = user.Cash,
                            Symbol = purchaseOrder.Symbol,
                            Purchased = purchaseOrder.Count,
                            TotalPurchased = stock.PurchasedCount,
                            status = 200,
                            detail = "Your purchase order has been processed"
                        });
                    }
                    catch ( Exception )
                    {
                        return BadRequest(new 
                        { 
                            status = 400, 
                            detail = "Failed to purchase stock." 
                        });
                    }
                }

            }
            else
            {
                return BadRequest(new
                {
                    status = 400,
                    detail = "You don't have enough cash to process this purchase order."
                });
            }
        }
            
    }
}