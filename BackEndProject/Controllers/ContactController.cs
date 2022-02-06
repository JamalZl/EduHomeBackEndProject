using BackEndProject.DAL;
using BackEndProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEndProject.Controllers
{
    public class ContactController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _context;
        public ContactController(UserManager<AppUser> userManager,AppDbContext context)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Message(ContactMessage mssg)
        {
            if (!ModelState.IsValid) return View();
            ContactMessage cm = new ContactMessage
            {
                Message = mssg.Message,
                Email = mssg.Email,
                SendDate = DateTime.Now
            };
            _context.ContactMessages.Add(cm);
            _context.SaveChanges();
            return RedirectToAction("index", "home");
        }
    }
}
