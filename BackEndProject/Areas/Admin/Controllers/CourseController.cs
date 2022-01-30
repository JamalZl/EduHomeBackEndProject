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
    public class CourseController : Controller
    {
        private readonly AppDbContext _context;
        private IWebHostEnvironment _env;
        public CourseController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(int page=1)
        {
            ViewBag.TotalPage = Math.Ceiling((decimal)_context.Courses.Count() / 4);
            ViewBag.CurrentPage = page;
            List<Course> courses = _context.Courses.Skip((page-1)*4).Take(4).ToList();
            return View(courses);
        }
        public IActionResult Create()
        {
            ViewBag.Tags = _context.Tags.ToList();
            ViewBag.Categories = _context.Categories.ToList();
            Course course = _context.Courses.Include(c => c.CourseFeatures).FirstOrDefault();
            return View();
        }
        //public IActionResult Create(Course course)
        //{
        //    ViewBag.Tags = _context.Tags.ToList();
        //    ViewBag.Categories = _context.Categories.ToList();
        //    if (!ModelState.IsValid) return View();
        //    course.CourseTags = new List<CourseTags>();
        //    course.Category = new Category();
        //    foreach (var item in)
        //    {

        //    }
        //}
    }
}
