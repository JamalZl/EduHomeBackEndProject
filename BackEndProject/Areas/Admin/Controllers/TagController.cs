using BackEndProject.DAL;
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
    public class TagController : Controller
    {
        private readonly AppDbContext _context;
        private IWebHostEnvironment _env;
        public TagController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(int page = 1)
        {
            ViewBag.TotalPage = Math.Ceiling((decimal)_context.Tags.Count() / 4);
            ViewBag.CurrentPage = page;
            List<Tag> tags = _context.Tags.Include(t=>t.CourseTags).ThenInclude(ct=>ct.Course).Skip((page-1) * 4).Take(4).ToList();
            return View(tags);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Tag tag)
        {
            if (!ModelState.IsValid) return View();
            if (tag.Name==null)
            {
                ModelState.AddModelError("Name", "Enter a tag name");
                return View(tag);
            }
            List<Tag> tagNames = _context.Tags.Where(t => t.Name == tag.Name).ToList();
            foreach (Tag item in tagNames)
            {
                if (item.Name==tag.Name)
                {
                    ModelState.AddModelError("Name", "Tag is exist in database.Please enter different tag name");
                    return View();
                }
            }
            _context.Tags.Add(tag);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Edit(int id)
        {
            Tag tag = _context.Tags.FirstOrDefault(t=>t.Id==id);
            if (tag == null) return NotFound();
            return View(tag);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Tag tagModel,int id)
        {
            if (!ModelState.IsValid) return View();
            Tag existTag = _context.Tags.FirstOrDefault(t => t.Id == tagModel.Id);
            Tag nameControl = _context.Tags.Where(s => s.Name == tagModel.Name).FirstOrDefault();
            if (existTag == null) return NotFound();
            if (tagModel.Name == null)
            {
                ModelState.AddModelError("Name", "Enter a tag name");
                return View(tagModel);
            }
            if (nameControl.Id!=id)
            {
                ModelState.AddModelError("Name", "Tag is exist in database.Please enter different tag name");
                return View(existTag);
            }
            existTag.Name = tagModel.Name;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Delete(int id)
        {
            Tag tag = _context.Tags.FirstOrDefault(s=>s.Id==id);
            Tag existTag = _context.Tags.FirstOrDefault(t => t.Id == tag.Id);
            if (existTag == null) return NotFound();
            if (tag == null) return Json(new { status = 404 });
            _context.Tags.Remove(tag);
            _context.SaveChanges();
            return Json(new { status = 200 });
        }
    }
}
