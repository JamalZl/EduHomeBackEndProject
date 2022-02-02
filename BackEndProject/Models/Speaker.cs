using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BackEndProject.Models
{
    public class Speaker
    {
        public int Id { get; set; }
        [StringLength(maximumLength:70)]
        public string Name { get; set; }
        public string Image { get; set; }
        public List<EventSpeaker> EventSpeakers { get; set; }
        public int PositionId { get; set; }
        public Position Position { get; set; }
        public int CompanyId { get; set; }
        public Company Company { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}
