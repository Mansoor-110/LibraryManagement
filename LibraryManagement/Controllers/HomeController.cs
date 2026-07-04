using System.Diagnostics;
using LibraryManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace LibraryManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        //===============================================BOOKS================================================
        public IActionResult Books()
        {
            var data = _context.Books.Where(p => p.IsActive == true).ToList();
            return View(data);
        }
        public IActionResult BooksDetail(int id)
        {
            var find_specific = _context.Books.Find(id);
            return View(find_specific);
        }
        public IActionResult About()
        {
            return View();
        }
        public IActionResult Courses()
        {
            return View();
        }
        public IActionResult Profile()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }

        //=================================Register====================================\\
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(User obj)
        {
            if (ModelState.IsValid)
            {

            _context.Users.Add(obj);
            _context.SaveChanges();
                return RedirectToAction("Login","Home");
            }
            return View(obj);
        }
        //====================================Login====================================\\
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(string email , string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null) {
                ViewBag.Error = "Email doesn't exist";
                return View();
            }
            if (user.Password != password) {
                ViewBag.Error = "Email or password doesn't exist";
            }
            HttpContext.Session.SetInt32("UserId", user.User_id);
            HttpContext.Session.SetString("UserName", user.User_name);
            HttpContext.Session.SetString("Role", user.Role);
            
            if(user.Role == "Admin")
            {
                return RedirectToAction("Index", "Admin");
            }
            

            return RedirectToAction("Index", "Home");
            
        }
        //===============================Logout===========================
        public IActionResult Logout()
        {
           HttpContext.Session.Clear();
            return RedirectToAction("Index","Home");
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
