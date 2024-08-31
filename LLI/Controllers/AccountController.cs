using LLI.Data;
using LLI.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using System.Security.Claims;

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
        public IActionResult Register(RegisterViewModel users)
        {
            if (ModelState.IsValid)
            {
                _context.users.Add(users);
                _context.SaveChanges();
                return RedirectToAction("Login","Account"); 
            }
            return View(users);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            // If the user is already authenticated, redirect them to a different page
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
                var user = _context.users.FirstOrDefault(u => u.username == model.username);
                if (user != null && user.password == model.password)
                {
                    // Sign in the user using authentication
                    // This is an example; you'll need to implement the actual authentication logic
                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.username),
                // Add other claims here as needed
            };
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                        // Optionally set properties like IsPersistent, ExpiresUtc, etc.
                    };
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

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
