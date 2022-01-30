using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BackEndProject.Models
{
    public class HeaderSlider
    {
        public int Id { get; set; }
        [StringLength(maximumLength:100)]
        [Required]
        public string Title { get; set; }
        [StringLength(maximumLength:500)]
        public string Description { get; set; }
        [StringLength(maximumLength:200)]
        public string Image { get; set; }
        public int Order { get; set; }

        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}
