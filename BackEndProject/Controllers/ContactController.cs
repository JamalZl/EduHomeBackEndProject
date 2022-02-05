using BackEndProject.DAL;
using BackEndProject.Models;
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
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Message(ContactMessage messag)
        //{
        //    AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
        //    //if (!ModelState.IsValid) return RedirectToAction("index", "contact");
        //    //if (!_context.Any(c => c.Id == messag.AppUserId)) return NotFound();
        //    ContactMessage cmessage = new ContactMessage
        //    {
        //        Message = messag.Message,
        //        AppUser = messag.AppUser,
        //        CreatedTime = DateTime.Now,
        //        AppUserId = user.Id,
        //    };
        //    _context.ContactMessages.Add(cmessage);
        //    _context.SaveChanges();
        //    return RedirectToAction("index", "contact");
        //}
    }
}
