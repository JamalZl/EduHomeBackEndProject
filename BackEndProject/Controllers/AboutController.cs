using BackEndProject.DAL;
using BackEndProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEndProject.Controllers
{
    public class AboutController : Controller
    {
        private readonly AppDbContext _context;
        public AboutController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            AboutVM aboutVM = new AboutVM
            {
                Setting = _context.Settings.FirstOrDefault(),
                NoticeBoards = _context.NoticeBoards.ToList(),
                Teachers = _context.Teachers.Include(t=>t.Position).Take(4).ToList()
            };
            return View(aboutVM);
        }
    }
}
