using BackEndProject.Models;
using BackEndProject.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEndProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppUser> _signInResult;

        public AccountController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<AppUser> signInResult)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInResult = signInResult;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM login)
        {
            if (!ModelState.IsValid) return View();

            AppUser user = await _userManager.FindByNameAsync(login.Username);

            if (user == null)
            {
                ModelState.AddModelError("", "Username or password is incorrect");
                return View();
            }
            if (!user.IsAdmin)
            {
                ModelState.AddModelError("", "Username or password is incorrect");
                return View();
            }

            Microsoft.AspNetCore.Identity.SignInResult result = await _signInResult.PasswordSignInAsync(user, login.Password, false, false);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Username or password is incorrect");
                return View();
            }
            return RedirectToAction("index", "dashboard");

        }
        //public async Task CreateRole()
        //{
        //    await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
        //    await _roleManager.CreateAsync(new IdentityRole("Admin"));
        //    await _roleManager.CreateAsync(new IdentityRole("Member"));
        //}

        //public async Task CreateAdmin()
        //{
        //    AppUser user = new AppUser
        //    {
        //        UserName = "Camal123",
        //        Email = "jamalaz@code.edu.az",
        //        Name = "Camal",
        //        Surname = "Zeynalli",
        //        Country = "Azerbaijan"
        //    };
        //    await _userManager.CreateAsync(user, "camal123");
        //    await _userManager.AddToRoleAsync(user, "SuperAdmin");
        //}
        public async Task<IActionResult> Logout()
        {
            await _signInResult.SignOutAsync();
            return RedirectToAction("login", "account");
        }
    }
}
