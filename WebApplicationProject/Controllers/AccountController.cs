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
            // ✅ Clear the logged-in user
            MockDB.CurrentLoggedInUserId = 0; // or default value
            
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        [HttpPost]
        public IActionResult Signup(User user)
        {
            user.Id = MockDB.UsersList.Max(u => u.Id) + 1;
            //สร้าง user
            MockDB.UsersList.Add(user);

            // ✅ Set the newly created user as the current logged-in user
            MockDB.CurrentLoggedInUserId = user.Id;
            
            // ✅ Store user ID in session for persistence
            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("Username", user.Username);

            return RedirectToAction("Login", new { success = true });
        }
        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var user = MockDB.UsersList
                .FirstOrDefault(u =>
                    u.Email == email &&
                    u.Password == password);

            if (user != null)
            {
                // ✅ Set the logged-in user as current
                MockDB.CurrentLoggedInUserId = user.Id;
                
                // ✅ Store both username and user ID in session
                HttpContext.Session.SetInt32("UserId", user.Id);
                HttpContext.Session.SetString("Username", user.Username);

                return RedirectToAction("Index", "Home");
            }
            
            ViewBag.Error = "Email or Password incorrect";
            return View();
        }
        [HttpPost]
        public JsonResult LoginAjax(string username, string password)
        {
            //var hash = HashPassword(password);

            var user = MockDB.UsersList
                .FirstOrDefault(u =>
                    u.Username == username &&
                    //u.Password == hash);
                    u.Password == password);

            if (user == null)
                return Json(new { success = false });

            //HttpContext.Session.SetString("Role", user.Role);

            return Json(new { success = true });
        }
    }
}