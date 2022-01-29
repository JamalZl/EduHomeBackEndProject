using BackEndProject.DAL;
using BackEndProject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEndProject.Services
{
    public class LayoutServices
    {
        private readonly AppDbContext _context;
        public LayoutServices(AppDbContext context)
        {
            _context = context;
        }
        public Setting GetSettingsData()
        {
            Setting data = _context.Settings.Include(s=>s.FooterSocials).FirstOrDefault();
            return data;
        }
    }
}
