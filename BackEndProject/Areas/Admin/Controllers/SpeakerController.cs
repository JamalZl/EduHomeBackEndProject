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
    public class SpeakerController : Controller
    {

        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public SpeakerController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(int page = 1)
        {
            ViewBag.TotalPage = Math.Ceiling((decimal)_context.Speakers.Count() / 2);
            ViewBag.CurrentPage = page;
            List<Speaker> Speakers = _context.Speakers.Include(s=>s.Position).Include(s=>s.Company).Skip((page - 1) * 2).Take(2).ToList();
            return View(Speakers);
        }
        public IActionResult Create()
        {
            ViewBag.Positions = _context.Positions.ToList();
            ViewBag.Companies = _context.Companies.ToList();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Speaker speaker)
        {
            ViewBag.Positions = _context.Positions.ToList();
            ViewBag.Companies = _context.Companies.ToList();
            if (!ModelState.IsValid) return View();
            if (speaker.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "Please insert an image");
                return View();
            }
            else
            {
                if (!speaker.ImageFile.IsSizeOkay(2))
                {
                    ModelState.AddModelError("ImageFile", "Image size can not be more than 2MB");
                    return View();
                }
                if (!speaker.ImageFile.IsImage())
                {
                    ModelState.AddModelError("ImageFile", "Please insert a valid image type such as jpg,png,jpeg etc");
                    return View();
                }
                speaker.Image = speaker.ImageFile.SaveImg(_env.WebRootPath, "assets/img/event");
            }

            if (speaker.Name==null)
            {
                ModelState.AddModelError("Name", "Please insert a name");
                return View();
            }
            if (speaker.PositionId == 0)
            {
                ModelState.AddModelError("PositionId", "Please select one position name");
                return View();
            }
            if (speaker.CompanyId == 0)
            {
                ModelState.AddModelError("CompanyId", "Please select one company name");
                return View();
            }
            _context.Speakers.Add(speaker);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            ViewBag.Positions = _context.Positions.ToList();
            ViewBag.Companies = _context.Companies.ToList();
            Speaker speaker = _context.Speakers.FirstOrDefault(s => s.Id == id);
            if (speaker == null) return NotFound();
            return View(speaker);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Speaker speaker, int id)
        {
            ViewBag.Positions = _context.Positions.ToList();
            ViewBag.Companies = _context.Companies.ToList();
            Speaker existSpeaker = _context.Speakers.FirstOrDefault(s => s.Id == speaker.Id);
            if (existSpeaker == null) return NotFound();
            if (!ModelState.IsValid) return View();
            if (speaker.ImageFile != null)
            {
                if (!speaker.ImageFile.IsImage())
                {
                    ModelState.AddModelError("ImageFile", "Please insert a valid image type such as jpg,png,jpeg etc");
                    return View(existSpeaker);
                }
                if (!speaker.ImageFile.IsSizeOkay(2))
                {
                    ModelState.AddModelError("ImageFile", "Image size can not be more than 2MB");
                    return View(existSpeaker);
                }
                Helpers.Helper.DeleteImg(_env.WebRootPath, "/assets/img/event", existSpeaker.Image);
                existSpeaker.Image = speaker.ImageFile.SaveImg(_env.WebRootPath, "assets/img/event");
            }
            if (speaker.Name == null)
            {
                ModelState.AddModelError("Name", "Please insert a name");
                return View(existSpeaker);
            }
            if (speaker.PositionId == 0)
            {
                ModelState.AddModelError("PositionId", "Please select one position name");
                return View(existSpeaker);
            }
            if (speaker.CompanyId == 0)
            {
                ModelState.AddModelError("CompanyId", "Please select one company name");
                return View(existSpeaker);
            }
            existSpeaker.Name = speaker.Name;
            existSpeaker.PositionId = speaker.PositionId;
            existSpeaker.CompanyId = speaker.CompanyId;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Delete(int id)
        {
            Speaker speaker = _context.Speakers.FirstOrDefault(s => s.Id == id);
            Speaker existSpeaker = _context.Speakers.FirstOrDefault(s => s.Id == speaker.Id);
            if (existSpeaker == null) return NotFound();
            if (speaker == null) return Json(new { status = 404 });
            _context.Speakers.Remove(speaker);
            _context.SaveChanges();
            return Json(new { status = 200 });
        }

    }
}
