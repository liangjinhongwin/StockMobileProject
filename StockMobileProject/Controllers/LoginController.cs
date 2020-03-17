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
using Newtonsoft.Json.Linq;
using StockMobileProject.Areas.Identity.Pages.Account;
using StockMobileProject.Data;

namespace StockMobileProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _config;
        private readonly ApplicationDbContext _context;

        public LoginController(
            SignInManager<IdentityUser> signInManager,
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
        public async Task<JsonResult> OnPostAsync([FromBody]LoginModel.InputModel inputModel)
        {
            dynamic jsonResponse = new JObject();
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(inputModel.Email.ToUpper(), inputModel.Password, inputModel.RememberMe, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    var UserManager = _serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
                    var user = await UserManager.FindByEmailAsync(inputModel.Email);
                    if (user != null)
                    {
                        var tokenString = GenerateJSONWebToken(user);
                        jsonResponse.token = tokenString;
                        jsonResponse.status = "OK";
                    }
                }
                else if (result.IsLockedOut)
                {
                    jsonResponse.token = "";
                    jsonResponse.status = "Account has been locked out due to too many attempts.";
                }
            }
            jsonResponse.token = "";
            jsonResponse.status = "Invalid login information.";
            return Json(jsonResponse);
        }

        string GenerateJSONWebToken(IdentityUser user)
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