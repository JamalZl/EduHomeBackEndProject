using BackEndProject.DAL;
using BackEndProject.Models;
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
        public EventController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int page=1)
        {
            ViewBag.TotalPage = Math.Ceiling((decimal)_context.Events.Count() / 4);
            ViewBag.CurrentPage = page;
            List<Event> eventModel = _context.Events.Include(e => e.EventSpeakers).ThenInclude(ev => ev.Speaker).ThenInclude(s => s.Company).Include(e => e.EventSpeakers).ThenInclude(es => es.Speaker).ThenInclude(e => e.Position).Skip((page-1)*4).Take(4).ToList();
            return View(eventModel);
        }
        public IActionResult Details(int id)
        {
            ViewBag.EventSpeakers = _context.EventSpeakers.Include(es => es.Speaker).ThenInclude(s=>s.Position).Include(es=>es.Speaker).ThenInclude(s=>s.Company).Include(es => es.Event).Where(es => es.EventId == id).ToList();
            Event eventt = _context.Events.Include(e => e.EventSpeakers).ThenInclude(ev => ev.Speaker).ThenInclude(s => s.Company).Include(e => e.EventSpeakers).ThenInclude(es => es.Speaker).ThenInclude(e => e.Position).FirstOrDefault(t=>t.Id==id);
            if (eventt==null )
            {
                return NotFound();
            }
            return View(eventt);
        }
    }
}
