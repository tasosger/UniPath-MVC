using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UniPath_MVC.Data;
using UniPath_MVC.Models;

namespace UniPath_MVC.Services
{
    public class ClassService
    {
        private readonly AppDbContext _context;

        public ClassService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Class?> GetClassDetailsAsync(int classId)
        {
            return await _context.Classes
                .Include(c => c.Teacher)
                .FirstOrDefaultAsync(c => c.Id == classId);
        }

        public async Task<List<Capsule>> GetCapsulesForClassAsync(int classId)
        {
            return await _context.Capsules
                .Where(c => c.ClassId == classId)
                .ToListAsync();
        }
    }
}
