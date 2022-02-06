using BackEndProject.DAL;
using BackEndProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEndProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class ContactMessageController : Controller
    {
        private readonly AppDbContext _context;
        public ContactMessageController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int page=1)
        {
            ViewBag.TotalPage= ViewBag.TotalPage = Math.Ceiling((decimal)_context.ContactMessages.Count() / 5);
            ViewBag.CurrentPage = page;
            List<ContactMessage> cMessages = _context.ContactMessages.Skip((page - 1) * 5).Take(5).ToList();
            return View(cMessages);
        }
        public IActionResult Delete(int id)
        {
            ContactMessage cmessage = _context.ContactMessages.FirstOrDefault(c => c.Id == id);
            if (cmessage == null) return NotFound();
            _context.ContactMessages.Remove(cmessage);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
