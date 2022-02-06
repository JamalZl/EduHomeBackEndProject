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
    public class SkillController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public SkillController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(int page = 1)
        {
            ViewBag.TotalPage = Math.Ceiling((decimal)_context.Skills.Count() / 6);
            ViewBag.CurrentPage = page;
            List<Skill> skills = _context.Skills.Include(t => t.TeacherSkills).ThenInclude(ct => ct.Teacher).Skip((page - 1) * 6).Take(6).ToList();
            return View(skills);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Skill skill)
        {
            if (!ModelState.IsValid) return View();
            if (skill.Name == null)
            {
                ModelState.AddModelError("Name", "Please enter skill name");
                return View();
            }
            List<Skill> skills = _context.Skills.Where(c => c.Name == skill.Name).ToList();
            foreach (Skill item in skills)
            {
                if (item.Name.ToLower().Trim() == skill.Name.ToLower().Trim())
                {
                    ModelState.AddModelError("Name", "Skill is exist in database.Please enter different skill name");
                    return View();
                }
            }
            _context.Skills.Add(skill);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Edit(int id)
        {
            Skill skill = _context.Skills.FirstOrDefault(c => c.Id == id);
            if (skill == null) return NotFound();
            return View(skill);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Skill skill, int id)
        {
            if (skill.Name == null)
            {
                ModelState.AddModelError("Name", "Enter a skill name");
                return View(skill);
            }
            Skill nameControl = _context.Skills.FirstOrDefault(t => t.Name.ToLower().Trim() == skill.Name.ToLower().Trim());
            if (!ModelState.IsValid) return View();
            Skill existSkill = _context.Skills.FirstOrDefault(c => c.Id == skill.Id);
            if (existSkill == null) return NotFound();
            if (nameControl != null && nameControl.Id != id)
            {
                ModelState.AddModelError("Name", "Skill is exist in database.Please enter different skill name");
                return View(existSkill);
            }
            existSkill.Name = skill.Name;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Delete(int id)
        {
            Skill skill = _context.Skills.FirstOrDefault(c => c.Id == id);
            Skill existSkill = _context.Skills.FirstOrDefault(c => c.Id == skill.Id);
            if (existSkill == null) return NotFound();
            if (skill == null) return Json(new { status = 404 });
            _context.Skills.Remove(skill);
            _context.SaveChanges();
            return Json(new { status = 200 });
        }
    }
}
