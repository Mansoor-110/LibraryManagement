using LibraryManagement.Helpers;
using LibraryManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.Diagnostics;
using System.Net;

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
        public IActionResult ReturnBook()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ReturnBook(int id)
        {
            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;

            var issuedBook =  _context.IssuedBooks
                .Include(i => i.BorrowRequest)
                    .ThenInclude(br => br.Book)
                .FirstOrDefault(i => i.issuedBookId == id);

            if (issuedBook == null)
            {
                TempData["ErrorMessage"] = "Ye issued book record nahi mila.";
                return RedirectToAction("Profile", "Home");
            }

            // Security check — sirf apni hi book return kar sake, kisi aur ki nahi
            if (issuedBook.BorrowRequest.User_id != userId)
            {
                return Forbid();
            }

            if (issuedBook.status == "Returned")
            {
                TempData["ErrorMessage"] = "Ye book already return ho chuki hai.";
                return RedirectToAction("Profile", "Home");
            }

            // Fine finalize/lock karo return ke waqt
            issuedBook.fineAmount = FineCalculator.Calculate(issuedBook.returnDate, "Issued", 0);
            issuedBook.actualReturnDate = DateTime.Now;
            issuedBook.status = "Returned";

            // Book ki available quantity wapas barhao
            if (issuedBook.BorrowRequest?.Book != null)
            {
                issuedBook.BorrowRequest.Book.qtyforborrow += 1;
            }

            _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Book successfully return ho gayi.";
            return RedirectToAction("Profile","Home");
        }
        //==========================================PROFILE======================================
        public IActionResult Profile()
        {
            int userId = HttpContext.Session.GetInt32("UserId") ?? 0; // apne login/session ke hisab se adjust karo

            var user = _context.Users.FirstOrDefault(u => u.User_id == userId);
            if (user == null) return NotFound();

            // Currently issued (active) books
            var issuedBooks = _context.IssuedBooks
                .Where(i => i.BorrowRequest.User_id == userId && i.status == "Issued")
                .Select(i => new IssuedBookCardVM
                {
                    IssuedBookId = i.issuedBookId,
                    BookName = i.BorrowRequest.Book.BookName,
                    BookAuthor = i.BorrowRequest.Book.BookAuthor,
                    BookImageName = i.BorrowRequest.Book.BookImageName,
                    IssuedDate = i.issuedDate,
                    DueDate = i.returnDate,
                    CurrentFine = i.fineAmount // filhal raw, neeche update hoga
                })
                .ToList();

            // Live fine calculate karo har issued book ke liye
            foreach (var book in issuedBooks)
            {
                book.CurrentFine = FineCalculator.Calculate(book.DueDate, "Issued", 0);
            }

            // Borrow requests (Pending/Approved/Rejected sab)
            var borrowRequests = _context.BorrowRequests
                .Where(r => r.User_id == userId)
                .OrderByDescending(r => r.CreatedAt)
                .Select(r => new BorrowRequestRowVM
                {
                    BorrowRequestId = r.borrowRequestid,
                    BookName = r.Book.BookName,
                    RequestedOn = r.CreatedAt,
                    Status = r.Status
                })
                .ToList();

            // History (jo books return ho chuki hain)
            var history =  _context.IssuedBooks
                .Where(i => i.BorrowRequest.User_id == userId && i.status == "Returned")
                .OrderByDescending(i => i.actualReturnDate)
                .Select(i => new HistoryRowVM
                {
                    BookName = i.BorrowRequest.Book.BookName,
                    IssuedDate = i.issuedDate,
                    ReturnedDate = i.actualReturnDate,
                    FinePaid = i.fineAmount
                })
                .ToList();

            var vm = new ProfileVM
            {
                UserId = user.User_id,
                UserName = user.User_name,
                Email = user.Email,
                MemberSince = user.CreatedAt,
                OutstandingFine = issuedBooks.Sum(b => b.CurrentFine),
                BooksCurrentlyIssuedCount = issuedBooks.Count,
                PendingRequestsCount = borrowRequests.Count(r => r.Status == "Pending"),
                IssuedBooks = issuedBooks,
                BorrowRequests = borrowRequests,
                History = history
            };

            return View(vm);
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
            

            _context.Users.Add(obj);
            _context.SaveChanges();
                return RedirectToAction("Login","Home");
            
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
                return View();
            }
            HttpContext.Session.SetInt32("UserId", user.User_id);
            HttpContext.Session.SetString("UserName", user.User_name);
            HttpContext.Session.SetString("Role", user.Role);
            
            if(user.Role == "Admin")
            {
                return RedirectToAction("Index", "Admin");
            }else if(user.Role == "Librarian")
            {
                return RedirectToAction("Index", "Librarian");
            }


                return RedirectToAction("Index", "Home");
            
        }
        //===============================Logout===========================
        public IActionResult Logout()
        {
           HttpContext.Session.Clear();
            return RedirectToAction("Index","Home");
        }

        public IActionResult BorrowRequest(int id)
        {
            int bookid = id;
            var userId = HttpContext.Session.GetInt32("UserId");
            

            if(userId == null)
            {
                return RedirectToAction("Login", "Home");
            }
            var existingRecord = _context.BorrowRequests.FirstOrDefault(br =>
            br.User_id == userId &&
            br.BookId == bookid && 
            br.Status =="Pending");
            if (existingRecord != null)
            {
                TempData["ExistingMessage"] = "Your request is in the process. The librarian will review it shortly.";
                return RedirectToAction("BooksDetail", new { id = bookid });
            }
            BorrowRequest BorrowObj = new BorrowRequest
            {
                BookId = bookid,
                User_id = (int)userId

            };
            _context.BorrowRequests.Add(BorrowObj);
            _context.SaveChanges();


            TempData["SuccessMessage"] = "Your request is successfully sent to the Librarian. He'll review it shortly";
            return RedirectToAction("Profile", "Home");
        }
        //==============================CART===========================
        private int CurrentUserId => HttpContext.Session.GetInt32("UserId") ?? 0;

        public IActionResult Cart()
        {
            var userId = CurrentUserId;
            if (userId == null)
            {
                return RedirectToAction("Login", "Home");
            }

            var cartItems = _context.CartItems
                .Where(c => c.User_id == userId)
                .OrderByDescending(c => c.AddedAt)
                .Select(c => new CartItemVM
                {
                    CartItemId = c.CartItemId,
                    BookId = c.BookId,
                    BookName = c.Book.BookName,
                    BookAuthor = c.Book.BookAuthor,
                    BookImageName = c.Book.BookImageName,
                    Quantity = c.Quantity,
                    BookPrice = c.Book.BookPrice,
                    AddedAt = c.AddedAt
                })
                .ToList();

            return View(cartItems);
        }

        [HttpPost]
        public IActionResult AddToCart(int id, int quantity = 1)
        {
            int userId = CurrentUserId;

            var book = _context.Books.FirstOrDefault(b => b.BookId == id);
            if (book == null)
            {
                TempData["ErrorMessage"] = "Book nahi mili.";
                return RedirectToAction("BooksDetail", new { id });
            }

            if (quantity < 1) quantity = 1;




            bool alreadyInCart = _context.CartItems.Any(c => c.User_id == userId && c.BookId == id);

            if (alreadyInCart)
            {
                TempData["ErrorMessage"] = "Already In Cart";
                return RedirectToAction("Cart", "Home");
            }

            _context.CartItems.Add(new CartItem
            {
                User_id = userId,
                BookId = id,
                Quantity = quantity
            });

            _context.SaveChanges();

            TempData["SuccessMessage"] = "Book Is added to the cart";
            return RedirectToAction("Cart","Home");
        }

        [HttpPost]
        public IActionResult Remove(int id) // id = CartItemId
        {
            int userId = CurrentUserId;

            var item = _context.CartItems.FirstOrDefault(c => c.CartItemId == id && c.User_id == userId);

            if (item != null)
            {
                _context.CartItems.Remove(item);
                _context.SaveChanges();
            }

            return RedirectToAction("Cart","Home");
        }
        //==============================WISHLIST===========================
 
public IActionResult Wishlist()
        {
            var userId = CurrentUserId;
            if (userId == 0)
            {
                return RedirectToAction("Login", "Home");
            }

            var wishlistItems = _context.WishlistItems
                .Where(w => w.User_id == userId)
                .OrderByDescending(w => w.AddedAt)
                .Select(w => new WishlistItemVM
                {
                    WishlistItemId = w.WishlistItemId,
                    BookId = w.BookId,
                    BookName = w.Book.BookName,
                    BookAuthor = w.Book.BookAuthor,
                    BookImageName = w.Book.BookImageName,
                    BookPrice = w.Book.BookPrice,
                    Quantity = w.Book.quantity,
                    AddedAt = w.AddedAt
                })
                .ToList();

            return View(wishlistItems);
        }

        [HttpPost]
        public IActionResult AddToWishlist(int id) // id = BookId
        {
            int userId = CurrentUserId;
            var book = _context.Books.FirstOrDefault(b => b.BookId == id);
            if (book == null)
            {
                TempData["ErrorMessage"] = "Book nahi mili.";
                return RedirectToAction("BooksDetail", new { id });
            }

            bool alreadyInWishlist = _context.WishlistItems.Any(w => w.User_id == userId && w.BookId == id);
            if (alreadyInWishlist)
            {
                TempData["ErrorMessage"] = "Already In Wishlist";
                return RedirectToAction("Wishlist", "Home");
            }

            _context.WishlistItems.Add(new WishlistItem
            {
                User_id = userId,
                BookId = id
            });
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Book added to your wishlist";
            return RedirectToAction("Wishlist", "Home");
        }

        [HttpPost]
        public IActionResult RemoveFromWishlist(int id) // id = WishlistItemId
        {
            int userId = CurrentUserId;
            var item = _context.WishlistItems.FirstOrDefault(w => w.WishlistItemId == id && w.User_id == userId);
            if (item != null)
            {
                _context.WishlistItems.Remove(item);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Removed from wishlist";
            }
            return RedirectToAction("Wishlist", "Home");
        }

        [HttpPost]
        public IActionResult MoveToCart(int id) // id = WishlistItemId
        {
            int userId = CurrentUserId;
            var wishItem = _context.WishlistItems.FirstOrDefault(w => w.WishlistItemId == id && w.User_id == userId);
            if (wishItem != null)
            {
                bool alreadyInCart = _context.CartItems.Any(c => c.User_id == userId && c.BookId == wishItem.BookId);
                if (!alreadyInCart)
                {
                    _context.CartItems.Add(new CartItem
                    {
                        User_id = userId,
                        BookId = wishItem.BookId,
                        Quantity = 1
                    });
                }
                _context.WishlistItems.Remove(wishItem);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Book moved to cart";
            }
            return RedirectToAction("Cart", "Home");
        }
        //==============================CHECKOUT===========================

        [HttpGet]
        public IActionResult Checkout()
        {
            var userId = CurrentUserId;
            if (userId == 0)
            {
                return RedirectToAction("Login", "Home");
            }

            var cartItems = _context.CartItems
                .Where(c => c.User_id == userId)
                .Select(c => new CartItemVM
                {
                    CartItemId = c.CartItemId,
                    BookId = c.BookId,
                    BookName = c.Book.BookName,
                    BookAuthor = c.Book.BookAuthor,
                    BookImageName = c.Book.BookImageName,
                    BookPrice = c.Book.BookPrice,
                    Quantity = c.Quantity,
                    AddedAt = c.AddedAt
                })
                .ToList();

            if (!cartItems.Any())
            {
                TempData["ErrorMessage"] = "Aapka cart khaali hai.";
                return RedirectToAction("Cart", "Home");
            }

            var vm = new CheckoutVM
            {
                CartItems = cartItems,
                TotalAmount = cartItems.Sum(c => c.BookPrice * c.Quantity)
            };

            return View(vm);
        }

        [HttpPost]
        public IActionResult PlaceOrder(CheckoutVM model)
        {
            var userId = CurrentUserId;
            if (userId == 0)
            {
                return RedirectToAction("Login", "Home");
            }

            var cartItems = _context.CartItems
                .Where(c => c.User_id == userId)
                .ToList();

            if (!cartItems.Any())
            {
                TempData["ErrorMessage"] = "Aapka cart khaali hai.";
                return RedirectToAction("Cart", "Home");
            }

            if (string.IsNullOrWhiteSpace(model.FullName) || string.IsNullOrWhiteSpace(model.Phone) ||
                string.IsNullOrWhiteSpace(model.Address) || string.IsNullOrWhiteSpace(model.City))
            {
                TempData["ErrorMessage"] = "Please fill all delivery details.";
                return RedirectToAction("Checkout", "Home");
            }

            var order = new Order
            {
                User_id = userId,
                FullName = model.FullName,
                Phone = model.Phone,
                Address = model.Address,
                City = model.City,
                PaymentMethod = "Cash on Delivery",
                OrderStatus = "Pending",
                OrderItems = new List<OrderItem>()
            };

            int total = 0;
            foreach (var item in cartItems)
            {
                var book = _context.Books.FirstOrDefault(b => b.BookId == item.BookId);
                if (book == null) continue;

                int lineTotal = book.BookPrice * item.Quantity;
                total += lineTotal;

                order.OrderItems.Add(new OrderItem
                {
                    BookId = book.BookId,
                    BookName = book.BookName,
                    BookAuthor = book.BookAuthor,
                    BookImageName = book.BookImageName,
                    Quantity = item.Quantity,
                    Price = book.BookPrice
                });
            }
            order.TotalAmount = total;

            _context.Orders.Add(order);
            _context.CartItems.RemoveRange(cartItems);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Order placed successfully!";
            return RedirectToAction("OrderConfirmation", new { id = order.OrderId });
        }

        [HttpGet]
        public IActionResult OrderConfirmation(int id)
        {
            var userId = CurrentUserId;
            var order = _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefault(o => o.OrderId == id && o.User_id == userId);

            if (order == null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(order);
        }

        [HttpGet]
        public IActionResult MyOrders()
        {
            var userId = CurrentUserId;
            if (userId == 0)
            {
                return RedirectToAction("Login", "Home");
            }

            var orders = _context.Orders
                .Where(o => o.User_id == userId)
                .Include(o => o.OrderItems)
                .OrderByDescending(o => o.CreatedAt)
                .ToList();

            return View(orders);
        }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
