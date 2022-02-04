using BackEndProject.DAL;
using BackEndProject.Extensions;
using BackEndProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEndProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = "SuperAdmin,Admin")]

    public class BlogController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public BlogController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(int page = 1)
        {
            ViewBag.TotalPage = Math.Ceiling((decimal)_context.Blogs.Count() / 3);
            ViewBag.CurrentPage = page;
            List<Blog> blogs = _context.Blogs.Skip((page - 1) * 3).Take(3).ToList();
            return View(blogs);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Blog blog)
        {

            if (!ModelState.IsValid) return View();
            if (blog.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "Please insert an image");
                return View();
            }
            else
            {
                if (!blog.ImageFile.IsImage())
                {
                    ModelState.AddModelError("ImageFile", "Please insert a valid image type such as jpg,png,jpeg etc");
                    return View();
                }
                if (!blog.ImageFile.IsSizeOkay(2))
                {
                    ModelState.AddModelError("ImageFile", "Image size can not be more than 2MB");
                    return View();
                }
                blog.Image = blog.ImageFile.SaveImg(_env.WebRootPath, "assets/img/blog");
            }
            if (blog.Title==null)
            {
                ModelState.AddModelError("Title", "Please enter a title");
                return View();
            }
            _context.Blogs.Add(blog);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Edit(int id)
        {
            Blog blog = _context.Blogs.FirstOrDefault(s => s.Id == id);
            if (blog == null) return NotFound();
            return View(blog);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Blog blog, int id)
        {
            Blog existBlog = _context.Blogs.FirstOrDefault(s => s.Id == blog.Id);
            if (existBlog == null) return NotFound();
            if (!ModelState.IsValid) return View();
            if (blog.ImageFile != null)
            {
                if (!blog.ImageFile.IsImage())
                {
                    ModelState.AddModelError("ImageFile", "Please insert a valid image type such as jpg,png,jpeg etc");
                    return View(existBlog);
                }
                if (!blog.ImageFile.IsSizeOkay(2))
                {
                    ModelState.AddModelError("ImageFile", "Image size can not be more than 2MB");
                    return View(existBlog);
                }
                Helpers.Helper.DeleteImg(_env.WebRootPath, "/assets/img/blog", existBlog.Image);
                existBlog.Image = blog.ImageFile.SaveImg(_env.WebRootPath, "assets/img/blog");
            }
            if (blog.Title==null)
            {
                ModelState.AddModelError("Title", "Please enter a title");
                return View(existBlog);
            }
            existBlog.Title = blog.Title;
            existBlog.Description = blog.Description;
            existBlog.Date = blog.Date;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Delete(int id)
        {
            Blog blog = _context.Blogs.FirstOrDefault(s => s.Id == id);
            Blog existBlog = _context.Blogs.FirstOrDefault(s => s.Id == blog.Id);
            if (existBlog == null) return NotFound();
            if (blog == null) return Json(new { status = 404 });
            _context.Blogs.Remove(blog);
            _context.SaveChanges();
            return Json(new { status = 200 });
        }
    }
}

