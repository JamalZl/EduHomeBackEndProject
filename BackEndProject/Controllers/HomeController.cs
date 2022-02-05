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
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            HomeVM homeVM = new HomeVM
            {
                Setting = _context.Settings.FirstOrDefault(),
                NoticeBoards = _context.NoticeBoards.ToList(),
                Sliders = _context.Sliders.ToList(),
                Courses = _context.Courses.Take(3).Include(c => c.Category).ToList(),
                Events = _context.Events.Take(4).ToList(),
                Blogs = _context.Blogs.Include(b => b.Comments).Take(3).ToList()
            };
            return View(homeVM);
        }
        [HttpGet]
        public IActionResult Search(string keyword)
        {
            //HomeVM homeVM = new HomeVM
            //{
            //    Courses = _context.Courses.Take(3).Include(c => c.Category).ToList(),
            //    Events = _context.Events.Take(4).ToList(),
            //    Blogs = _context.Blogs.Take(3).ToList()
            //};
            
                HomeVM homeV = new HomeVM
                {
                    Courses = _context.Courses.Include(c => c.Category).Where(f => f.Name.ToLower().Trim().Contains(keyword.ToLower().Trim())).ToList(),
                    Events = _context.Events.ToList().Where(f => f.Name.ToLower().Trim().Contains(keyword.ToLower().Trim())).ToList(),
                    Blogs = _context.Blogs.Include(b => b.Comments).ToList().Where(f => f.Title.ToLower().Trim().Contains(keyword.ToLower().Trim())).ToList()
                };
                return View(homeV);
            

        }

        //public IActionResult Search()
        //{
        //    return View();
        //}
        //[HttpGet]
        //public async Task<IActionResult> Search (string keyword)
        //{
        //    HomeVM homeVM = new HomeVM
        //   {
        //       Courses = _context.Courses.Take(3).Include(c => c.Category).ToList(),
        //       Events = _context.Events.Take(4).ToList(),
        //       Blogs = _context.Blogs.Take(3).ToList()
        //   };

        //    if (!string.IsNullOrEmpty(keyword))
        //    {
        //        ViewData["Getallsearch"] = keyword;
        //        HomeVM homeV = new HomeVM
        //        {
        //            Courses = await _context.Courses.Include(c => c.Category).Where(f => f.Name.Contains(keyword)).AsNoTracking().ToListAsync(),
        //            Events = await _context.Events.Where(f => f.Name.Contains(keyword)).AsNoTracking().ToListAsync(),
        //            Blogs = await _context.Blogs.Where(f => f.Title.Contains(keyword)).AsNoTracking().ToListAsync()
        //        };
        //        return View(homeV);
        //    }
        //    return View(homeVM);

        //}
    }
}
