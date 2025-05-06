using LibraryDashboard2.Data;
using LibraryDashboard2.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;

namespace LibraryDashboard2.Controllers
{
    public class AccountController : Controller
    {
        private readonly MyDbContext _context;
        //private readonly string _jwtKey;

        public AccountController(MyDbContext context)
        {
            _context = context;
        }

        [HttpGet("login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet("register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpGet("/")]
        public IActionResult LoginRoot()
        {
            return RedirectToAction("Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwt");
            return RedirectToAction("Login", "Account");
        }

        //[HttpPost]
        //public IActionResult Login(User model)
        //{
        //    var user = _context.Users.FirstOrDefault(u => u.Username == model.Username);

        //    if (user != null && BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
        //    {
        //        var key = Encoding.UTF8.GetBytes("Z3ZfK8d!xR#9uBt4$P2wL6m@E1qT7nJz\r\n");

        //        var claims = new List<Claim>
        //        {
        //            new Claim(ClaimTypes.Name, user.Username),
        //            new Claim(ClaimTypes.Role, user.Role)
        //        };

        //        var token = new JwtSecurityToken(
        //            issuer: "LibraryApp",
        //            audience: "LibraryApp",
        //            claims: claims,
        //            expires: DateTime.UtcNow.AddHours(1),
        //            signingCredentials: new SigningCredentials(
        //                new SymmetricSecurityKey(key),
        //                SecurityAlgorithms.HmacSha256)
        //        );

        //        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        //        Response.Cookies.Append("jwt", jwt, new CookieOptions
        //        {
        //            HttpOnly = true,
        //            Secure = true,
        //            SameSite = SameSiteMode.Strict,
        //            Expires = DateTime.UtcNow.AddHours(1)
        //        });


        //    }

        //    ViewBag.Error = "Invalid credentials!";
        //    return View();
        //}

        [HttpPost("login")]
        public IActionResult Login(User model)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == model.Username);

            

            if (user != null && BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
            {

                // JWT Secret Key
                var key = Encoding.UTF8.GetBytes("Z3ZfK8d!xR#9uBt4$P2wL6m@E1qT7nJz");

                // Create claims
                var claims = new List<Claim>
               {
                  new Claim(ClaimTypes.Name, user.Username),
                  new Claim(ClaimTypes.Role, user.Role)
                 };

                // Create token
                var token = new JwtSecurityToken(
                    issuer: "LibraryApp",
                    audience: "LibraryApp",
                    claims: claims,
                    expires: DateTime.UtcNow.AddHours(1),
                    signingCredentials: new SigningCredentials(
                        new SymmetricSecurityKey(key),
                        SecurityAlgorithms.HmacSha256)
                );

                // Serialize token
                var jwt = new JwtSecurityTokenHandler().WriteToken(token);

                // Set token in HTTP-only secure cookie
                Response.Cookies.Append("jwt", jwt, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddHours(1)
                });



                // Redirect based on the role
                if (user.Role == "Student")
                {
                    return RedirectToAction("Index", "Dashboard", new { area = "Student" });
                }
                else if (user.Role == "Librarian")
                {
                    return RedirectToAction("Dashboard", "Dashboard");
                }
                else
                {
                    // Default or fallback dashboard
                    return RedirectToAction("Account", "Login");
                }
            }

            ViewBag.Error = "Invalid credentials!";
            return View();
        }






        [HttpPost("register")]
        public IActionResult Register(User user)
        {
            if (_context.Users.Any(u => u.Username == user.Username))
            {
                ViewBag.Error = "Username already exists.";
                return View();
            }

            if (string.IsNullOrEmpty(user.Role) || !(user.Role == "Student" || user.Role == "Librarian"))
            {
                ViewBag.Error = "Please select a valid role.";
                return View();
            }

            // Hash the password before storing
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

            _context.Users.Add(user);
            _context.SaveChanges();

            TempData["Message"] = "Registration successful! Please log in.";
            return RedirectToAction("Login");
        }

        [HttpGet("dashboard")]
        public IActionResult Dashboard()
        {
            return View();
        }

        public IActionResult Welcome()
        {
            return View();
        }
    }
}
