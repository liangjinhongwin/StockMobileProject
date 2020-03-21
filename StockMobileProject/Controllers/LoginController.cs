using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using StockMobileProject.Areas.Identity.Pages.Account;
using StockMobileProject.Data;
using StockMobileProject.Models;

namespace StockMobileProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _config;
        private readonly ApplicationDbContext _context;

        public LoginController(
            SignInManager<ApplicationUser> signInManager,
            IServiceProvider serviceProvider,
            IConfiguration config,
            ApplicationDbContext context
            )
        {
            _signInManager = signInManager;
            _serviceProvider = serviceProvider;
            _config = config;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> OnPostAsync([FromBody]LoginModel.InputModel input)
        {
            if (ModelState.IsValid)
            {
                
                var result = await _signInManager.PasswordSignInAsync(input.Email.ToUpper(), input.Password, input.RememberMe, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    var UserManager = _serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                    var user = await UserManager.FindByEmailAsync(input.Email);
                    if (user != null)
                    {
                        var tokenString = GenerateJSONWebToken(user);
//<<<<<<< HEAD
//                        jsonResponse.token = tokenString;
//                        jsonResponse.status = 200;
//                        return Json(jsonResponse);
//=======
                        return Ok(new { token = tokenString, status = 200, detail = "OK." });
//>>>>>>> branch-kimo
                    }
                }
                else if (result.IsLockedOut)
                {
//<<<<<<< HEAD
//                    jsonResponse.status = 400;
//                    jsonResponse.detail = "Account has been locked out due to too many attempts.";
                    
//                    return Json(jsonResponse);
//                }
//            }
//            jsonResponse.status = 400;
//            jsonResponse.detail = "Invalid login information.";
//            return Json(jsonResponse);
//=======
                    return BadRequest(new { status = 400, detail = "Account has been locked out due to too many attempts." });
                }
            }

            return BadRequest(new { status = 400, detail = "Invalid login information." });
//>>>>>>> branch-kimo
        }

        string GenerateJSONWebToken(ApplicationUser user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };
            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}