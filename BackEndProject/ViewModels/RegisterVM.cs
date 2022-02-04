using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BackEndProject.ViewModels
{
    public class RegisterVM
    {
        [Required]
        [StringLength(maximumLength:30)]
        public string Username { get; set; }
        [Required]
        [StringLength(maximumLength:70)]
        public string Email { get; set; }
        [Required]
        [StringLength(maximumLength:30)]
        public string Name { get; set; }
        [Required]
        [StringLength(maximumLength:100)]
        public string Country { get; set; }
        [Required]
        [StringLength(maximumLength:30)]
        public string Surname { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
        public bool Terms { get; set; }
    }
}
