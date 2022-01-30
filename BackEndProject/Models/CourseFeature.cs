using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BackEndProject.Models
{
    public class CourseFeature
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        [StringLength(maximumLength:50)]
        public string Duration { get; set; }
        //[StringLength(maximumLength: 50)]
        //public string ClassDuration { get; set; }
        [StringLength(maximumLength:50)]
        public string SkillLevel { get; set; }
        [StringLength(maximumLength:50)]
        public string Language { get; set; }
        public int StudentCount { get; set; }
        public double CourseFee { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
    }
}
