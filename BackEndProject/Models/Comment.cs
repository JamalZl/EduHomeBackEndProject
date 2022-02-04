using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BackEndProject.Models
{
    public class Comment
    {
        public int Id { get; set; }
        [Required]
        [StringLength(maximumLength: 500)]
        public string Text { get; set; }
        public bool IsAccess { get; set; }
        public DateTime CreatedTime { get; set; }
        public int? BlogId { get; set; }
        public Blog Blog { get; set; }
        public int? CourseId { get; set; }
        public Course Course { get; set; }
        public int? EventId { get; set; }
        public Event Event { get; set; }
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}
