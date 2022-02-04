using BackEndProject.DAL;
using BackEndProject.Models;
using BackEndProject.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<AppUser> _userManager;

        public CourseController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
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
            ViewBag.Categories = _context.Categories.Include(c => c.Courses).ToList();
            ViewBag.Tags = _context.Tags.ToList();
            ViewBag.Blogs = _context.Blogs.ToList();
            Course course = _context.Courses.Include(c=>c.Comments).ThenInclude(c=>c.AppUser).Include(c => c.Category).Include(c => c.CourseTags).ThenInclude(ct => ct.Tag).Include(c => c.CourseFeatures).FirstOrDefault(c => c.Id == id);
            List<Comment> comments = _context.Comments.Include(c => c.Course).Include(c => c.AppUser).Where(c => c.CourseId == id).ToList();
            if (course == null) return NotFound();
            return View(course);

        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment(Comment comment)
        {
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (!ModelState.IsValid) return RedirectToAction("details", "course", new { id = comment.CourseId });
            if (!_context.Courses.Any(c => c.Id == comment.CourseId)) return NotFound();
            Comment cmmt = new Comment
            {
                Text = comment.Text,
                CourseId = comment.CourseId,
                CreatedTime = DateTime.Now,
                AppUserId = user.Id,
            };
            //if (cmmt.Text.Length > 500)
            //{
            //    ModelState.AddModelError("Text", "You can not enter more than 500 characters");
            //    return View();
            //}
            //else if (cmmt.Text == null)
            //{
            //    ModelState.AddModelError("Text", "You can not send empty comment");
            //    return View();
            //}
            _context.Comments.Add(cmmt);
            _context.SaveChanges();
            return RedirectToAction("details", "course", new { id = comment.CourseId });
        }
        public async Task<IActionResult> DeleteComment(int id)
        {
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (!ModelState.IsValid) return RedirectToAction("Index", "Course");
            if (!_context.Comments.Any(c => c.Id == id && c.AppUserId == user.Id)) return NotFound();
            Comment comment = _context.Comments.FirstOrDefault(c => c.Id == id && c.AppUserId == user.Id);
            _context.Comments.Remove(comment);
            _context.SaveChanges();
            return RedirectToAction("Details", "Course", new { id = comment.CourseId });
        }
    }
}
