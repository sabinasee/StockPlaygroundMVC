using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Configuration;
using System.IO;
using System.Reflection;
using StockBL;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace StockPlaygroundMVC.Controllers
{
    public class UsersController : Controller
    {
        private readonly StockUsersDBContext _context;
        private readonly string _apiKey = "TNKC6NOVASXVEQVM";
        public UsersController(StockUsersDBContext context)
        {
            //string currentPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            //// Your DB filename    
            //string dbFileName = "StockUsersDB.db";
            //// Creates a full path that contains your DB file            
            //string dbPath = @"D:\Sabina - Documente\projects\StockPlaygroundMVC\StockDAL";
            //string absolutePath = Path.Combine(dbPath, dbFileName);
            //SqliteConnection conn = new SqliteConnection(@"Data Source = " + absolutePath);
            //conn.Open();
            _context = context;
        }

        public async Task<IActionResult> Index([FromServices] IRepositoryUser repoUser)
        {
            //var model = await _context.Users.ToArrayAsync();
            var model = repoUser.UsersWithAccountRole();
            return View(model);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserName,UserPassword,UserRole")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Watchlist([FromServices] IRepositoryUser repoUser)
        {
            List<WatchlistItems> searchedSymbols = new List<WatchlistItems>();
            var watchlist = repoUser.RetrieveUserWatchlist();
            var symbols = watchlist.Split(';');
            foreach (string sym in symbols)
            {
                var reqUri = "https://" + $@"www.alphavantage.co/query?function=SYMBOL_SEARCH&keywords=" + sym + "&apikey=" + _apiKey;
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync(reqUri))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();

                        dynamic data = JsonConvert.DeserializeObject(apiResponse);
                        var count = data.bestMatches.Count;
                        var firstMatch = data.bestMatches[0];
                        WatchlistItems wl = (JsonConvert.DeserializeObject<WatchlistItems>(firstMatch.ToString()));
                        searchedSymbols.Add(wl);
                    }
                }
            }
            return View(searchedSymbols);
            //return View(model);
        }
        public async Task<IActionResult> SeeCompanyOverwiew(string sym)
        {
            sym = "TME";
            var reqUri = "https://" + $@"www.alphavantage.co/query?function=OVERVIEW&symbol=" + sym + "&apikey=" + _apiKey;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(reqUri))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    return View("SeeCompanyOverview", apiResponse);
                }
            }
        }
    }
}
