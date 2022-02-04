using BackEndProject.DAL;
using BackEndProject.Extensions;
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
    public class SliderController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public SliderController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(int page = 1)
        {
            ViewBag.TotalPage = Math.Ceiling((decimal)_context.Sliders.Count() / 2);
            ViewBag.CurrentPage = page;
            List<HeaderSlider> sliders = _context.Sliders.Skip((page - 1) * 2).Take(2).ToList();
            return View(sliders);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(HeaderSlider slider)
        {
            if (!ModelState.IsValid) return View();
            List<HeaderSlider> OrderList = _context.Sliders.Where(s => s.Order == slider.Order).ToList();
            if (slider.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "Please insert an image");
                return View();
            }
            if (!slider.ImageFile.IsSizeOkay(2))
            {
                ModelState.AddModelError("ImageFile", "Image size can not be more than 2MB");
                return View();
            }
            if (!slider.ImageFile.IsImage())
            {
                ModelState.AddModelError("ImageFile", "Please insert a valid image type such as jpg,png,jpeg etc");
                return View();
            }
            foreach (HeaderSlider item in OrderList)
            {
                if (item.Order == slider.Order)
                {
                    ModelState.AddModelError("Order", "Order number is taken.Please enter different order");
                    return View();
                }
            }
            slider.Image = slider.ImageFile.SaveImg(_env.WebRootPath, "assets/img/slider");
            _context.Sliders.Add(slider);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            HeaderSlider slider = _context.Sliders.FirstOrDefault(s => s.Id == id);
            if (slider == null) return NotFound();
            return View(slider);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(HeaderSlider slider, int id)
        {
            HeaderSlider existSlider = _context.Sliders.FirstOrDefault(s => s.Id == slider.Id);
            HeaderSlider Order = _context.Sliders.Where(s => s.Order == slider.Order).FirstOrDefault();
            if (existSlider == null) return NotFound();
            if (!ModelState.IsValid) return View();
            if (slider.ImageFile != null)
            {
                if (!slider.ImageFile.IsImage())
                {
                    ModelState.AddModelError("ImageFile", "Please insert a valid image type such as jpg,png,jpeg etc");
                    return View(existSlider);
                }
                if (!slider.ImageFile.IsSizeOkay(2))
                {
                    ModelState.AddModelError("ImageFile", "Image size can not be more than 2MB");
                    return View(existSlider);
                }
                Helpers.Helper.DeleteImg(_env.WebRootPath, "/assets/img/slider", existSlider.Image);
                existSlider.Image = slider.ImageFile.SaveImg(_env.WebRootPath, "assets/img/slider");
            }
            if (Order.Id!=id)
            {
                ModelState.AddModelError("Order", "Order number is taken.Please enter different order");
                return View(existSlider);
            }
            existSlider.Title = slider.Title;
            existSlider.Description = slider.Description;
            existSlider.Order = slider.Order;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Delete(int id)
        {
            HeaderSlider slider = _context.Sliders.FirstOrDefault(s => s.Id == id);
            HeaderSlider existSlider = _context.Sliders.FirstOrDefault(s => s.Id == slider.Id);
            if (existSlider == null) return NotFound();
            if (slider == null) return Json(new { status = 400 });
            _context.Sliders.Remove(slider);
            _context.SaveChanges();
            return Json(new { status = 200 });
        }
    }
}

