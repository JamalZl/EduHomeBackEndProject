using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BackEndProject.Models
{
    public class Event
    {
        public int Id { get; set; }
        [StringLength(maximumLength:100)]
        public string Name { get; set; }
        [StringLength(maximumLength: 100)]
        public string Image { get; set; }
        [StringLength(maximumLength: 500)]
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime Date { get; set; }
        public string Venue { get; set; }
        public List<EventSpeaker> EventSpeakers { get; set; }
        [NotMapped]
        public List<int> SpeakerIds { get; set; }
        public List<Comment> Comments { get; set; }

        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }

}
