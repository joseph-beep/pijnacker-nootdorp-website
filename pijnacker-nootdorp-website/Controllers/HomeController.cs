using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using pijnacker_nootdorp_website.Models;
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

            return View(((IEnumerable<House>)houses, user));
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

            return View();
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

                    return Redirect("/");
                }
            }

            return View(data);
        }

        [Route("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("user");

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

                _context.Add(user);
                _context.SaveChanges();
                HttpContext.Session.Set("user", Encoding.ASCII.GetBytes(_context.Users.FirstOrDefault(u => u.Email == user.Email).Id.ToString()));
                
                return Redirect("/");
            }

            return View(user);
        }

        #region Register

        [Route("register")]
        public IActionResult Register()
        {
            return View();
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

        public List<House> GetHouses()
        {
            // stel in waar de database gevonden kan worden
            string connectionString = "Server=172.16.160.21;Port=3306;Database=110737;Uid=110737;Pwd=infsql2021;";
            //string connectionString = "Server=informatica.st-maartenscollege.nl;Port=3306;Database=110737;Uid=110737;Pwd=infsql2021;";
            // maak een lege lijst waar we de namen in gaan opslaan
            List<House> products = new List<House>();

            // verbinding maken met de database
            //using (MySqlConnection conn = new MySqlConnection(connectionString))
            //{
            //    // verbinding openen
            //    conn.Open();

            //    // SQL query die we willen uitvoeren
            //    MySqlCommand cmd = new MySqlCommand("select * from houses", conn);

            //    // resultaat van de query lezen
            //    using (var reader = cmd.ExecuteReader())
            //    {
            //        // elke keer een regel (of eigenlijk: database rij) lezen
            //        while (reader.Read())
            //        {
            //            House house = new House
            //            {
            //                ID = Convert.ToInt32(reader["id"]),
            //                Address = reader["address"].ToString(),
            //                Price = Convert.ToInt32(reader["price"]),
            //                Description = reader["description"].ToString()
            //            };

            //            // voeg de naam toe aan de lijst met namen
            //            products.Add(house);
            //        }
            //    }
            //}

            // return de lijst met namen
            return products;
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
