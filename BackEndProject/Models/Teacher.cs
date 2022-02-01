using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BackEndProject.Models
{
    public class Teacher
    {
        public int Id { get; set; }
        [StringLength(maximumLength: 50)]
        public string Name { get; set; }
        [StringLength(maximumLength:50)]
        public string Surname { get; set; }
        [StringLength(maximumLength:500)]
        public string About { get; set; }
        [StringLength(maximumLength:150)]
        public string Image { get; set; }
        [StringLength(maximumLength:100)]
        public string Degree { get; set; }
        [StringLength(maximumLength:90)]
        public string Experience { get; set; }
        [StringLength(maximumLength:60)]
        public string Mail { get; set; }
        [StringLength(maximumLength:50)]
        public string PhoneNumber { get; set; }
        public string FacebookUrl { get; set; }
        public string PinterestUrl { get; set; }
        public string TwitterUrl { get; set; }
        public string VimeoUrl { get; set; }
        //public List<TeacherSkill> TeacherSkills { get; set; }
        public List<TeacherFaculty> TeacherFaculties { get; set; }
        public List<TeacherHobby> TeacherHobbies { get; set; }
        public List<Social> Socials { get; set; }
        public int PositionId { get; set; }
        public Position Position { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
        [NotMapped]
        public List<int> HobbyIds { get; set; }
        [NotMapped]
        public List<int> FacultyIds { get; set; }
    }
}
