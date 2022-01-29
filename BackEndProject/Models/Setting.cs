using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BackEndProject.Models
{
    public class Setting
    {
        public int Id { get; set; }
        [StringLength(maximumLength:50)]
        public string SiteLogo { get; set; }
        [StringLength(maximumLength: 50)]

        public string HeaderNumber { get; set; }
        [StringLength(maximumLength: 50)]

        public string SearchIcon { get; set; }
        [StringLength(maximumLength: 500)]

        public string FooterDescription { get; set; }
        [StringLength(maximumLength: 50)]

        public string Address { get; set; }
        [StringLength(maximumLength: 50)]
        public string FooterNumber1 { get; set; }
        [StringLength(maximumLength:50)]
        public string FooterNumber2 { get; set; }
        [StringLength(maximumLength:50)]
        public string FooterMail { get; set; }
        [StringLength(maximumLength:50)]
        public string FooterWebsite { get; set; }
        [StringLength(maximumLength:200)]
        public string FooterHasTechUrl { get; set; }
        [StringLength(maximumLength:100)]
        public string AboutTitle { get; set; }
        [StringLength(maximumLength:500)]
        public string AboutDescription { get; set; }
        [StringLength(maximumLength:80)]
        public string AboutImage { get; set; }
        [StringLength(maximumLength:200)]
        public string AboutViewUrl { get; set; }
        [StringLength(maximumLength: 200)]
        public string VideoTourUrl { get; set; }

        public List<FooterSocial> FooterSocials { get; set; }
    }
}
