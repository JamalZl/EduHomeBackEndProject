using BackEndProject.DAL;
using BackEndProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEndProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin")]

    public class UserManagmentController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppUser> _signInResult;
        public UserManagmentController(AppDbContext context, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<AppUser> signInResult)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInResult = signInResult;
        }
        public IActionResult Index(int page=1)
        {
            ViewBag.TotalPage = Math.Ceiling((decimal)_context.Users.Count() / 5);
            ViewBag.CurrentPage = page;
            List<AppUser> users = _context.Users.Skip((page - 1) * 5).Take(5).ToList();
            return View(users);
        }
        public async Task<IActionResult> RoleChanger(string id)
        {
            AppUser user = await _userManager.FindByIdAsync(id);
            if (user.IsAdmin)
            {
                user.IsAdmin = false;
            }
            else
            {
                user.IsAdmin = true;
            }
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
