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
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;
        private IWebHostEnvironment _env;
        public CategoryController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(int page=1)
        {
            ViewBag.TotalPage = Math.Ceiling((decimal)_context.Categories.Count() / 4);
            ViewBag.CurrentPage = page;
            List<Category> categories = _context.Categories.Include(t => t.Courses).Skip((page - 1) * 4).Take(4).ToList();
            return View(categories);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            if (!ModelState.IsValid) return View();
            if (category.Name==null)
            {
                ModelState.AddModelError("Name", "Please enter category name");
                return View();
            }
            List<Category> categories = _context.Categories.Where(c => c.Name == category.Name).ToList();
            foreach (Category item in categories)
            {
                if (item.Name.ToLower().Trim()==category.Name.ToLower().Trim())
                {
                    ModelState.AddModelError("Name", "Category is exist in database.Please enter different category name");
                    return View();
                }
            }
            _context.Categories.Add(category);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Edit(int id)
        {
            Category category = _context.Categories.FirstOrDefault(c => c.Id == id);
            if (category==null) return NotFound();
            return View(category);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category,int id)
        {
            if (category.Name == null)
            {
                ModelState.AddModelError("Name", "Enter a category name");
                return View(category);
            }
            Category nameControl = _context.Categories.FirstOrDefault(t => t.Name.ToLower().Trim() == category.Name.ToLower().Trim());
            if (!ModelState.IsValid) return View();
            Category existCategory = _context.Categories.FirstOrDefault(c => c.Id == category.Id);
            if (existCategory == null) return NotFound();
            if (nameControl != null && nameControl.Id != id)
            {
                ModelState.AddModelError("Name", "Category is exist in database.Please enter different category name");
                return View(existCategory);
            }
            existCategory.Name = category.Name;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Delete(int id)
        {
            Category category = _context.Categories.FirstOrDefault(c => c.Id == id);
            Category existCategory = _context.Categories.FirstOrDefault(c => c.Id == category.Id);
            if (existCategory == null) return NotFound();
            if (category == null) return Json(new { status = 404 });
            _context.Categories.Remove(category);
            _context.SaveChanges();
            return Json(new { status = 200 });
        }
    }
}
