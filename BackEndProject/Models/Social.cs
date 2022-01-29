using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BackEndProject.Models
{
    public class Social
    {
        public int Id { get; set; }
        [StringLength(maximumLength: 50)]
        public string Logo { get; set; }
        [StringLength(maximumLength: 200)]
        public string LogoUrl { get; set; }
        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; }
    }
}
