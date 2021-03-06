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
            return View(new Contact { });
        }

        [Route("contact")]
        [HttpPost]
        public IActionResult Contact(Contact contact)
        {
            if (ModelState.IsValid)
            {
                _context.Contacts.Add(contact);
                _context.SaveChanges();
                
                return Redirect("/");
            }

            return View(contact);
        }

        [Route("profile")]
        public IActionResult Profile()
        {
            if (Website.User == null)
            {
                return Redirect("/login-error");
            }

            return View(new UserProfile { User = Website.User });
        }

        [Route("profile")]
        [HttpPost]
        public IActionResult Profile(UserProfile profile)
        {
            if (!string.IsNullOrEmpty(profile.NewFirstName))
            {
                Website.User.FirstName = profile.NewFirstName;
                Website.User.LastName = profile.NewLastName;

                _context.Entry(Website.User).Property(x => x.FirstName).IsModified = true;
                _context.Entry(Website.User).Property(x => x.LastName).IsModified = true;
            }

            if (!string.IsNullOrEmpty(profile.NewEmail)
                && profile.NewEmail == profile.RepeatedNewEmail
                && ComputeSha256Hash(profile.PasswordEmail) == Website.User.Password)
            {
                Website.User.Email = profile.NewEmail;

                _context.Entry(Website.User).Property(x => x.Email).IsModified = true;
            }

            if (!string.IsNullOrEmpty(profile.NewPassword)
                && profile.NewPassword == profile.RepeatedNewPassword
                && ComputeSha256Hash(profile.OldPassword) == Website.User.Password)
            {
                Website.User.Password = ComputeSha256Hash(profile.NewPassword);

                _context.Entry(Website.User).Property(x => x.Password).IsModified = true;
            }

            _context.SaveChanges();

            profile.User = Website.User;

            return View(profile);
        }

        [Route("cart")]
        public IActionResult Cart()
        {
            if (Website.User == null)
            {
                return Redirect("/login-error");
            }

            if (Website.User.Order == null)
            {
                Website.User.Order = _context.Orders.FirstOrDefault(x => x.UserId == Website.User.Id);

                if (Website.User.Order == null)
                {
                    _context.Orders.Add(new Order { UserId = Website.User.Id });
                    _context.SaveChanges();

                    Website.User.Order = _context.Orders.FirstOrDefault(x => x.UserId == Website.User.Id);
                }
            }

            User user = Website.User;
            Order order = _context.Orders.FirstOrDefault(u => u.UserId == user.Id);

            return View(new CartModel
            {
                User = user,
                Order = order
            });
        }
        
        [Route("cart/pay")]
        public IActionResult Pay()
        {
            _context.Orders.Remove(_context.Orders.FirstOrDefault(u => Website.User.Id == u.UserId));
            _context.Orders.Add(new Order { UserId = Website.User.Id });

            _context.SaveChanges();

            Website.User.Order = _context.Orders.FirstOrDefault(x => x.UserId == Website.User.Id);

            return Redirect("/");
        }

        [Route("cart/add/{houseId}")]
        public IActionResult AddCart(int houseId)
        {
            if (Website.User == null)
            {
                return Redirect("/login-error");
            }
            else
            {
                if (Website.User.Order == null)
                {
                    Website.User.Order = _context.Orders.FirstOrDefault(x => x.UserId == Website.User.Id);

                    if (Website.User.Order == null)
                    {
                        _context.Orders.Add(new Order { UserId = Website.User.Id });
                        _context.SaveChanges();

                        Website.User.Order = _context.Orders.FirstOrDefault(x => x.UserId == Website.User.Id);
                    }
                }

                OrderItem newOrderItem = new OrderItem
                {
                    HouseId = houseId,
                    OrderId = Website.User.Order.Id
                };

                _context.OrderItems.Add(newOrderItem);

                _context.SaveChanges();

                return Redirect($"/cart");
            }
        }

        [Route("login-error")]
        public IActionResult LoginError(string message)
        {
            return View(message);
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
                _context.Orders.Add(new Order { UserId = user.Id });
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

            //int? outdoorArea_minimum = int.TryParse(search.OutdoorArea_Minimum, out int data3) ? data3 : null;
            //int? outdoorArea_maximum = int.TryParse(search.OutdoorArea_Maximum, out int data4) ? data4 : null;

            //int? indoorArea_minimum = int.TryParse(search.IndoorArea_Minimum, out int data5) ? data5 : null;
            //int? indoorArea_maximum = int.TryParse(search.IndoorArea_Maximum, out int data6) ? data6 : null;

            //int? buildYear_minimum = int.TryParse(search.BuildYear_Minimum, out int data7) ? data7 : null;
            //int? buildYear_maximum = int.TryParse(search.BuildYear_Maximum, out int data8) ? data8 : null;

            //bool wheelchair = search.Wheelchair == "on";
            //bool car = search.Car == "on";
            //bool publicTransport = search.PublicTransport == "on";

            string[] searchQueryKeywords = GetKeywords(search.SearchQuery);

            List<House> houses = new List<House>();
            Dictionary<House, int> matches = new Dictionary<House, int>();
            foreach (var house in _context.Houses)
            {
                if (price_minimum == null || house.Price < price_minimum) continue;
                else if (house.Price > price_maximum) continue;

                //if (outdoorArea_minimum == null || house.OutdoorArea < outdoorArea_minimum) continue;
                //else if (house.OutdoorArea > outdoorArea_maximum) continue;

                //if (indoorArea_minimum == null || house.IndoorArea < indoorArea_minimum) continue;
                //else if (house.IndoorArea > indoorArea_maximum) continue;

                //if (buildYear_minimum == null || house.BuildYear < buildYear_minimum) continue;
                //else if (house.BuildYear > buildYear_maximum) continue;

                //bool filter = true;
                //if (!wheelchair && !car && !publicTransport)
                //{
                //    filter = false;
                //}
                //else
                //{
                //    if (wheelchair && house.AccessData.wheelchair) filter = false;
                //    else if (car && house.AccessData.car) filter = false;
                //    else if (publicTransport && house.AccessData.publicTransport) filter = false;
                //}

                //if (filter) continue;

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
