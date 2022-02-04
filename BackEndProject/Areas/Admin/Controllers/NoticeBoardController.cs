using BackEndProject.DAL;
using BackEndProject.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEndProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class NoticeBoardController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public NoticeBoardController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(int page=1)
        {
            ViewBag.TotalPage = Math.Ceiling((decimal)_context.NoticeBoards.Count() / 4);
            ViewBag.CurrentPage = page;
            List<NoticeBoard> noticeBoards = _context.NoticeBoards.Skip((page - 1) * 4).Take(4).ToList();
            return View(noticeBoards);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(NoticeBoard noticeBoard)
        {
            if (!ModelState.IsValid) return View();
            if (noticeBoard.Description==null)
            {
                ModelState.AddModelError("Description", "Please enter description");
            }
            _context.NoticeBoards.Add(noticeBoard);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Edit(int id)
        {
            NoticeBoard noticeBoard = _context.NoticeBoards.FirstOrDefault(c => c.Id == id);
            if (noticeBoard == null) return NotFound();
            return View(noticeBoard);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(NoticeBoard notice)
        {
            if (notice.Description == null)
            {
                ModelState.AddModelError("Name", "Enter a notice board description");
                return View(notice);
            }
           
            if (!ModelState.IsValid) return View();
            NoticeBoard existNotice = _context.NoticeBoards.FirstOrDefault(c => c.Id == notice.Id);
            if (existNotice == null) return NotFound();

            existNotice.Description = notice.Description;
            existNotice.Date = notice.Date;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public  IActionResult Delete(int id)
        {
            NoticeBoard notice = _context.NoticeBoards.FirstOrDefault(nb => nb.Id == id);
            NoticeBoard existNotice = _context.NoticeBoards.FirstOrDefault(nb => nb.Id == notice.Id);
            if (existNotice == null) return NotFound();
            if (notice == null) return Json(new { status = 404 });
            _context.NoticeBoards.Remove(notice);
            _context.SaveChanges();
            return Json(new { status = 200 });
        }
    }
}
