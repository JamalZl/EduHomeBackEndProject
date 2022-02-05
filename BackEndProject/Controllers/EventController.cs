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

namespace BackEndProject.Controllers
{
    public class EventController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public EventController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index(int page=1)
        {
            ViewBag.TotalPage = Math.Ceiling((decimal)_context.Events.Count() / 4);
            ViewBag.CurrentPage = page;
            List<Event> eventModel = _context.Events.Include(e => e.EventSpeakers).ThenInclude(ev => ev.Speaker).ThenInclude(s => s.Company).Include(e => e.EventSpeakers).ThenInclude(es => es.Speaker).ThenInclude(e => e.Position).Skip((page-1)*4).Take(4).ToList();
            return View(eventModel);
        }
        [HttpGet]
        public IActionResult Index(string keyword,int page=1)
        {
            if (!string.IsNullOrEmpty(keyword))
            {
                
                List<Event> events = _context.Events.Include(e=>e.EventSpeakers).ThenInclude(es=>es.Speaker).Where(f => f.Name.Contains(keyword)|| f.EventSpeakers.FirstOrDefault().Speaker.Name.Contains(keyword)).ToList();
                if (!events.Any(f => f.Name.Contains(keyword)))
                {
                    ModelState.AddModelError("", "No result");
                    return View(events);
                }
                return View(events);
            }
            ViewBag.TotalPage = Math.Ceiling((decimal)_context.Events.Count() / 4);
            ViewBag.CurrentPage = page;
            List<Event> eventModel = _context.Events.Include(e => e.EventSpeakers).ThenInclude(ev => ev.Speaker).ThenInclude(s => s.Company).Include(e => e.EventSpeakers).ThenInclude(es => es.Speaker).ThenInclude(e => e.Position).Skip((page - 1) * 4).Take(4).ToList();
            return View(eventModel);

        }
        public IActionResult Details(int id)
        {
            ViewBag.EventSpeakers = _context.EventSpeakers.Include(es => es.Speaker).ThenInclude(s=>s.Position).Include(es=>es.Speaker).ThenInclude(s=>s.Company).Include(es => es.Event).Where(es => es.EventId == id).ToList();
            Event eventt = _context.Events.Include(e=>e.Comments).ThenInclude(c=>c.AppUser).Include(e => e.EventSpeakers).ThenInclude(ev => ev.Speaker).ThenInclude(s => s.Company).Include(e => e.EventSpeakers).ThenInclude(es => es.Speaker).ThenInclude(e => e.Position).FirstOrDefault(t=>t.Id==id);
            if (eventt==null )
            {
                return NotFound();
            }
            return View(eventt);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment(Comment comment)
        {
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (!ModelState.IsValid) return RedirectToAction("details", "event", new { id = comment.EventId });
            if (!_context.Events.Any(c => c.Id == comment.EventId)) return NotFound();
            Comment cmmt = new Comment
            {
                Text = comment.Text,
                EventId = comment.EventId,
                CreatedTime = DateTime.Now,
                AppUserId = user.Id,
            };
            _context.Comments.Add(cmmt);
            _context.SaveChanges();
            return RedirectToAction("details", "event", new { id = comment.EventId });
        }
        public async Task<IActionResult> DeleteComment(int id)
        {
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (!ModelState.IsValid) return RedirectToAction("Index", "Event");
            if (!_context.Comments.Any(c => c.Id == id && c.AppUserId == user.Id)) return NotFound();
            Comment comment = _context.Comments.FirstOrDefault(c => c.Id == id && c.AppUserId == user.Id);
            _context.Comments.Remove(comment);
            _context.SaveChanges();
            return RedirectToAction("Details", "Event", new { id = comment.EventId });
        }
    }
}
