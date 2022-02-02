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
    public class CompanyController : Controller
    {
        private readonly AppDbContext _context;
        private IWebHostEnvironment _env;
        public CompanyController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(int page = 1)
        {
            ViewBag.TotalPage = Math.Ceiling((decimal)_context.Companies.Count() / 4);
            ViewBag.CurrentPage = page;
            List<Company> companies = _context.Companies.Include(c=>c.Speakers).Skip((page - 1) * 4).Take(4).ToList();
            return View(companies);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Company company)
        {
            if (!ModelState.IsValid) return View();
            if (company.Name == null)
            {
                ModelState.AddModelError("Name", "Please enter Company name");
                return View();
            }
            List<Company> companies = _context.Companies.Where(c => c.Name == company.Name).ToList();
            foreach (Company item in companies)
            {
                if (item.Name.ToLower().Trim() == company.Name.ToLower().Trim())
                {
                    ModelState.AddModelError("Name", "Company is exist in database.Please enter different company name");
                    return View();
                }
            }
            _context.Companies.Add(company);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Edit(int id)
        {
            Company company = _context.Companies.FirstOrDefault(c => c.Id == id);
            if (company == null) return NotFound();
            return View(company);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Company company, int id)
        {
            if (company.Name == null)
            {
                ModelState.AddModelError("Name", "Enter a company name");
                return View(company);
            }
            Company nameControl = _context.Companies.FirstOrDefault(t => t.Name.ToLower().Trim() == company.Name.ToLower().Trim());
            if (!ModelState.IsValid) return View();
            Company existCompany = _context.Companies.FirstOrDefault(c => c.Id == company.Id);
            if (existCompany == null) return NotFound();
            if (nameControl != null && nameControl.Id != id)
            {
                ModelState.AddModelError("Name", "Company is exist in database.Please enter different company name");
                return View(existCompany);
            }
            existCompany.Name = company.Name;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Delete(int id)
        {
            Company company = _context.Companies.FirstOrDefault(c => c.Id == id);
            Company existCompany = _context.Companies.FirstOrDefault(c => c.Id == company.Id);
            if (existCompany == null) return NotFound();
            if (company == null) return Json(new { status = 404 });
            _context.Companies.Remove(company);
            _context.SaveChanges();
            return Json(new { status = 200 });
        }
    }
}
