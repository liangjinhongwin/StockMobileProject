﻿using System;
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
    public class WatchController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WatchController (ApplicationDbContext context)
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
        public IActionResult SetWatch ([FromBody]WatchModel updatedStock)
        {
            var email = HttpContext.User.Claims.ElementAt(0).Value;
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var stock = _context.UserStocks.Where(u => u.Id == id).FirstOrDefault(u => u.Symbol == updatedStock.Symbol);

            if ( stock == null && updatedStock.IsWatch == true )
            {
                try
                {
                    _context.UserStocks.Add(new Models.UserStock
                    {
                        Id = id,
                        Symbol = updatedStock.Symbol,
                        IsWatched = updatedStock.IsWatch,
                        PurchasedCount = 0
                    });
                    _context.SaveChanges();
                }
                catch ( Exception e )
                {
                    return BadRequest(new { status = 400, detail = "Failed to create watch list." });
                }
            }
            else
            {
                try
                {
                    if ( stock.PurchasedCount == 0 && updatedStock.IsWatch == false )
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
                catch ( Exception e )
                {
                    return BadRequest(new { status = 400, detail = "Failed to update watch list." });
                }

            }

            return Ok();
        }
    }
}