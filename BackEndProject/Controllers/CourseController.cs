using BackEndProject.DAL;
using BackEndProject.Models;
using BackEndProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEndProject.Controllers
{
    public class CourseController : Controller
    {
        private readonly AppDbContext _context;
        public CourseController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int page = 1)
        {
            ViewBag.TotalPage = Math.Ceiling((decimal)_context.Courses.Count() / 3);
            ViewBag.CurrentPage = page;
            CourseVM courseVM = new CourseVM
            {
                Courses = _context.Courses.Include(c => c.Category).Include(c => c.CourseTags).ThenInclude(ct => ct.Tag).Skip((page - 1) * 3).Take(3).ToList()
            };
            return View(courseVM);
        }
        public IActionResult Details(int id)
        {
            ViewBag.Categories = _context.Categories.Include(c=>c.Courses).ToList();
            ViewBag.Tags = _context.Tags.ToList();
            ViewBag.Blogs = _context.Blogs.ToList();
            Course course=_context.Courses.Include(c => c.Category).Include(c => c.CourseTags).ThenInclude(ct => ct.Tag).Include(c=>c.CourseFeatures).FirstOrDefault(c=>c.Id==id);
            if (course == null) return NotFound();
            return View(course);
             
        }
    }
}
