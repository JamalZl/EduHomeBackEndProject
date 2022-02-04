using BackEndProject.DAL;
using BackEndProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEndProject.Controllers
{
    public class TeacherController : Controller
    {
        private readonly AppDbContext _context;
        public TeacherController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int page=1)
        {
            ViewBag.TotalPage = Math.Ceiling((decimal)_context.Teachers.Count() / 8);
            ViewBag.CurrentPage = page;
            List<Teacher> teacherModel = _context.Teachers.Include(t => t.TeacherFaculties).ThenInclude(tf => tf.Faculty).Include(t => t.TeacherHobbies).ThenInclude(th => th.Hobby).Include(t => t.Position).Skip((page-1)*8).Take(8).ToList();
            return View(teacherModel);
        }
        public IActionResult Details(int id)
        {
            ViewBag.TeacherHobbies = _context.TeacherHobbies.Include(th=>th.Teacher).Include(th=>th.Hobby).Where(th=>th.TeacherId==id).ToList();
            ViewBag.TeacherFaculties = _context.TeacherFaculties.Include(tf=>tf.Faculty).Include(tf=>tf.Teacher).Where(tf=>tf.TeacherId==id).ToList();
            Teacher teacher = _context.Teachers.Include(t => t.TeacherFaculties).ThenInclude(tf => tf.Faculty).Include(t => t.TeacherHobbies).ThenInclude(th => th.Hobby).Include(t => t.Position).FirstOrDefault(t=>t.Id==id);
            if (teacher == null) return NotFound();
            return View(teacher);
        }
    }
}
