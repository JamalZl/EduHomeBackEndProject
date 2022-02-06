using BackEndProject.DAL;
using BackEndProject.Models;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class FacultyController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public FacultyController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(int page = 1)
        {
            ViewBag.TotalPage = Math.Ceiling((decimal)_context.Faculties.Count() / 6);
            ViewBag.CurrentPage = page;
            List<Faculty> faculties = _context.Faculties.Include(t => t.TeacherFaculties).ThenInclude(ct => ct.Teacher).Skip((page - 1) * 6).Take(6).ToList();
            return View(faculties);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Faculty faculty)
        {
            if (!ModelState.IsValid) return View();
            if (faculty.Name == null)
            {
                ModelState.AddModelError("Name", "Please enter faculty name");
                return View();
            }
            List<Faculty> faculties = _context.Faculties.Where(c => c.Name == faculty.Name).ToList();
            foreach (Faculty item in faculties)
            {
                if (item.Name.ToLower().Trim() == faculty.Name.ToLower().Trim())
                {
                    ModelState.AddModelError("Name", "Faculty is exist in database.Please enter different faculty name");
                    return View();
                }
            }
            _context.Faculties.Add(faculty);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Edit(int id)
        {
            Faculty faculty = _context.Faculties.FirstOrDefault(c => c.Id == id);
            if (faculty == null) return NotFound();
            return View(faculty);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Faculty faculty, int id)
        {
            if (faculty.Name == null)
            {
                ModelState.AddModelError("Name", "Enter a faculty name");
                return View(faculty);
            }
            Faculty nameControl = _context.Faculties.FirstOrDefault(t => t.Name.ToLower().Trim() == faculty.Name.ToLower().Trim());
            if (!ModelState.IsValid) return View();
            Faculty existFaculty = _context.Faculties.FirstOrDefault(c => c.Id == faculty.Id);
            if (existFaculty == null) return NotFound();
            if (nameControl != null && nameControl.Id != id)
            {
                ModelState.AddModelError("Name", "Faculty is exist in database.Please enter 2 faculty name");
                return View(existFaculty);
            }
            existFaculty.Name = faculty.Name;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Delete(int id)
        {
            Faculty faculty = _context.Faculties.FirstOrDefault(c => c.Id == id);
            Skill existSkill = _context.Skills.FirstOrDefault(c => c.Id == faculty.Id);
            if (existSkill == null) return NotFound();
            if (faculty == null) return Json(new { status = 404 });
            _context.Faculties.Remove(faculty);
            _context.SaveChanges();
            return Json(new { status = 200 });
        }
    }
}
