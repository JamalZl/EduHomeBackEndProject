using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BackEndProject.Models
{
    public class FooterSocial
    {
        public int Id { get; set; }
        [Required]
        [StringLength(maximumLength: 70)]

        public string Logo { get; set; }
        [Required]
        [StringLength(maximumLength: 200)]
        public string LogoUrl { get; set; }

        public int SettingId { get; set; }
        public Setting Setting { get; set; }
    }
}
