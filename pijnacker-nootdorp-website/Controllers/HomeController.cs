using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using pijnacker_nootdorp_website.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace pijnacker_nootdorp_website.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly DatabaseContext _context;

        public HomeController(ILogger<HomeController> logger, DatabaseContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            List<House> houses = GetHouses();

            User user = null;
            if (HttpContext.Session.TryGetValue("user", out byte[] userId_raw))
            {
                string userId = Encoding.ASCII.GetString(userId_raw);

                user = _context.Users.FirstOrDefault(u => u.Id == int.Parse(userId));
            }

            return View(new HomeModel
            {
                User = user,
                Houses = houses
            });
        }

        [HttpPost]
        public IActionResult Index(HomeModel data)
        {
            data.IsInitialized = true;
            List<House> houses = GetHouses(data);

            User user = null;
            if (HttpContext.Session.TryGetValue("user", out byte[] userId_raw))
            {
                string userId = Encoding.ASCII.GetString(userId_raw);

                user = _context.Users.FirstOrDefault(u => u.Id == int.Parse(userId));
            }

            data.User = user;
            data.Houses = houses;

            return View(data);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Route("login")]
        public IActionResult Login()
        {
            if (HttpContext.Session.TryGetValue("user", out byte[] userId_raw))
            {
                return Redirect("/");
            }

            return View(new LoginData
            {
                Email = "",
                Password = ""
            });
        }

        [Route("login")]
        [HttpPost]
        public IActionResult Login(LoginData data)
        {
            if (HttpContext.Session.TryGetValue("user", out byte[] userId_raw))
            {
                return Redirect("/");
            }

            User userData = _context.Users.FirstOrDefault(u => u.Email == data.Email);

            if (ModelState.IsValid && userData != null)
            {
                string hashInput = ComputeSha256Hash(data.Password);

                if (hashInput == userData.Password)
                {
                    HttpContext.Session.Set("user", Encoding.ASCII.GetBytes(userData.Id.ToString()));

                    Website.User = userData;

                    return Redirect("/");
                }
            }

            if (userData == null)
            {
                data.EmailIsWrong = true;
            }

            data.Password = "";

            return View(data);
        }

        [Route("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("user");

            Website.User = null;

            return Redirect("/");
        }

        [Route("contact")]
        public IActionResult Contact()
        {
            return View();
        }

        [Route("contact")]
        [HttpPost]
        public IActionResult Contact(User user)
        {
            if (ModelState.IsValid)
            {
                user.Password = ComputeSha256Hash(user.Password);

                _context.Users.Add(user);
                _context.SaveChanges();
                HttpContext.Session.Set("user", Encoding.ASCII.GetBytes(_context.Users.FirstOrDefault(u => u.Email == user.Email).Id.ToString()));
                
                return Redirect("/");
            }

            return View(user);
        }

        [Route("profile")]
        public IActionResult Profile()
        {
            return View(Website.User);
        }

        [Route("cart")]
        public IActionResult Cart()
        {
            User user = null;
            if (HttpContext.Session.TryGetValue("user", out byte[] userId_raw))
            {
                string userId = Encoding.ASCII.GetString(userId_raw);

                user = _context.Users.FirstOrDefault(u => u.Id == int.Parse(userId));
            }

            Order order = _context.Orders.FirstOrDefault(x => x.User.Id == user.Id);
            if (order == null)
            {
                order = new Order
                {
                    UserId = user.Id,
                    User = user
                };

                _context.Orders.Add(order);
                _context.SaveChanges();
            }

            return View(new CartModel
            {
                User = user,
                Order = order
            });
        }
        
        [Route("cart/pay")]
        public IActionResult Pay()
        {
            return View();
        }

        [Route("cart/pay/done")]
        public IActionResult FinishPay()
        {
            User user = null;
            if (HttpContext.Session.TryGetValue("user", out byte[] userId_raw))
            {
                string userId = Encoding.ASCII.GetString(userId_raw);

                user = _context.Users.FirstOrDefault(u => u.Id == int.Parse(userId));
            }

            _context.Orders.Remove(user.Order);
            _context.SaveChanges();

            return Redirect("/");
        }

        [Route("cart/add/{houseId}")]
        public IActionResult AddCart(int houseId)
        {
            User user = null;
            if (HttpContext.Session.TryGetValue("user", out byte[] userId_raw))
            {
                string userId = Encoding.ASCII.GetString(userId_raw);

                user = _context.Users.FirstOrDefault(u => u.Id == int.Parse(userId));
            }

            user.Order.OrderItems.Add(new OrderItem
            {
                HouseId = houseId,
                OrderId = user.Order.Id
            });
            _context.SaveChanges();

            return Redirect($"/houses/{houseId}");
        }

        #region Register

        [Route("register")]
        public IActionResult Register()
        {
            return View(new User { });
        }

        [Route("register")]
        [HttpPost]
        public IActionResult Register(User user)
        {
            if (ModelState.IsValid && !_context.Users.Any(u => u.Email == user.Email))
            {
                user.Password = ComputeSha256Hash(user.Password);

                _context.Users.Add(user);
                _context.SaveChanges();
                HttpContext.Session.Set("user", Encoding.ASCII.GetBytes(_context.Users.FirstOrDefault(u => u.Email == user.Email).Id.ToString()));

                Website.User = user;

                return Redirect("/");
            }

            return View(user);
        }

        #endregion

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public List<House> GetHouses(HomeModel search = null)
        {
            if (search == null) return _context.Houses.ToList();

            int? price_minimum = int.TryParse(search.Price_Minimum, out int data1) ? data1 : null;
            int? price_maximum = int.TryParse(search.Price_Maximum, out int data2) ? data2 : null;

            int? outdoorArea_minimum = int.TryParse(search.OutdoorArea_Minimum, out int data3) ? data3 : null;
            int? outdoorArea_maximum = int.TryParse(search.OutdoorArea_Maximum, out int data4) ? data4 : null;

            int? indoorArea_minimum = int.TryParse(search.IndoorArea_Minimum, out int data5) ? data5 : null;
            int? indoorArea_maximum = int.TryParse(search.IndoorArea_Maximum, out int data6) ? data6 : null;

            int? buildYear_minimum = int.TryParse(search.BuildYear_Minimum, out int data7) ? data7 : null;
            int? buildYear_maximum = int.TryParse(search.BuildYear_Maximum, out int data8) ? data8 : null;

            bool wheelchair = search.Wheelchair == "on";
            bool car = search.Car == "on";
            bool publicTransport = search.PublicTransport == "on";

            string[] searchQueryKeywords = GetKeywords(search.SearchQuery);

            Debug.WriteLine($"MORE: {Website.IsMoreOptions}");

            List<House> houses = new List<House>();
            Dictionary<House, int> matches = new Dictionary<House, int>();
            foreach (var house in _context.Houses)
            {
                if (price_minimum == null || house.Price < price_minimum) continue;
                else if (house.Price > price_maximum) continue;

                if (Website.IsMoreOptions)
                {
                    if (outdoorArea_minimum == null || house.OutdoorArea < outdoorArea_minimum) continue;
                    else if (house.OutdoorArea > outdoorArea_maximum) continue;

                    if (indoorArea_minimum == null || house.IndoorArea < indoorArea_minimum) continue;
                    else if (house.IndoorArea > indoorArea_maximum) continue;

                    if (buildYear_minimum == null || house.BuildYear < buildYear_minimum) continue;
                    else if (house.BuildYear > buildYear_maximum) continue;

                    bool filter = true;
                    if (!wheelchair && !car && !publicTransport)
                    {
                        filter = false;
                    }
                    else
                    {
                        if (wheelchair && house.AccessData.wheelchair) filter = false;
                        else if (car && house.AccessData.car) filter = false;
                        else if (publicTransport && house.AccessData.publicTransport) filter = false;
                    }

                    if (filter) continue;
                }

                int matchCount = searchQueryKeywords == null ? 999 : CountMatches(GetKeywords(house.Address), searchQueryKeywords);

                if (matchCount > 0)
                {
                    houses.Add(house);
                    matches.Add(house, matchCount);
                }
            }

            return houses.OrderByDescending(x => matches[x]).ToList();
        }

        private string[] GetKeywords(string text)
        {
            if (string.IsNullOrEmpty(text)) return null;

            string[] separators = new string[] { ",", ".", "!", "\'", " ", "\'s", "-", "_", "?" };

            string[] keywords = text.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < keywords.Length; i++)
            {
                keywords[i] = keywords[i].ToLower();
            }

            return keywords;
        }

        private int CountMatches(string[] keywords1, string[] keywords2)
        {
            int matches = 0;

            for (int i = 0; i < keywords1.Length; i++)
            {
                for (int j = 0; j < keywords2.Length; j++)
                {
                    if (keywords1[i].Contains(keywords2[j]) || keywords2[j].Contains(keywords1[i]))
                    {
                        matches++;
                    }
                }
            }

            return matches;
        }

        static string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }
    }
}
