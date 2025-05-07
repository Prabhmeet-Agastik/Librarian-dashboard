using LibraryDashboard2.Data;
using LibraryDashboard2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static LibraryDashboard2.Models.Book;
namespace LibraryDashboard2.Controllers
{
    //[Authorize(Roles = "Librarian")]
    public class DashboardController : Controller
    {
        private readonly MyDbContext _context;

        public DashboardController(MyDbContext context)
        {
            _context = context;
        }
        public IActionResult Dashboard()
        {
            return View();
        }

        public IActionResult DashboardHome()
        {

            if (IsAjaxRequest())
                return PartialView("DashboardHome");

            

            ViewData["InitialPartial"] = "DashboardHome";
            return View("Dashboard");
            //return PartialView("DashboardHome");


        }

        //public IActionResult IssuedBooks()
        //{

        //    if (IsAjaxRequest())
        //        return PartialView("IssuedBooks");

        //    var students = _context.Users.ToList();
        //    var books = _context.Books.ToList();

        //    ViewBag.Students = students;
        //    ViewBag.Books = books;

        //    ViewData["InitialPartial"] = "IssuedBooks";
        //    return View("Dashboard");
        //    //return PartialView("IssuedBooks");

        //}

        public IActionResult IssuedBooks()
        {
            var students = _context.Users.Where(u => u.Role == "Student").ToList();

            var books = _context.Books
                                   .GroupBy(b => b.BookId)
                                   .Select(g => g.First())
                                    .ToList();


            var issuedBooks = _context.IssuedBooks.ToList(); // Fetch IssuedBooks

            ViewBag.Students = students;
            ViewBag.Books = books;
            ViewBag.IssuedBooks = issuedBooks;

            if (IsAjaxRequest())
                return PartialView("IssuedBooks");

            ViewData["InitialPartial"] = "IssuedBooks";
            return View("Dashboard");
        }

        public IActionResult AvailableBooks()
        {

            //if (IsAjaxRequest())
            //{
            //    var books = _context.Books.ToList();
            //    var model = books;
            //    ViewData["InitialPartial"] = "_AvailableBooksTablePartial";
            //    return PartialView(ViewData["InitialPartial"].ToString(), model);
            //}
            var books = _context.Books.ToList();
            var model = books;
            ViewData["InitialPartial"] = "AvailableBooks";
            return View(ViewData["InitialPartial"].ToString(), model);

            //ViewData["InitialPartial"] = "AvailableBooks";
            //return View("Dashboard");

            

        }



        //public IActionResult LateFees()
        //{

        //    if (IsAjaxRequest())
        //        return PartialView("LateFees");

        //    ViewData["InitialPartial"] = "LateFees";
        //    return View("Dashboard");



        //}

        //[HttpPost]
        //public IActionResult IssueBook([FromBody] IssuedBook model)
        //{
        //    var book = _context.Books.FirstOrDefault(b => b.BookId == model.BookId);
        //    book.Quantity -= 1;

        //    if (ModelState.IsValid)
        //    {
        //        _context.IssuedBooks.Add(model);
        //        _context.SaveChanges();
        //        return Json(new { success = true });
        //    }
        //    return Json(new { success = false, error = "Invalid data" });
        //}

        [HttpPost]
        public IActionResult IssueBook([FromBody] IssuedBook model)
        {
            // Find an available copy of the book by BookId
            var availableCopy = _context.Books
                .FirstOrDefault(b => b.BookId == model.BookId && !b.IsIssued);

            if (availableCopy == null)
            {
                return Json(new { success = false, error = "No available copies" });
            }

            if (ModelState.IsValid)
            {
                // Mark the copy as issued
                availableCopy.IsIssued = true;
                availableCopy.IssuedTo = model.StudentName;
                availableCopy.IssueDate = DateTime.Now;

                // Optionally track in a separate IssuedBooks table
                _context.IssuedBooks.Add(model);

                _context.SaveChanges();

                return Json(new { success = true });
            }

            return Json(new { success = false, error = "Invalid data" });
        }





        public IActionResult ReturnRequest()
        {
            var request = _context.ReturnBookRequests.ToList();

            return View(request);
        }

        //[HttpPost]
        //public JsonResult AcceptReturn(int bookId, string username)
        //{
        //    try
        //    {
        //        // 1. Delete from return requests table
        //        var request = _context.ReturnBookRequests
        //            .FirstOrDefault(r => r.BookId == bookId && r.Username == username);
        //        if (request != null)
        //        {
        //            _context.ReturnBookRequests.Remove(request);
        //        }

        //        // 2. Update issued book record (e.g., mark as returned or remove)
        //        var issuedBook = _context.IssuedBooks
        //            .FirstOrDefault(i => i.BookId == bookId && i.StudentName == username);
        //        if (issuedBook != null)
        //        {
        //            _context.IssuedBooks.Remove(issuedBook); // or update status if you track it
        //        }

        //        // 3. Increment available copies in the Books table
        //        var book = _context.Books.FirstOrDefault(b => b.BookId == bookId);
        //        if (book != null)
        //        {
        //            book.Quantity += 1;
        //        }

        //        _context.SaveChanges();

        //        return Json(new { success = true });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { success = false, message = ex.Message });
        //    }
        //}

        [HttpPost]
        public JsonResult AcceptReturn(int bookId, string username)
        {
            try
            {
                // 1. Delete from return requests table
                var request = _context.ReturnBookRequests
                    .FirstOrDefault(r => r.BookId == bookId && r.Username == username);
                if (request != null)
                {
                    _context.ReturnBookRequests.Remove(request);
                }

                // 2. Find the specific issued book copy (by BookId, username, and IsIssued = true)
                var issuedBook = _context.Books
                    .FirstOrDefault(b => b.BookId == bookId && b.IssuedTo == username && b.IsIssued == true);

                if (issuedBook != null)
                {
                    // Mark the book copy as available again
                    issuedBook.IsIssued = false;
                    issuedBook.IssuedTo = null;
                    issuedBook.IssueDate = null;
                }

                // 3. Remove tracking from IssuedBooks table (if still used for history/logging)
                var issuedRecord = _context.IssuedBooks
                    .FirstOrDefault(i => i.BookId == bookId && i.StudentName == username);
                if (issuedRecord != null)
                {
                    _context.IssuedBooks.Remove(issuedRecord);
                }

                _context.SaveChanges();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }





        public IActionResult LateFees()
        {
            var today = DateTime.Now;
            var issuedBooks = _context.IssuedBooks.ToList();

            var lateFees = issuedBooks
                .Where(b => (today - b.IssueDate).TotalDays > 2)
                .Select(b => new LateFeeViewModel
                {
                    BookId = b.BookId,
                    BookName = b.BookName,
                    StudentName = b.StudentName,
                    IssueDate = b.IssueDate,
                    DaysOverdue = (int)(today - b.IssueDate).TotalDays - 2,
                    Fee = ((int)(today - b.IssueDate).TotalDays - 2) * 10
                })
                .ToList();

            return View(lateFees);
        }










        //        [HttpPost]
        //        public IActionResult AddBook([FromBody] Book model)
        //        {
        //            if (ModelState.IsValid)
        //            {
        //                _context.Books.Add(model);
        //                _context.SaveChanges();
        //                return Json(new { success = true });
        //            }

        //            return Json(new { success = false, error = "Invalid data" });
        //}

        [HttpPost]
        public IActionResult AddBook([FromBody] BookViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Create multiple copies of the book
                var bookCopies = new List<Book>();

                for (int i = 0; i < model.Quantity; i++)
                {
                    var bookCopy = new Book
                    {
                        Title = model.Title,
                        Author = model.Author,
                        BookId = model.BookId,  // Use the same BookId for all copies
                        IsIssued = false,  // Default value, can be updated when issued
                        Genre = model.Genre,
                    };

                    bookCopies.Add(bookCopy);
                }

                // Add all copies to the database
                _context.Books.AddRange(bookCopies);
                _context.SaveChanges();

                return Json(new { success = true });
            }

            return Json(new { success = false, error = "Invalid data" });
        }




        private bool IsAjaxRequest()
        {
            return Request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }
    }
}
