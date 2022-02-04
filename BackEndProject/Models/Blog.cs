using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BackEndProject.Models
{
    public class Blog
    {
        public int Id { get; set; }
        [StringLength(maximumLength:120)]
        public string Title { get; set; }
        [StringLength(maximumLength:520)]
        public string Description { get; set; }
        [StringLength(maximumLength:200)]
        public string Image { get; set; }
        public DateTime Date { get; set; }
        public List<Comment> Comments { get; set; }

        [NotMapped]
        public IFormFile ImageFile { get; set; }

    }
}
