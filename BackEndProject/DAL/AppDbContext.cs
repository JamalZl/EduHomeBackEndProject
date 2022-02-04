using BackEndProject.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEndProject.DAL
{
    public class AppDbContext:IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {

        }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<FooterSocial> FooterSocials { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseFeature> CourseFeatures { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventSpeaker> EventSpeakers { get; set; }
        public DbSet<Faculty> Faculties { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<TeacherSkill> TeacherSkills { get; set; }
        public DbSet<TeacherFaculty> TeacherFaculties { get; set; }
        public DbSet<Hobby> Hobbies { get; set; }
        public DbSet<TeacherHobby> TeacherHobbies { get; set; }
        public DbSet<Social> Socials { get; set; }
        public DbSet<NoticeBoard> NoticeBoards { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<HeaderSlider> Sliders { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<CourseTags> CourseTags { get; set; }
        public DbSet<Speaker> Speakers { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Comment> Comments { get; set; }
    }
}
