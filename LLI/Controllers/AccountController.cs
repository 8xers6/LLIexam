using LLI.Data;
using LLI.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.Scripting;
using Org.BouncyCastle.Crypto.Generators;

namespace LLI.Controllers
{
    
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        private readonly ApplicationDbContext _context;
        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }
        [HttpPost]
        public IActionResult Register(RegisterViewModel model, string confirmPassword)
        {
            if (ModelState.IsValid)
            {
                var userExists = _context.Users.Any(u => u.username == model.username);
                if (userExists)
                {
                    ModelState.AddModelError("Username", "Username is already taken.");
                }
                else if (model.password != confirmPassword)
                {
                    ModelState.AddModelError("ConfirmPassword", "The password and confirmation password do not match.");
                }
                else
                {
                    var hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.password);

                    var user = new User
                    {
                        username = model.username,
                        password = hashedPassword
                    };

                    _context.Users.Add(model); // Add the user object here
                    _context.SaveChanges();

                    TempData["SuccessMessage"] = "Registration successful! Please log in.";

                    return RedirectToAction("Login", "Account");
                }
            }

            return View(model);
        }





        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("List", "BarangayInformation");
            }

            return View(new RegisterViewModel());
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Login(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users.FirstOrDefault(u => u.username == model.username);
                if (user != null && user.password == model.password)
                {
                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.username),
                
            };
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                        // Optionally 
                    };
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
                    TempData["WelcomeMessage"] = "Welcome to the Resident List!";
                    return RedirectToAction("List", "BarangayInformation");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                    return View();
                }
            }

            return View(model);
        }
    }
}
