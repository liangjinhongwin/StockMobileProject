using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using StockMobileProject.Data;
using System.Linq;

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
            var email = HttpContext.User.Claims.ElementAt(0).Value;
            var user = _context.Users.Where(u => u.Email == email).FirstOrDefault();
            dynamic jsonResponse = new JObject();

            if (user.UserStocks == null)
            {
                jsonResponse.status = 404;
                jsonResponse.detail = "No stock list for the user.";
                return NotFound(jsonResponse);
            }

            if (user.UserStocks.Where(s => s.IsWatched == true) == null)
            {
                jsonResponse.status = 404;
                jsonResponse.detail = "No watch list for the user.";
                return NotFound(jsonResponse);
            }

            var watchedList = user.UserStocks
                .Where(s => s.IsWatched == true)
                .Select(s => new WatchedModel()
                {
                    Symbol = s.Symbol
                });

            jsonResponse.status = 200;
            jsonResponse.detail = "OK.";
            return Ok(jsonResponse);
        }
    }
}