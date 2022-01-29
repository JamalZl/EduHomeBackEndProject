using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BackEndProject.Models
{
    public class NoticeBoard
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        [StringLength(maximumLength:300)]
        public string Description { get; set; }
    }
}
