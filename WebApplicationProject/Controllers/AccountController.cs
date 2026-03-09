using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApplicationProject.Models;
using WebApplicationProject.Data;
using System.Net.Http;
using System.Text.Json;
using System.IO;

namespace WebApplicationProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Login(bool success = false)
        {
            if (success)
                ViewBag.Success = "Signup successful! Please login.";

            return View();
        }

        public IActionResult Signup()
        {
            return View();
        }

        public IActionResult Logout()
        {   
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        [HttpPost]
        public IActionResult Signup(SignupViewModel model)
        {
            if (string.IsNullOrEmpty(model.FName))
            {
                ViewBag.Error = "Firstname required";
                return View(model);
            }

            if (string.IsNullOrEmpty(model.SName))
            {
                ViewBag.Error = "Surname required";
                return View(model);
            }

            if (model.Birthday == default)
            {
                ViewBag.Error = "Birthday required";
                return View(model);
            }

            if (model.Birthday > DateTime.Now)
            {
                ViewBag.Error = "Birthday cannot be in the future";
                return View(model);
            }

            if (string.IsNullOrEmpty(model.Username))
            {
                ViewBag.Error = "Username required";
                return View(model);
            }

            if (_context.Users.Any(u => u.Username == model.Username))
            {
                ViewBag.Error = "Username already exists";
                return View(model);
            }

            if (_context.Users.Any(u => u.Email == model.Email))
            {
                ViewBag.Error = "Email already exists";
                return View(model);
            }

            if (model.Password != model.RepeatPassword)
            {
                ViewBag.Error = "Passwords do not match";
                return View(model);
            }

            var user = new User
            {
                FName = model.FName,
                SName = model.SName,
                Birthday = model.Birthday,
                Username = model.Username,
                Email = model.Email,
                Password = model.Password,
                Image = "https://ui-avatars.com/api/?name=" + model.FName[0] + model.SName[0] + "&background=random",
                Settings = new UserSettings()
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return RedirectToAction("Login", new { success = true });
        }
        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var user = _context.Users
                .FirstOrDefault(u => u.Email == email && u.Password == password);

            if (user != null)
            {
                HttpContext.Session.SetInt32("UserId", user.Id);
                HttpContext.Session.SetString("Username", user.Username);
                HttpContext.Session.SetString("Userimage", user.Image);

                return RedirectToAction("Home", "Home");
            }
            
            ViewBag.Error = "Email or Password incorrect";
            return View();
        }
        [HttpPost]
        public JsonResult LoginAjax(string username, string password)
        {
            var user = _context.Users
                .FirstOrDefault(u => u.Username == username && u.Password == password);

            if (user == null)
                return Json(new { success = false });

            return Json(new { success = true });
        }
        [HttpPost]
        public JsonResult CheckEmail(string email)
        {
            bool exists = _context.Users.Any(u => u.Email == email);

            return Json(new { exists = exists });
        }

    }
}