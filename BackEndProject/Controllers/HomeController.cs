﻿using BackEndProject.DAL;
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
                Courses = _context.Courses.Include(c => c.Category).ToList(),
                Events = _context.Events.ToList(),
                Blogs = _context.Blogs.ToList()
            };
            return View(homeVM);
        }
    }
}
