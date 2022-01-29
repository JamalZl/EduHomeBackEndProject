using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BackEndProject.Models
{
    public class Speaker
    {
        public int Id { get; set; }
        [StringLength(maximumLength:70)]
        public string Name { get; set; }
        public List<EventSpeaker> EventSpeakers { get; set; }
        public string PositionId { get; set; }
        public Position Position { get; set; }
        public string CompanyId { get; set; }
        public Company Company { get; set; }
    }
}
