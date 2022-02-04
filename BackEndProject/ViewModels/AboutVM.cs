using BackEndProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEndProject.ViewModels
{
    public class AboutVM
    {
        public Setting Setting { get; set; }
        public List<NoticeBoard> NoticeBoards { get; set; }
        public List<Teacher> Teachers { get; set; }
    }
}
