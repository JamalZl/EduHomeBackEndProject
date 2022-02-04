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
    public class PositionController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public PositionController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(int page = 1)
        {
            ViewBag.TotalPage = Math.Ceiling((decimal)_context.Positions.Count() / 6);
            ViewBag.CurrentPage = page;
            List<Position> positions = _context.Positions.Include(p=>p.Teachers).Skip((page - 1) * 6).Take(6).ToList();
            return View(positions);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Position position)
        {
            if (!ModelState.IsValid) return View();
            if (position.Name == null)
            {
                ModelState.AddModelError("Name", "Please enter position name");
                return View();
            }
            List<Position> positions = _context.Positions.Where(c => c.Name == position.Name).ToList();
            foreach (Position item in positions)
            {
                if (item.Name.ToLower().Trim() == position.Name.ToLower().Trim())
                {
                    ModelState.AddModelError("Name", "Position is exist in database.Please enter different position name");
                    return View();
                }
            }
            _context.Positions.Add(position);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Edit(int id)
        {
            Position position = _context.Positions.FirstOrDefault(c => c.Id == id);
            if (position == null) return NotFound();
            return View(position);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Position position, int id)
        {
            if (position.Name == null)
            {
                ModelState.AddModelError("Name", "Enter a position name");
                return View(position);
            }
            Position nameControl = _context.Positions.FirstOrDefault(t => t.Name.ToLower().Trim() == position.Name.ToLower().Trim());
            if (!ModelState.IsValid) return View();
            Position existPosition = _context.Positions.FirstOrDefault(c => c.Id == position.Id);
            if (existPosition == null) return NotFound();
            if (nameControl != null && nameControl.Id != id)
            {
                ModelState.AddModelError("Name", "Position is exist in database.Please enter different position name");
                return View();
            }
            existPosition.Name = position.Name;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Delete(int id)
        {
            Position position = _context.Positions.FirstOrDefault(c => c.Id == id);
            Position existPosition = _context.Positions.FirstOrDefault(c => c.Id == position.Id);
            if (existPosition == null) return NotFound();
            if (position == null) return Json(new { status = 404 });
            _context.Positions.Remove(position);
            _context.SaveChanges();
            return Json(new { status = 200 });
        }
    }
}
