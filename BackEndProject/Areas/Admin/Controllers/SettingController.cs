using BackEndProject.DAL;
using BackEndProject.Extensions;
using BackEndProject.Models;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class SettingController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public SettingController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index()
        {
            Setting setting = _context.Settings.Include(s=>s.FooterSocials).FirstOrDefault();
            return View(setting);
        }
        public IActionResult Edit()
        {
            Setting setting = _context.Settings.Include(s => s.FooterSocials).FirstOrDefault();
            return View(setting);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Setting settingModel)
        {
            Setting setting = _context.Settings.Include(s => s.FooterSocials).FirstOrDefault(s => s.Id == settingModel.Id);
            if (!ModelState.IsValid) return NotFound();
            if (settingModel.ImageLogoFile==null)
            {
                ModelState.AddModelError("ImageLogoFile", "Please enter Logo");
            }
            if (settingModel.ImageAboutFile == null)
            {
                ModelState.AddModelError("AboutImage", "Please enter about Image");
            }
            if (settingModel.SearchIcon == null)
            {
                ModelState.AddModelError("SearchIcon", "Please enter Search Icon");
            }
            if (settingModel.AboutDescription == null)
            {
                ModelState.AddModelError("AboutDescription", "Please enter about Description");
            }
            if (settingModel.AboutTitle == null)
            {
                ModelState.AddModelError("AboutTitle", "Please enter about Title");
            }
            if (settingModel.AboutViewUrl==null)
            {
                ModelState.AddModelError("AboutViewUrl", "Please enter  about View");
            }
            if (settingModel.FooterDescription == null)
            {
                ModelState.AddModelError("FooterDescription", "Please enter footer Description ");
            }
            if (settingModel.ImageLogoFile != null && settingModel.ImageAboutFile !=null )
            {
                if (!settingModel.ImageLogoFile.IsSizeOkay(2))
                {
                    ModelState.AddModelError("ImageLogoFile", "Image size can not be more than 2MB");
                    return View();
                }
                if (!settingModel.ImageLogoFile.IsImage())
                {
                    ModelState.AddModelError("ImageLogoFile", "Please insert a valid image type such as jpg,png,jpeg etc");
                    return View();
                }
                if (!settingModel.ImageAboutFile.IsImage())
                {
                    ModelState.AddModelError("ImageAboutFile", "Please insert a valid image type such as jpg,png,jpeg etc");
                    return View();
                }
                if (!settingModel.ImageAboutFile.IsSizeOkay(2))
                {
                    ModelState.AddModelError("ImageAboutFile", "Image size can not be more than 2MB");
                    return View();
                }
                Helpers.Helper.DeleteImg(_env.WebRootPath, "assets/img/logo", setting.SiteLogo);
                Helpers.Helper.DeleteImg(_env.WebRootPath, "assets/img/logo", setting.AboutImage);
                setting.SiteLogo = settingModel.ImageLogoFile.SaveImg(_env.WebRootPath, "assets/img/logo");
                setting.AboutImage = settingModel.ImageAboutFile.SaveImg(_env.WebRootPath, "assets/img/logo");
               
            }
            if (settingModel.ImageLogoFile == null && settingModel.ImageAboutFile != null)
            {
                
                if (!settingModel.ImageAboutFile.IsImage())
                {
                    ModelState.AddModelError("ImageAboutFile", "Please insert a valid image type such as jpg,png,jpeg etc");
                    return View();
                }
                if (!settingModel.ImageAboutFile.IsSizeOkay(2))
                {
                    ModelState.AddModelError("ImageAboutFile", "Image size can not be more than 2MB");
                    return View();
                }
                Helpers.Helper.DeleteImg(_env.WebRootPath, "assets/img/logo", setting.AboutImage);
               
                setting.AboutImage = settingModel.ImageAboutFile.SaveImg(_env.WebRootPath, "assets/img/logo");

            }
            if (settingModel.ImageLogoFile != null && settingModel.ImageAboutFile == null)
            {
                if (!settingModel.ImageLogoFile.IsImage())
                {
                    ModelState.AddModelError("ImageLogoFile", "Please insert a valid image type such as jpg,png,jpeg etc");
                    return View();
                }
                if (!settingModel.ImageLogoFile.IsSizeOkay(2))
                {
                    ModelState.AddModelError("ImageLogoFile", "Image size can not be more than 2MB");
                    return View();
                }
                Helpers.Helper.DeleteImg(_env.WebRootPath, "assets/img/logo", setting.SiteLogo);
                setting.SiteLogo = settingModel.ImageLogoFile.SaveImg(_env.WebRootPath, "assets/img/logo");

            }

            setting.AboutTitle = settingModel.AboutTitle;
            setting.AboutDescription = settingModel.AboutDescription;
            setting.FooterDescription = settingModel.FooterDescription;
            setting.Address  = settingModel.Address;
            setting.FooterWebsite = settingModel.FooterWebsite;
            setting.FooterMail = settingModel.FooterMail;
            setting.FooterNumber1 = settingModel.FooterNumber1;
            setting.FooterNumber2 = settingModel.FooterNumber2;
            setting.HeaderNumber = settingModel.HeaderNumber;
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
