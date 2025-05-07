using LibraryDashboard2.Areas.Student.Data;
using LibraryDashboard2.Areas.Student.Models;
using LibraryDashboard2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace LibraryDashboard2.Areas.Student.Controllers
{
    [Area("Student")]
    //[Authorize(Policy = "StudentOnly")]
    //[Authorize(Roles = "Student")]
    public class DashboardController : Controller
    {
        private readonly StudentDbContext _context;

        public DashboardController(StudentDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AvailableBooks()
        {
            var books = _context.Books.ToList();
            var model = books;
            ViewData["InitialPartial"] = "_AvailableBooks";
                return View(ViewData["InitialPartial"].ToString(), model);

        }

        public IActionResult IssuedBooks()
        {

            var username = GetUsernameFromJwtCookie();
            if (string.IsNullOrEmpty(username))
            {
                return Unauthorized(); // Return Unauthorized if there's no username in JWT
            }

            // Fetch the issued books for this user
            var issuedBooks = _context.IssuedBooks
                .Where(b => b.StudentName == username)
                .ToList();

            return PartialView("_IssuedBooks", issuedBooks);
        }

        public IActionResult LateFees()
        {
            var today = DateTime.Now;
            var issuedBooks = _context.IssuedBooks.ToList();

            var lateFees = issuedBooks
                .Where(b => (today - b.IssueDate).TotalDays > 2)
                .Select(b => new LateFeeView
                {
                    BookId = b.BookId,
                    BookName = b.BookName,
                    StudentName = b.StudentName,
                    IssueDate = b.IssueDate,
                    DaysOverdue = (int)(today - b.IssueDate).TotalDays - 2,
                    Fee = ((int)(today - b.IssueDate).TotalDays - 2) * 10
                })
                .ToList();

            return View("_LateFees", lateFees);
        }

        [HttpPost("ReturnBook")]
        public IActionResult ReturnBook(int bookId)
        {
            try
            {
                var username = GetUsernameFromJwtCookie();

                var request = new LibraryDashboard2.Areas.Student.Models.ReturnBookRequest
                {
                    BookId = bookId,
                    Username = username,
                    RequestDate = DateTime.Now
                };

                _context.ReturnBookRequests.Add(request);
                _context.SaveChanges();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                var errorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return BadRequest($"Error saving return request: {errorMessage}");
            }
        }


        [HttpPost]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwt");
            return RedirectToAction("Login","Account");

        }

        


        private string GetUsernameFromJwtCookie()
        {
            var token = Request.Cookies["jwt"];  // Get JWT from cookies

            if (string.IsNullOrEmpty(token))
                return null;  // If no token is present, return null

            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

            var usernameClaim = jsonToken?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);

            return usernameClaim?.Value;  // Extract the username
        }


    }
}

