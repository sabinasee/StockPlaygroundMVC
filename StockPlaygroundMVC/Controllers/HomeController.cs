using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StockBL;
using StockPlaygroundMVC.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace StockPlaygroundMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly string _apiKey = "TNKC6NOVASXVEQVM";

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> DummySearch(string symbol)
        {
            symbol = "BA";
            List<Symbols> searchedSymbols = new List<Symbols>();
            var reqUri = "https://" + $@"www.alphavantage.co/query?function=SYMBOL_SEARCH&keywords="+symbol+"&apikey="+_apiKey;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(reqUri))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    
                    dynamic data = JsonConvert.DeserializeObject(apiResponse);
                    var count = data.bestMatches.Count;
                    var firstSymbol = data.bestMatches[2];
                    searchedSymbols = data.bestMatches.ToObject<List<Symbols>>();
                }
            }
            return View(searchedSymbols);
        }

        public ViewResult Search() => View();
        [HttpPost]
        public async Task<IActionResult> Search(string sym)
        {
            List<Symbols> searchedSymbols = new List<Symbols>();
            var reqUri = "https://" + $@"www.alphavantage.co/query?function=SYMBOL_SEARCH&keywords=" + sym + "&apikey=" + _apiKey;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(reqUri))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();

                    dynamic data = JsonConvert.DeserializeObject(apiResponse);
                    var count = data.bestMatches.Count;
                    var firstSymbol = data.bestMatches[2];
                    searchedSymbols = data.bestMatches.ToObject<List<Symbols>>();
                }
            }
            return View(searchedSymbols);
        }

        public IActionResult Authenticate()
        {
            var stockClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, "Sabina"),
                new Claim(ClaimTypes.Role, "Account"),
                new Claim("AuthUser","Only auth users can access the API")
            };

            var stockIdentity = new ClaimsIdentity(stockClaims, "Stocks identity");

            var userPrincipal = new ClaimsPrincipal(new[] { stockIdentity });

            HttpContext.SignInAsync(userPrincipal);

            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
