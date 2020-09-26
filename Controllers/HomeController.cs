using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using LoginRegistration.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore; 
using Microsoft.AspNetCore.Http; //Session

namespace LoginRegistration.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private MyContext dbContext;

        public HomeController(ILogger<HomeController> logger,MyContext _context)
        {
            _logger = logger;
            dbContext = _context;
        }
        [HttpGet("")]
        public IActionResult Index()
        {
            if(HttpContext.Session.GetString("first_name") !=null)
            {
                return RedirectToAction("Success");
            }
            return View();
        }
        [HttpPost("register")]
        public IActionResult Register(User user)
        {
            if(ModelState.IsValid)
            {
                if(dbContext.Users.Any( user => user.email == user.email))
                {
                    ModelState.AddModelError("email","Email already in use!");
                }
                PasswordHasher<User> hasher = new PasswordHasher<User>();
                user.pw_hash= hasher.HashPassword(user,user.pw_hash);
                dbContext.Add(user);
                dbContext.SaveChanges();

                HttpContext.Session.SetString("first_name",user.first_name);
                HttpContext.Session.SetString("last_name",user.last_name);
                HttpContext.Session.SetString("email",user.email);
                return RedirectToAction("Success");
            }
            return View("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Success()
        {
            if(HttpContext.Session.GetString("first_name")==null)
            {
                return RedirectToAction("Index");
            }
            return View();
        }
        [HttpPost("login")]
        public IActionResult Login(LoginUser user)
        {
            if(ModelState.IsValid)
            {
                var dbUser = dbContext.Users.FirstOrDefault(u => u.email ==user.Email); //grab profile from db
                if(dbUser==null) //first check email
                {
                    ModelState.AddModelError("Email","Invalid Email/Password");
                    return View("Index");
                }
                var hasher = new PasswordHasher<LoginUser>();
                var result = hasher.VerifyHashedPassword(user, dbUser.pw_hash, user.password); //Hash and check password
                if(result ==0)
                {
                    ModelState.AddModelError("password","Invalid Email/Password");
                    return View("Index");
                }
                HttpContext.Session.SetString("first_name", dbUser.first_name); //save profile in session
                HttpContext.Session.SetString("last_name",dbUser.last_name);
                HttpContext.Session.SetString("Email",dbUser.email);
                return RedirectToAction("Index");
            }
            return View("Index");
        }
        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
