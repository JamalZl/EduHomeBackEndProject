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
    public class HobbyController : Controller
    {
        private readonly AppDbContext _context;
        private IWebHostEnvironment _env;
        public HobbyController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(int page = 1)
        {
            ViewBag.TotalPage = Math.Ceiling((decimal)_context.Hobbies.Count() / 6);
            ViewBag.CurrentPage = page;
            List<Hobby> hobbies = _context.Hobbies.Include(t => t.TeacherHobbies).ThenInclude(ct => ct.Teacher).Skip((page - 1) * 6).Take(6).ToList();
            return View(hobbies);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Hobby hobby)
        {
            if (!ModelState.IsValid) return View();
            if (hobby.Name == null)
            {
                ModelState.AddModelError("Name", "Please enter category name");
                return View();
            }
            List<Hobby> hobbies = _context.Hobbies.Where(c => c.Name == hobby.Name).ToList();
            foreach (Hobby item in hobbies)
            {
                if (item.Name.ToLower().Trim() == hobby.Name.ToLower().Trim())
                {
                    ModelState.AddModelError("Name", "Hobby is exist in database.Please enter different hobby name");
                    return View();
                }
            }
            _context.Hobbies.Add(hobby);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Edit(int id)
        {
            Hobby hobby = _context.Hobbies.FirstOrDefault(c => c.Id == id);
            if (hobby == null) return NotFound();
            return View(hobby);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Hobby hobby, int id)
        {
            if (hobby.Name == null)
            {
                ModelState.AddModelError("Name", "Enter a category name");
                return View(hobby);
            }
            Hobby nameControl = _context.Hobbies.FirstOrDefault(t => t.Name.ToLower().Trim() == hobby.Name.ToLower().Trim());
            if (!ModelState.IsValid) return View();
            Hobby existHobby = _context.Hobbies.FirstOrDefault(c => c.Id == hobby.Id);
            if (existHobby == null) return NotFound();
            if (nameControl != null && nameControl.Id != id)
            {
                ModelState.AddModelError("Name", "Hobby is exist in database.Please enter different hobby name");
                return View(existHobby);
            }
            existHobby.Name = hobby.Name;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Delete(int id)
        {
            Hobby hobby = _context.Hobbies.FirstOrDefault(c => c.Id == id);
            Hobby existHobby = _context.Hobbies.FirstOrDefault(c => c.Id == hobby.Id);
            if (existHobby == null) return NotFound();
            if (hobby == null) return Json(new { status = 404 });
            _context.Hobbies.Remove(hobby);
            _context.SaveChanges();
            return Json(new { status = 200 });
        }
    } 
}
