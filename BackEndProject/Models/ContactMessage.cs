using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BackEndProject.Models
{
    public class ContactMessage
    {
        public int Id { get; set; }
        [Required]
        [StringLength(maximumLength: 500)]
        public string Message { get; set; }
        public string Email { get; set; }
        public DateTime SendDate { get; set; }
    }
}
