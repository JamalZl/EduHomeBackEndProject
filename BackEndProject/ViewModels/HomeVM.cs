using BackEndProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEndProject.ViewModels
{
    public class HomeVM
    {
        public Setting Setting { get; set; }
        public List<NoticeBoard> NoticeBoards { get; set; }
        public List<HeaderSlider> Sliders { get; set; }
        public List<Course> Courses { get; set; }
        public List<Event> Events { get; set; }
        public List<Blog> Blogs { get; set; }
    }
}
