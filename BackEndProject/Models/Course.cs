using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BackEndProject.Models
{
    public class Course
    {
        public int Id { get; set; }
        [StringLength(maximumLength:50)]
        public string Name { get; set; }
        [StringLength(maximumLength: 500)]

        public string Description { get; set; }
        [StringLength(maximumLength: 50)]
        public string Image { get; set; }
        [StringLength(maximumLength: 450)]

        public string About { get; set; }
        [StringLength(maximumLength: 450)]

        public string Apply { get; set; }
        [StringLength(maximumLength: 450)]

        public string Certification { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public List<CourseFeature> CourseFeatures { get; set; }
        [NotMapped]
        public List<int> TagIds { get; set; }
        public List<CourseTags> CourseTags { get; set; }
        [NotMapped]
        public IFormFile ImageFormFile { get; set; }
    }
}
