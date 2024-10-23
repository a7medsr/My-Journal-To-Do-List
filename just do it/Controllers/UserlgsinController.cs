using just_do_it.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace just_do_it.Controllers
{
    public class UserlgsinController : Controller
    {
        private readonly justdoitDbcontext _context;

        public UserlgsinController(justdoitDbcontext context)
        {
            _context = context;
        }

        public IActionResult Signup()
        {
            return View();
        }

        public IActionResult SaveUser(string Name, string Email, string Password)
        {
         
            var existingUser = _context.Users.FirstOrDefault(u => u.Email == Email);
            if (existingUser != null)
            {
                ViewBag.NotificationMessage = "The email is already registered.";
                ViewBag.NotificationType = "danger";
                return View("Signup");
            }

            User user = new User
            {
                Name = Name,
                Email = Email,
                Password = Password
            };
            _context.Users.Add(user);
            _context.SaveChanges();
            return RedirectToAction("login");
        }

        public IActionResult login()
        {
            return View();
        }

        public IActionResult logine(string email, string pass)
        { 
           
            var user = _context.Users.FirstOrDefault(x => x.Email == email);
            if (user != null && user.Password == pass)
            {
                return RedirectToAction("UserView", "UserView", new { id = user.Id });
            }
           
                ViewBag.NotificationMessage = "email or passowerd is worng";
                ViewBag.NotificationType = "danger";
                return View("login");
        }
    }
}