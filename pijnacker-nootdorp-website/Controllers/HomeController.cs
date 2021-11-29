using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using pijnacker_nootdorp_website.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace pijnacker_nootdorp_website.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            List<House> houses = GetHouses();

            if (HttpContext.Session.TryGetValue("user", out byte[] userId_raw))
            {
                string userId = Encoding.ASCII.GetString(userId_raw);

                ViewData["user"] = userId;
            }
            else
            {
                ViewData["user"] = "Niemand";
            }

            return View(houses);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        /// <summary>
        /// Wordt gecalled on submit van form in login.cshtml en ook on address
        /// </summary>
        [Route("login")]
        public IActionResult Login(string username, string password)
        {
            if (password == "123")
            {
                HttpContext.Session.Set("user", Encoding.ASCII.GetBytes(username));
                return Redirect("/");
            }
            else
            {
                return View();
            }
        }

        [Route("contact")]
        public IActionResult Contact()
        {
            List<House> houses = GetHouses();

            return View(houses);
        }

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
    }
}
