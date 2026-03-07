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
            //Clear the logged-in user
            MockDB.CurrentLoggedInUserId = 0; // or default value
            
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

            if (MockDB.UsersList.Any(u => u.Username == model.Username))
            {
                ViewBag.Error = "Username already exists";
                return View(model);
            }

            if (string.IsNullOrEmpty(model.Email))
            {
                ViewBag.Error = "Email required";
                return View(model);
            }

            if (MockDB.UsersList.Any(u => u.Email == model.Email))
            {
                ViewBag.Error = "Email already exists";
                return View(model);
            }

            if (string.IsNullOrEmpty(model.Password))
            {
                ViewBag.Error = "Password required";
                return View(model);
            }

            if (model.Password != model.RepeatPassword)
            {
                ViewBag.Error = "Passwords do not match";
                return View(model);
            }

            var user = new User
            {
                Id = MockDB.UsersList.Max(u => u.Id) + 1,
                FName = model.FName,
                SName = model.SName,
                Birthday = model.Birthday,
                Username = model.Username,
                Email = model.Email,
                Password = model.Password,
                Image = "https://ui-avatars.com/api/?name=" + model.FName[0] + model.SName[0] + "&background=random"
            };

            MockDB.UsersList.Add(user);

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
        [HttpPost]
        public JsonResult CheckEmail(string email)
        {
            bool exists = MockDB.UsersList.Any(u => u.Email == email);

            return Json(new { exists = exists });
        }

    }
}