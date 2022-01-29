using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEndProject.Models
{
    public class TeacherFaculty
    {
        public int Id { get; set; }
        public int TeacherId { get; set; }
        public int FacultyId { get; set; }
        public Teacher Teacher { get; set; }
        public Faculty Faculty { get; set; }
    }
}
