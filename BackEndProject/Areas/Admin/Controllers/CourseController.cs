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
    public class CourseController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public CourseController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(int page=1)
        {
            ViewBag.TotalPage = Math.Ceiling((decimal)_context.Courses.Count() / 3);
            ViewBag.CurrentPage = page;
            List<Course> courses = _context.Courses.Include(c=>c.Category).Skip((page-1)*3).Take(3).ToList();
            return View(courses);
        }
        public IActionResult Create()
        {
            ViewBag.Tags = _context.Tags.ToList();
            ViewBag.Categories = _context.Categories.ToList();
            Course course = _context.Courses.Include(c => c.CourseFeatures).FirstOrDefault();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Course course)
        {
            ViewBag.Tags = _context.Tags.ToList();
            ViewBag.Categories = _context.Categories.ToList();
            if (!ModelState.IsValid) return View();
            course.CourseTags = new List<CourseTags>();
            if (course.Name==null)
            {
                ModelState.AddModelError("Name", "Please insert a name");
                return View();
            }
            if (course.CategoryId == 0)
            {
                ModelState.AddModelError("CategoryId", "Please select one category");
                return View();
            }
            if (course.TagIds==null)
            {
                ModelState.AddModelError("TagIds", "Please select at least  one tag");
                return View();
            }
            else
            {
                foreach (var id in course.TagIds)
                {
                    CourseTags courseTags = new CourseTags
                    {
                        Course = course,
                        CourseId = id,
                        TagId = id

                    };
                    course.CourseTags.Add(courseTags);
                }
            }
            if (course.ImageFormFile == null)
            {
                ModelState.AddModelError("ImageFormFile", "Please enter an image");
                return View();
            }
            else
            {
                if (!course.ImageFormFile.IsImage())
                {
                    ModelState.AddModelError("ImageFile", "Please insert a valid image type such as jpg,png,jpeg etc");
                    return View();
                }
                if (!course.ImageFormFile.IsSizeOkay(2))
                {
                    ModelState.AddModelError("ImageFile", "Image size can not be more than 2MB");
                    return View();
                }
                course.Image = course.ImageFormFile.SaveImg(_env.WebRootPath, "assets/img/course");
            }
            _context.Courses.Add(course);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Edit(int id)
        {
            ViewBag.Tags = _context.Tags.ToList();
            ViewBag.Categories = _context.Categories.ToList();
            Course course = _context.Courses.Include(x=>x.Category).Include(x=>x.CourseTags).ThenInclude(x=>x.Tag).Include(c=>c.CourseFeatures).FirstOrDefault(c => c.Id == id);
            if (course == null) NotFound();
            return View(course);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public  IActionResult Edit(Course course)
        {
            ViewBag.Tags = _context.Tags.ToList();
            ViewBag.Categories = _context.Categories.ToList();
            Course existedCourse = _context.Courses.Include(t => t.Category).Include(t => t.CourseTags).ThenInclude(ct => ct.Tag).Include(t=>t.CourseFeatures).FirstOrDefault(t => t.Id == course.Id);

            if (!ModelState.IsValid) return View(existedCourse);
            if (existedCourse == null) return NotFound();
            if (course.Name == null)
            {
                ModelState.AddModelError("Name", "Please insert a name");
                return View();
            }
            if (course.ImageFormFile!=null)
            {
                if (!course.ImageFormFile.IsImage())
                {
                    ModelState.AddModelError("ImageFile", "Please insert a valid image type such as jpg,png,jpeg etc");
                    return View(existedCourse);
                }
                if (!course.ImageFormFile.IsSizeOkay(2))
                {
                    ModelState.AddModelError("ImageFile", "Image size can not be more than 2MB");
                    return View(existedCourse);
                }
                Helpers.Helper.DeleteImg(_env.WebRootPath, "/assets/img/course", existedCourse.Image);
                existedCourse.Image = course.ImageFormFile.SaveImg(_env.WebRootPath, "assets/img/course");
            }
            if (course.CategoryId==0)
            {
                ModelState.AddModelError("CategoryId", "Please select at least one category");
                return View(existedCourse);
            }

            if (course.TagIds!=null)
            {
                List<CourseTags> removeTags = existedCourse.CourseTags.Where(ct => !course.TagIds.Contains(ct.Id)).ToList();
                existedCourse.CourseTags.RemoveAll(fc => removeTags.Any(rc => fc.Id == rc.Id));
                if (removeTags == null)
                {
                    ModelState.AddModelError("TagIds", "Please select at least  one tag");
                    return View(existedCourse);
                }
                foreach (var tagId in course.TagIds )
                {
                    CourseTags courseTag = existedCourse.CourseTags.FirstOrDefault(ct => ct.TagId==tagId);
                    if (courseTag==null)
                    {
                        CourseTags cTags = new CourseTags
                        {
                            TagId = tagId,
                            CourseId = existedCourse.Id
                        };
                        existedCourse.CourseTags.Add(cTags);
                    }
                }
            }
            if (course.TagIds==null)
            {
                ModelState.AddModelError("TagIds", "Please select at least one tag");
                return View(existedCourse);
            }
            existedCourse.Description = course.Description;
            existedCourse.Name = course.Name;
            existedCourse.About = course.About;
            existedCourse.Apply = course.Apply;
            existedCourse.Certification = course.Certification;
            existedCourse.CategoryId = course.CategoryId;
            existedCourse.CourseFeatures.StartTime = course.CourseFeatures.StartTime;
            existedCourse.CourseFeatures.Duration = course.CourseFeatures.Duration;
            existedCourse.CourseFeatures.ClassDuration = course.CourseFeatures.ClassDuration;
            existedCourse.CourseFeatures.SkillLevel = course.CourseFeatures.SkillLevel;
            existedCourse.CourseFeatures.Language = course.CourseFeatures.Language;
            existedCourse.CourseFeatures.StudentCount = course.CourseFeatures.StudentCount;
            existedCourse.CourseFeatures.CourseFee = course.CourseFeatures.CourseFee;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Delete(int id)
        {
            Course course = _context.Courses.FirstOrDefault(c => c.Id == id);
            Course existCourse = _context.Courses.FirstOrDefault(c => c.Id == course.Id);
            if (existCourse == null) return NotFound();
            if (course == null) return Json(new { status = 404 });
            _context.Courses.Remove(course);
            _context.SaveChanges();
            return Json(new { status = 200 });
        }
        public IActionResult Comments(int CourseId)
        {
            if (!_context.Comments.Any(c => c.CourseId == CourseId)) return RedirectToAction("index", "course");
            List<Comment> comments = _context.Comments.Include(c => c.AppUser).Where(c => c.CourseId == CourseId).ToList();
            return View(comments);
        }
        public IActionResult CStatusChange(int id)
        {
            if (!_context.Comments.Any(c => c.Id == id)) return RedirectToAction("Index", "Course");
            Comment comment = _context.Comments.SingleOrDefault(c => c.Id == id);
            comment.IsAccess = comment.IsAccess ? false : true;
            _context.SaveChanges();
            return RedirectToAction("Comments", "Course", new { CourseId = comment.CourseId });

        }
    }
}
