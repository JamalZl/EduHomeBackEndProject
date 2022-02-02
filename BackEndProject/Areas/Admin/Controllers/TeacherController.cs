using BackEndProject.DAL;
using BackEndProject.Extensions;
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
    public class TeacherController : Controller
    {
        private readonly AppDbContext _context;
        private IWebHostEnvironment _env;
        public TeacherController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(int page=1)
        {
            ViewBag.TotalPage = Math.Ceiling((decimal)_context.Teachers.Count() / 3);
            ViewBag.CurrentPage = page;
            List<Teacher> teachers = _context.Teachers.Include(t=>t.Position).Skip((page - 1) * 3).Take(3).ToList();
            return View(teachers);
        }
        public IActionResult Create()
        {
            ViewBag.Hobbies = _context.Hobbies.ToList();
            ViewBag.Faculties = _context.Faculties.ToList();
            ViewBag.Positions = _context.Positions.ToList();
            Teacher teacher = _context.Teachers.FirstOrDefault();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Teacher teacher)
        {
            ViewBag.Hobbies = _context.Hobbies.ToList();
            ViewBag.Faculties = _context.Faculties.ToList();
            ViewBag.Positions = _context.Positions.ToList();
            if (!ModelState.IsValid) return View();
            teacher.TeacherFaculties = new List<TeacherFaculty>();
            teacher.TeacherHobbies = new List<TeacherHobby>();
            if (teacher.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "Please enter an image");
                return View();
            }
            if (teacher.Name==null)
            {
                ModelState.AddModelError("Name", "Please enter name");
                return View();
            }
            if (teacher.Surname == null)
            {
                ModelState.AddModelError("Name", "Please enter surname");
                return View();
            }
            else
            {
                if (!teacher.ImageFile.IsImage())
                {
                    ModelState.AddModelError("ImageFile", "Please insert a valid image type such as jpg,png,jpeg etc");
                    return View();
                }
                if (!teacher.ImageFile.IsSizeOkay(2))
                {
                    ModelState.AddModelError("ImageFile", "Image size can not be more than 2MB");
                    return View();
                }
                teacher.Image = teacher.ImageFile.SaveImg(_env.WebRootPath, "assets/img/teacher");
            }
            if (teacher.Name == null)
            {
                ModelState.AddModelError("Name", "Please enter teacher name");
            }
            if (teacher.PositionId == 0)
            {
                ModelState.AddModelError("PositionId", "Please select one position");
                return View();
            }
            
            if (teacher.FacultyIds == null)
            {
                ModelState.AddModelError("FacultyIds", "Please select at least  one faculty");
                return View();
            }
            else
            {
                foreach (var id in teacher.FacultyIds)
                {
                    TeacherFaculty teacherFaculty = new TeacherFaculty
                    {
                        Teacher = teacher,
                        FacultyId = id,
                        TeacherId = id

                    };
                    teacher.TeacherFaculties.Add(teacherFaculty);
                }
            }
            if (teacher.HobbyIds == null)
            {
                ModelState.AddModelError("HobbyIds", "Please select at least  one hobby");
                return View();
            }
            else
            {
                foreach (var id in teacher.HobbyIds)
                {
                    TeacherHobby teacherHobby = new TeacherHobby
                    {
                        Teacher = teacher,
                        HobbyId = id,
                        TeacherId = id
                    };
                    teacher.TeacherHobbies.Add(teacherHobby);
                }
            }
           
            
            _context.Teachers.Add(teacher);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Edit(int id)
        {
            ViewBag.Hobbies = _context.Hobbies.ToList();
            ViewBag.Faculties = _context.Faculties.ToList();
            ViewBag.Positions = _context.Positions.ToList();
            Teacher teacher = _context.Teachers.Include(t => t.TeacherFaculties).ThenInclude(tf => tf.Faculty).Include(t => t.TeacherHobbies).ThenInclude(th => th.Hobby).Include(t => t.Position).FirstOrDefault(t=>t.Id==id);
            if (teacher == null) return NotFound();
            return View(teacher);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Teacher teacher)
        {
            ViewBag.Hobbies = _context.Hobbies.ToList();
            ViewBag.Faculties = _context.Faculties.ToList();
            ViewBag.Positions = _context.Positions.ToList();
            Teacher existTeacher = _context.Teachers.Include(t => t.TeacherFaculties).ThenInclude(tf => tf.Faculty).Include(t => t.TeacherHobbies).ThenInclude(th => th.Hobby).Include(t => t.Position).FirstOrDefault(t => t.Id == teacher.Id);
            if (!ModelState.IsValid) return View();
            if (teacher.ImageFile!=null)
            {
                if (!teacher.ImageFile.IsImage())
                {
                    ModelState.AddModelError("ImageFile", "Please insert a valid image type such as jpg,png,jpeg etc");
                    return View(existTeacher);
                }
                if (!teacher.ImageFile.IsSizeOkay(2))
                {
                    ModelState.AddModelError("ImageFile", "Image size can not be more than 2MB");
                    return View(existTeacher);
                }
                Helpers.Helper.DeleteImg(_env.WebRootPath, "/assets/img/teacher", existTeacher.Image);
                existTeacher.Image = teacher.ImageFile.SaveImg(_env.WebRootPath, "assets/img/teacher");
            }
            if (teacher.Name == null)
            {
                ModelState.AddModelError("Name", "Please enter name");
                return View(existTeacher);
            }
            if (teacher.Surname == null)
            {
                ModelState.AddModelError("Name", "Please enter surname");
                return View(existTeacher);

            }
            if (teacher.PositionId==0)
            {
                ModelState.AddModelError("PositionId", "Please select one category");
                return View(existTeacher);
            }
            if (teacher.FacultyIds != null)
            {
                List<TeacherFaculty> removableFaculties = existTeacher.TeacherFaculties.Where(th => !teacher.FacultyIds.Contains(th.Id)).ToList();
                existTeacher.TeacherFaculties.RemoveAll(th => removableFaculties.Any(rh => th.Id == rh.Id));
                if (removableFaculties == null)
                {
                    ModelState.AddModelError("FacultyIds", "Please select at least one faculty");
                    return View(existTeacher);
                }
                foreach (var facultyId in teacher.FacultyIds)
                {
                    TeacherFaculty teacherFaculty = existTeacher.TeacherFaculties.FirstOrDefault(th => th.FacultyId == facultyId);
                    if (teacherFaculty == null)
                    {
                        TeacherFaculty tfaculty = new TeacherFaculty
                        {
                            FacultyId = facultyId,
                            TeacherId = existTeacher.Id
                        };
                        existTeacher.TeacherFaculties.Add(tfaculty);
                    }
                }
            }
            if (teacher.FacultyIds == null)
            {
                ModelState.AddModelError("FacultyIds", "Please select at least one faculty");
                return View(existTeacher);
            }
            if (teacher.HobbyIds != null)
            {
                List<TeacherHobby> removableHobbies = existTeacher.TeacherHobbies.Where(th => !teacher.HobbyIds.Contains(th.Id)).ToList();
                existTeacher.TeacherHobbies.RemoveAll(th => removableHobbies.Any(rh => th.Id == rh.Id));
                if (removableHobbies == null)
                {
                    ModelState.AddModelError("HobbyIds", "Please select at least one hobby");
                    return View(existTeacher);
                }
                foreach (var hobbyId in teacher.HobbyIds)
                {
                    TeacherHobby teacherHobby = existTeacher.TeacherHobbies.FirstOrDefault(th => th.HobbyId == hobbyId);
                    if (teacherHobby == null)
                    {
                        TeacherHobby tHobby = new TeacherHobby
                        {
                            HobbyId = hobbyId,
                            TeacherId = existTeacher.Id
                        };
                        existTeacher.TeacherHobbies.Add(tHobby);
                    }
                }
            }
            if (teacher.HobbyIds == null)
            {
                ModelState.AddModelError("HobbyIds", "Please select at least one hobby");
                return View(existTeacher);
            }
            existTeacher.Name = teacher.Name;
            existTeacher.Surname = teacher.Surname;
            existTeacher.About = teacher.About;
            existTeacher.Degree = teacher.Degree;
            existTeacher.Experience = teacher.Experience;
            existTeacher.Mail = teacher.Mail;
            existTeacher.PhoneNumber = teacher.PhoneNumber;
            existTeacher.FacebookUrl = teacher.FacebookUrl;
            existTeacher.PinterestUrl = teacher.PinterestUrl;
            existTeacher.TwitterUrl = teacher.TwitterUrl;
            existTeacher.VimeoUrl = teacher.VimeoUrl;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Delete(int id)
        {
            Teacher teacher = _context.Teachers.FirstOrDefault(c => c.Id == id);
            Teacher existTeacher = _context.Teachers.FirstOrDefault(c => c.Id == teacher.Id);
            if (existTeacher == null) return NotFound();
            if (teacher == null) return Json(new { status = 404 });
            _context.Teachers.Remove(teacher);
            _context.SaveChanges();
            return Json(new { status = 200 });
        }
    }
}
