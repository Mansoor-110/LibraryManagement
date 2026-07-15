using LibraryManagement.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Controllers
{   
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _WebHostEnvironment;

        public AdminController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _WebHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }
        //============================================Users==========================================================
        public IActionResult Users()
        {
            var data = _context.Users.ToList();
            return View(data);
        }
        //-----Edit 
        public IActionResult UserEdit(int id)
        {
            var find_specific = _context.Users.Find(id);
            return View(find_specific);
        }
        [HttpPost]
        public IActionResult UserEdit(int id, User obj)
        {
            _context.Users.Update(obj);
            _context.SaveChanges();
            return RedirectToAction("Users","Admin");
        }

        //-----Delete
        public IActionResult UserDelete(int id)
        {
            var obj = _context.Users.Find(id);
            _context.Remove(obj);
            _context.SaveChanges();
            return RedirectToAction("Users","Admin");
        }

        //============================================Books CRUD=====================================================
        public IActionResult AddBook()
        {
            return View();
        }
        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit = 1048576000)]
        [RequestSizeLimit(1048576000)]
        public IActionResult AddBook(Book obj)
        {
            string filename = "";
            if (obj.BookImage != null)
            {
                string uploadfolder = Path.Combine(_WebHostEnvironment.WebRootPath, "BookImages");
                filename = Guid.NewGuid().ToString() + " " + obj.BookImage.FileName;
                string filepath = Path.Combine(uploadfolder, filename);
                string extension = Path.GetExtension(obj.BookImage.FileName);

                if (extension == ".jfif" || extension == ".jpg" || extension == ".png" || extension == ".jpeg")
                {
                    obj.BookImage.CopyTo(new FileStream(filepath, FileMode.Create));

                    if (obj.BookImage.Length <= 1048576000)
                    {
                        Book book = new Book
                        {

                            BookName = obj.BookName,
                            BookPrice = obj.BookPrice,
                            quantity = obj.quantity,
                            qtyforborrow = obj.qtyforborrow,
                            BookDescription = obj.BookDescription,
                            BookAuthor = obj.BookAuthor,
                            BookLanguage = obj.BookLanguage,
                            BookPages = obj.BookPages,
                            IsActive = obj.IsActive,
                            BookImageName = filename // database column of image name
                        };

                        _context.Books.Add(book);
                        _context.SaveChanges();
                        return RedirectToAction("ViewBook", "Admin");

                    }
                }

            }                return View(obj);
        }
        //-----VIEW BOOK----
        public IActionResult ViewBook()
        {
            var data = _context.Books.ToList();
            return View(data);
        }
        //-----EDIT BOOK----
        public IActionResult BookEdit(int id)
        {
            var find_specific = _context.Books.Find(id);
            return View(find_specific);
        }
        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit = 1048576000)]
        [RequestSizeLimit(1048576000)]
        public IActionResult BookEdit(int id, Book obj)
        {
            var data = _context.Books.Find(id);
            if(data == null)
            {
                return NotFound();
            }
            data.BookName = obj.BookName;
            data.BookAuthor = obj.BookAuthor;
            data.BookLanguage = obj.BookLanguage;
            data.BookDescription = obj.BookDescription;
            data.BookPages = obj.BookPages;
            data.quantity = obj.quantity;
            data.qtyforborrow = obj.qtyforborrow;
            data.BookPrice = obj.BookPrice;
            data.IsActive = obj.IsActive;

            if(obj.BookImage != null)
            {
                string uploadfolder = Path.Combine(
                    _WebHostEnvironment.WebRootPath,
                    "BookImages"
                );

                string extension = Path.GetExtension(obj.BookImage.FileName).ToLower();

                if (extension == ".jfif" || extension == ".jpg" ||
                    extension == ".png" || extension == ".jpeg")
                {
                    string filename = Guid.NewGuid() + "_" + obj.BookImage.FileName;
                    string filepath = Path.Combine(uploadfolder, filename);

                    obj.BookImage.CopyTo(new FileStream(filepath, FileMode.Create));

                    data.BookImageName= filename;
                }
            }



            _context.SaveChanges();
            return RedirectToAction("ViewBook","Admin");
        }
        //-----DELETE BOOK-------
        public IActionResult BookDelete(int id)
        {
            var obj = _context.Books.Find(id);
            _context.Books.Remove(obj);
            _context.SaveChanges();

            return RedirectToAction("ViewBook","Admin");
        }


        //========================================Logout=========================================
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        //=========================================Admin ORDERS========================================

        [HttpGet]
        public IActionResult Orders()
        {
            //var role = HttpContext.Session.GetString("Role");
            //if (role != "Admin")
            //{
            //    return RedirectToAction("Login", "Home");
            //}

            var orders = _context.Orders
                .Include(o => o.OrderItems)
                .OrderByDescending(o => o.CreatedAt)
                .ToList();

            return View(orders);
        }

        [HttpPost]
        public IActionResult UpdateOrderStatus(int id, string status)
        {
            //var role = HttpContext.Session.GetString("Role");
            //if (role != "Admin")
            //{
            //    return RedirectToAction("Login", "Home");
            //}

            var order = _context.Orders.FirstOrDefault(o => o.OrderId == id);
            if (order != null)
            {
                order.OrderStatus = status;
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Order status updated successfully";
            }

            return RedirectToAction("Orders","Admin");
        }

        [HttpPost]
        public IActionResult OrderDelete(int id)
        {
            //var role = HttpContext.Session.GetString("Role");
            //if (role != "Admin")
            //{
            //    return RedirectToAction("Login", "Home");
            //}

            var order = _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefault(o => o.OrderId == id);

            if (order != null)
            {
                _context.OrderItems.RemoveRange(order.OrderItems);
                _context.Orders.Remove(order);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Order deleted successfully";
            }

            return RedirectToAction("Orders","Admin");
        }
    }
}
