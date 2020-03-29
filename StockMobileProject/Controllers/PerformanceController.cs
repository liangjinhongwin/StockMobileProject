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
    public class PerformanceController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public PerformanceController(ApplicationDbContext context)
        {
            _context = context;
        }
        public class PerformanceViewModel
        {
            public DateTime StartDate { get; set; }
            public decimal Cash { get; set; }
            public string Performance { get; set; }
        }
        
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> OnPostAsync([FromBody]PerformanceViewModel vm)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            ApplicationUser user = _context.Users.Where(u => u.Id == userId).FirstOrDefault();
           
            if (user == null)
            {
                return BadRequest(new { status = 400, datail = "user not found" });
            }
            user.Performance = vm.Performance;
            await _context.SaveChangesAsync();

            return Ok();
        }
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetPerformance()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            ApplicationUser user = _context.Users.Where(u => u.Id == userId).FirstOrDefault();
            if (user == null)
            {
                return BadRequest(new { status = 400, datail = "user not found" });
            }
            var result = new PerformanceViewModel()
            {
                StartDate = user.StartDate,
                Cash = user.Cash,
                Performance = user.Performance
            };

            return Ok(result);
        }
    }
}