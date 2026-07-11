using LibraryManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Controllers
{
    public class LibrarianController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _WebHostEnvironment;

        public LibrarianController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
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
        //-----VIEW BOOK----
        public IActionResult ViewBook()
        {
            var data = _context.Books.ToList();
            return View(data);
        }
        //------- BORROW REQUESTS ----------

        public IActionResult BorrowRequests()
        {
            var data = _context.BorrowRequests
                .Include(b => b.Book)
                .Include(b => b.User)
                .Select(b => new BorrowRequestVM{
                borrowRequestid = b.borrowRequestid,
                BookName = b.Book.BookName,
                BookImageName = b.Book.BookImageName,
                qtyforborrow =b.Book.qtyforborrow,
                User_name =b.User.User_name,
                Email = b.User.Email,
                CreatedAt = b.CreatedAt,
                Status = b.Status,

                }).ToList();
            return View(data);
        }


    }
}
