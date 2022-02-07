using BackEndProject.DAL;
using BackEndProject.Models;
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
    public class BlogController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public BlogController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index(int page=1)
        {
            ViewBag.TotalPage = Math.Ceiling((decimal)_context.Blogs.Count() / 3);
            ViewBag.CurrentPage = page;
            List<Blog> modelBlog = _context.Blogs.Include(b=>b.Comments).Skip((page-1)*3).Take(3).ToList();
            return View(modelBlog);
        }
        [HttpGet]
        public IActionResult Index(string keyword,int page=1)
        {
            if (!string.IsNullOrEmpty(keyword))
            {
                List<Blog> blogs = _context.Blogs.Include(b => b.Comments).Where(f => f.Title.Contains(keyword)).ToList();
                if (!blogs.Any(f => f.Title.Contains(keyword)))
                {
                    ModelState.AddModelError("", "No result");
                }
                return View(blogs);
            }
            ViewBag.TotalPage = Math.Ceiling((decimal)_context.Blogs.Count() / 3);
            ViewBag.CurrentPage = page;
            List<Blog> modelBlog = _context.Blogs.Include(b=>b.Comments).Skip((page - 1) * 3).Take(3).ToList();
            return View(modelBlog);
        }
        public IActionResult Details(int id)
        {
            Blog blog = _context.Blogs.Include(b=>b.Comments).ThenInclude(c=>c.AppUser).FirstOrDefault(b => b.Id == id);
            List<Comment> comments = _context.Comments.Include(c => c.Blog).Include(c => c.AppUser).Where(c => c.BlogId == id).ToList();
            if (blog == null) return NotFound();
            return View(blog);
        }

        public IActionResult Search(string search)
        {
            List<Blog> blogs = _context.Blogs.Include(b=>b.Comments).Where(b => b.Title.ToLower().Trim().Contains(search.ToLower().Trim())).ToList();
            return PartialView("_BlogPartialView", blogs);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment(Comment comment)
        {
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (!ModelState.IsValid) return RedirectToAction("details", "blog", new { id = comment.BlogId });
            if (!_context.Blogs.Any(c => c.Id == comment.BlogId)) return NotFound();
            Comment cmmt = new Comment
            {
                Text = comment.Text,
                BlogId = comment.BlogId,
                CreatedTime = DateTime.Now,
                AppUserId = user.Id,
            };
            _context.Comments.Add(cmmt);
            _context.SaveChanges();
            return RedirectToAction("details", "blog", new { id = comment.BlogId });
        }
        public async Task<IActionResult> DeleteComment(int id)
        {
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (!ModelState.IsValid) return RedirectToAction("Index", "blog");
            if (!_context.Comments.Any(c => c.Id == id && c.AppUserId == user.Id)) return NotFound();
            Comment comment = _context.Comments.FirstOrDefault(c => c.Id == id && c.AppUserId == user.Id);
            _context.Comments.Remove(comment);
            _context.SaveChanges();
            return RedirectToAction("Details", "blog", new { id = comment.BlogId });
        }
    }
}
