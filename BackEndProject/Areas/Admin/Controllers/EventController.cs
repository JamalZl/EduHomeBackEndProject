using BackEndProject.DAL;
using BackEndProject.Extensions;
using BackEndProject.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEndProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class EventController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public EventController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(int page = 1)
        {
            ViewBag.TotalPage = Math.Ceiling((decimal)_context.Events.Count() / 3);
            ViewBag.CurrentPage = page;
            List<Event> events = _context.Events.Include(t => t.EventSpeakers).ThenInclude(es => es.Speaker).Skip((page - 1) * 3).Take(3).ToList();
            return View(events);
        }
        public IActionResult Create()
        {
            ViewBag.Speakers = _context.Speakers.ToList();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Event eventModel)
        {
            ViewBag.Speakers = _context.Speakers.ToList();
            eventModel.EventSpeakers = new List<EventSpeaker>();
            if (!ModelState.IsValid) return View();
            if (eventModel.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "Please insert an image");
                return View();
            }
            else
            {
                if (!eventModel.ImageFile.IsSizeOkay(2))
                {
                    ModelState.AddModelError("ImageFile", "Image size can not be more than 2MB");
                    return View();
                }
                if (!eventModel.ImageFile.IsImage())
                {
                    ModelState.AddModelError("ImageFile", "Please insert a valid image type such as jpg,png,jpeg etc");
                    return View();
                }
                eventModel.Image = eventModel.ImageFile.SaveImg(_env.WebRootPath, "assets/img/event");
            }
            if (eventModel.Name == null)
            {
                ModelState.AddModelError("Name", "Please enter a name");
                return View();
            }
            if (eventModel.Venue == null)
            {
                ModelState.AddModelError("Venue", "Please enter Venue");
                return View();
            }

            if (eventModel.SpeakerIds == null)
            {
                ModelState.AddModelError("SpeakerIds", "Please select at least  one speaker");
                return View();
            }
            else
            {
                foreach (var id in eventModel.SpeakerIds)
                {
                    EventSpeaker eventSpeaker = new EventSpeaker
                    {
                        Event = eventModel,
                        SpeakerId = id,
                        EventId = id

                    };
                    eventModel.EventSpeakers.Add(eventSpeaker);
                }
            }
            _context.Events.Add(eventModel);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Edit(int id)
        {
            ViewBag.Speakers = _context.Speakers.ToList();
            Event eventModel = _context.Events.Include(e=>e.EventSpeakers).ThenInclude(es=>es.Speaker).FirstOrDefault(t => t.Id == id);
            if (eventModel == null) return NotFound();
            return View(eventModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Event eventModel)
        {
            //eventModel.EventSpeakers = new List<EventSpeaker>();

            Event existEvent = _context.Events.Include(e => e.EventSpeakers).ThenInclude(es => es.Speaker).FirstOrDefault(t => t.Id == eventModel.Id);
            if (!ModelState.IsValid) return View();
            if (eventModel.ImageFile != null)
            {
                if (!eventModel.ImageFile.IsImage())
                {
                    ModelState.AddModelError("ImageFile", "Please insert a valid image type such as jpg,png,jpeg etc");
                    return View(existEvent);
                }
                if (!eventModel.ImageFile.IsSizeOkay(2))
                {
                    ModelState.AddModelError("ImageFile", "Image size can not be more than 2MB");
                    return View(existEvent);
                }
                Helpers.Helper.DeleteImg(_env.WebRootPath, "/assets/img/event", existEvent.Image);
                existEvent.Image = eventModel.ImageFile.SaveImg(_env.WebRootPath, "assets/img/event");
            }
            if (eventModel.Name == null)
            {
                ModelState.AddModelError("Name", "Please enter a name");
                return View(existEvent);
            }
            if (eventModel.SpeakerIds == null)
            {
                ModelState.AddModelError("SpeakerIds", "Please select at least one speaker");
                return View(existEvent);
            }
            if (eventModel.Venue == null)
            {
                ModelState.AddModelError("Venue", "Please enter Venue");
                return View(existEvent);
            }
            if (eventModel.SpeakerIds != null)
            {
                List<EventSpeaker> removableSpeakers = existEvent.EventSpeakers.Where(th => !eventModel.SpeakerIds.Contains(th.Id)).ToList();
                existEvent.EventSpeakers.RemoveAll(th => removableSpeakers.Any(rh => th.Id == rh.Id));
                if (removableSpeakers == null)
                {
                    ModelState.AddModelError("SpeakerIds", "Please select at least one speaker");
                    return View(existEvent);
                }
                foreach (var speakerId in eventModel.SpeakerIds)
                {
                    EventSpeaker eventSpeaker = existEvent.EventSpeakers.FirstOrDefault(th => th.SpeakerId == speakerId);
                    if (eventSpeaker == null)
                    {
                        EventSpeaker eSpeaker = new EventSpeaker
                        {
                            SpeakerId = speakerId,
                            EventId = existEvent.Id
                        };
                        existEvent.EventSpeakers.Add(eSpeaker);
                    }
                }
            }
            if (eventModel.SpeakerIds == null)
            {
                ModelState.AddModelError("SpeakerIds", "Please select at least one speaker");
                return View(existEvent);
            }

            existEvent.Name = eventModel.Name;
            existEvent.Venue = eventModel.Venue;
            existEvent.Date = eventModel.Date;
            existEvent.StartDate = eventModel.StartDate;
            existEvent.EndDate = eventModel.EndDate;
            existEvent.Description = eventModel.Description;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Delete(int id)
        {
            Event eventModel = _context.Events.FirstOrDefault(c => c.Id == id);
            Event existEvent = _context.Events.FirstOrDefault(c => c.Id == eventModel.Id);
            if (existEvent == null) return NotFound();
            if (eventModel == null) return Json(new { status = 404 });
            _context.Events.Remove(eventModel);
            _context.SaveChanges();
            return Json(new { status = 200 });
        }
        public IActionResult Comments(int EventId)
        {
            if (!_context.Comments.Any(c => c.EventId == EventId)) return RedirectToAction("index", "event");
            List<Comment> comments = _context.Comments.Include(c => c.AppUser).Where(c => c.EventId == EventId).ToList();
            return View(comments);
        }
        public IActionResult CStatusChange(int id)
        {
            if (!_context.Comments.Any(c => c.Id == id)) return RedirectToAction("Index", "event");
            Comment comment = _context.Comments.SingleOrDefault(c => c.Id == id);
            comment.IsAccess = comment.IsAccess ? false : true;
            _context.SaveChanges();
            return RedirectToAction("Comments", "event", new { EventId = comment.EventId });

        }
    }
}
