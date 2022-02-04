using BackEndProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEndProject.ViewModels
{
    public class CourseVM
    {
        public List<Course> Courses { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
