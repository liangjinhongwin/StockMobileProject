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
using System.Net.Http;
using System.Runtime.Serialization;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Runtime.Serialization.Json;

namespace StockMobileProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UpdatePerformanceController : ControllerBase
    {
        private static readonly HttpClient client = new HttpClient();
        private readonly ApplicationDbContext _context;
        public UpdatePerformanceController(ApplicationDbContext context)
        {
            _context = context;
        }
        [DataContract]
        public class Quote
        {
            [DataMember]
            public double c { get; set; }
            [DataMember]
            public double h { get; set; }
            [DataMember]
            public double l { get; set; }
            [DataMember]
            public double o { get; set; }
            [DataMember]
            public double pc { get; set; }
            [DataMember]
            public double t { get; set; }
        }
        [HttpPost]

        public async Task<IActionResult> OnPostAsync()
        {
            //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Dictionary<string, Quote> snapShot= new Dictionary<string, Quote>();
            Dictionary<ApplicationUser, double> newPerformance = new Dictionary<ApplicationUser, double>();
            var users =  _context.Users;
            foreach(var user in users)
            {

                System.Diagnostics.Debug.WriteLine("============user id: " +user.Id);
                var stocks = _context.UserStocks.Where(u => u.Id == user.Id && u.PurchasedCount > 0);
                var cash = user.Cash;
                double stockTotal = 0;
                if (stocks.Count()> 0)
                {
                    foreach (var stock in stocks)
                    {
                        System.Diagnostics.Debug.WriteLine($"=====showing symbol:");
                        System.Diagnostics.Debug.WriteLine($"=====symbol: {stock.Symbol} count: {stock.PurchasedCount}");
                        Quote q = null;
                        if (snapShot.ContainsKey(stock.Symbol))
                        {
                            q = snapShot[stock.Symbol];
                        }
                        else
                        {
                            var count = 0;
                            while (count < 30)
                            {
                                try
                                {
                                    System.Threading.Thread.Sleep(2000);
                                    var responseString = await client.GetStringAsync($"https://finnhub.io/api/v1/quote?symbol={stock.Symbol}&token=bpnt9vnrh5ra872dvu20");
                                    q = ReadToObject(responseString);
                                    snapShot.Add(stock.Symbol, q);
                                    break;
                                }
                                catch (Exception e)
                                {
                                    count++;
                                }
                            }
                            if (q == null)
                            {
                                return BadRequest(new
                                {
                                    status = 400,
                                    detail = $"Failure to fetch {stock.Symbol} no data will be updated"
                                });
                            }

                        }
                        stockTotal += q.o * stock.PurchasedCount;
                        System.Diagnostics.Debug.WriteLine($" ====================================\nuser {user.Email} purchased {stock.Symbol} for ${q.o * stock.PurchasedCount}");
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($" ====================================\nuser {user.Email} did not purchase anything");
                }
                System.Diagnostics.Debug.WriteLine($" ====================================\nuser {user.Email}  purchased ${stockTotal}");
                newPerformance.Add(user, stockTotal);
            }
            //assigning performance to database
            foreach(var user in users)
            {
                var userCurrentPerformance = user.Performance;
                
                var userNewPerformance = $"{ userCurrentPerformance},{user.Cash + (decimal)newPerformance[user]}";
                System.Diagnostics.Debug.WriteLine($"=============================\n{user.Email} old:{user.Performance} new:{userNewPerformance}");
                user.Performance = userNewPerformance;
            }
            _context.SaveChangesAsync();
            
          
            
            return Ok();
        }
        public Quote ReadToObject(string json)
        {
            var deserializedUser = new Quote();
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            var ser = new DataContractJsonSerializer(deserializedUser.GetType());
            deserializedUser = ser.ReadObject(ms) as Quote;
            ms.Close();
            return deserializedUser;
        }

    }
}