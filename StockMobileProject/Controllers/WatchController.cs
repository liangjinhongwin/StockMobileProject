using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using StockMobileProject.Data;

namespace StockMobileProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WatchController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WatchController(ApplicationDbContext context)
        {
            _context = context;
        }

        // View Model
        public class WatchModel
        {
            public string Symbol { get; set; }
            public bool IsWatch { get; set; }
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult SetWatch([FromBody]WatchModel updatedStock)
        {
            var email = HttpContext.User.Claims.ElementAt(0).Value;
            var user = _context.Users.Where(u => u.Email == email).FirstOrDefault();
            dynamic jsonResponse = new JObject();

            if (user.UserStocks == null && updatedStock.IsWatch == true)
            {
                try
                {
                    _context.UserStocks.Add(new Models.UserStock
                    {
                        Email = email,
                        Symbol = updatedStock.Symbol,
                        IsWatched = updatedStock.IsWatch,
                        PurchasedCount = 0
                    });
                    _context.SaveChanges();
                }
                catch (Exception e)
                {
                    return BadRequest("1");
                }

            }
            else
            {
                var stock = user.UserStocks.Where(s => s.Symbol == updatedStock.Symbol).FirstOrDefault();
                if (stock == null && updatedStock.IsWatch == true)
                {
                    try
                    {
                        _context.UserStocks.Add(new Models.UserStock
                        {
                            Email = user.Email,
                            Symbol = updatedStock.Symbol,
                            IsWatched = true
                        });

                        _context.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        return BadRequest("2");
                    }
                }
                else
                {
                    try
                    {
                        if (stock.PurchasedCount == 0 && updatedStock.IsWatch == false)
                        {
                            _context.UserStocks.Remove(stock);
                            _context.SaveChanges();
                        }
                        else
                        {
                            stock.IsWatched = updatedStock.IsWatch;
                            _context.SaveChanges();
                        }
                    }
                    catch (Exception e)
                    {
                        return BadRequest("3");
                    }
                }
            }

            jsonResponse.status = 200;
            jsonResponse.detail = "Ok.";
            return Ok(jsonResponse);
        }
    }
}