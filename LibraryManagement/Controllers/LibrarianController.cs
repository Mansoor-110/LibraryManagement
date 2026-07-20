using LibraryManagement.Filters;
using LibraryManagement.Helpers;
using LibraryManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Controllers
{
    [RoleAuthorize("Librarian")]
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
            var borrowRequests = _context.BorrowRequests.ToList();

            var issuedBooksRaw = _context.IssuedBooks
                .Select(b => new IssuedBookVM
                {
                    issuedBookId = b.issuedBookId,
                    BookName = b.BorrowRequest.Book.BookName,
                    BookImageName = b.BorrowRequest.Book.BookImageName,
                    User_name = b.BorrowRequest.User.User_name,
                    Email = b.BorrowRequest.User.Email,
                    issuedDate = b.issuedDate,
                    returnDate = b.returnDate,
                    fineAmount = b.fineAmount,
                    status = b.status,
                }).ToList();

            foreach (var item in issuedBooksRaw)
            {
                item.fineAmount = FineCalculator.Calculate(item.returnDate, item.status, item.fineAmount);
            }

            var vm = new LibrarianDashboardVM
            {
                TotalUsers = _context.Users.Count(),
                TotalBooks = _context.Books.Count(),

                TotalBorrowRequests = borrowRequests.Count,
                PendingRequests = borrowRequests.Count(b => b.Status == "Pending"),
                ApprovedRequests = borrowRequests.Count(b => b.Status == "Approved"),
                RejectedRequests = borrowRequests.Count(b => b.Status == "Rejected"),

                TotalIssuedBooks = issuedBooksRaw.Count,
                OverdueBooks = issuedBooksRaw.Count(b => b.fineAmount > 0 && b.status.ToLower() != "returned"),
                TotalOutstandingFine = issuedBooksRaw.Where(b => b.status.ToLower() != "returned").Sum(b => b.fineAmount),

                RecentBorrowRequests = _context.BorrowRequests
                    .Include(b => b.Book)
                    .Include(b => b.User)
                    .OrderByDescending(b => b.CreatedAt)
                    .Take(6)
                    .Select(b => new BorrowRequestVM
                    {
                        borrowRequestid = b.borrowRequestid,
                        BookName = b.Book.BookName,
                        BookImageName = b.Book.BookImageName,
                        qtyforborrow = b.Book.qtyforborrow,
                        User_name = b.User.User_name,
                        Email = b.User.Email,
                        CreatedAt = b.CreatedAt,
                        Status = b.Status,
                    }).ToList(),

                RecentIssuedBooks = issuedBooksRaw
                    .OrderByDescending(b => b.issuedDate)
                    .Take(6)
                    .ToList(),

                LowBorrowStockBooks = _context.Books
                    .Where(b => b.qtyforborrow <= 3)
                    .OrderBy(b => b.qtyforborrow)
                    .Take(5)
                    .ToList()
            };

            return View(vm);
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
                 .OrderByDescending(b => b.CreatedAt)
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

        public IActionResult ApproveRequest()
        {
           
            return View();
        }
        [HttpPost]
        public IActionResult ApproveRequest(int id, IssuedBook obj)
        {
            if (id != null && obj != null)
            {
                IssuedBook issueBook = new IssuedBook
                {
                    borrowRequestid=id,
                    issuedDate = obj.issuedDate,
                    returnDate = obj.returnDate,
                };
                _context.IssuedBooks.Add(issueBook);
                _context.SaveChanges();

                var data = _context.BorrowRequests.Find(id);
                data.Status = "Approved";
                var bookid = data.BookId;
                _context.SaveChanges();

                var bookdata = _context.Books.Find(bookid);
                bookdata.qtyforborrow = bookdata.qtyforborrow-1;
                _context.SaveChanges();
                

                return RedirectToAction("BorrowRequests", "Librarian");


            }
            return View(obj);
        }



        public IActionResult RejectRequest(int id)
        {
            var data = _context.BorrowRequests.Find(id);
            data.Status = "Rejected";
            _context.SaveChanges();
            return RedirectToAction("BorrowRequests","Librarian");
        }
        public IActionResult IssuedBook()
        {
            var data = _context.IssuedBooks
                 .Select(b => new IssuedBookVM
                 {
                     issuedBookId = b.issuedBookId,
                     BookName = b.BorrowRequest.Book.BookName,
                     BookImageName = b.BorrowRequest.Book.BookImageName,
                     User_name = b.BorrowRequest.User.User_name,
                     Email = b.BorrowRequest.User.Email,
                     issuedDate=b.issuedDate,
                     returnDate= b.returnDate,
                     fineAmount=b.fineAmount,
                     status = b.status,

                 }).ToList();
            foreach (var item in data)
            {
                item.fineAmount = FineCalculator.Calculate(item.returnDate, item.status, item.fineAmount);
            }
            return View(data);
        }
    }
}
