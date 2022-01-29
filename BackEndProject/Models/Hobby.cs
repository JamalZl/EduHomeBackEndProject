using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BackEndProject.Models
{
    public class Hobby
    {
        public int Id { get; set; }
        [StringLength(maximumLength:100)]
        public string Name { get; set; }
        public List<TeacherHobby> TeacherHobbies { get; set; }
    }
}
