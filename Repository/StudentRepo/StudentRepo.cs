using mewo.Models;
using Microsoft.EntityFrameworkCore;

namespace mewo.Repository.StudentRepo
{
    public class StudentRepo : IStudentRepo
    {
        private readonly AppDbContext _context;
        public StudentRepo(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Student> CreateStudent(Student student)
        {
            await _context.Students.AddAsync(student);
            await _context.SaveChangesAsync();
            return student;
        }

        public async Task DeleteStudent(Guid id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student != null)
            {
                _context.Students.Remove(student);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Student>> GetAllStudentDefualt()
        {
            return await _context.Students.ToListAsync();
        }

        public async Task<Student> GetStudentById(Guid id)
        {
            return await _context.Students.FindAsync(id);
        }

        public async Task UpdateStudent(Student student)
        {
            _context.Students.Update(student);
            await _context.SaveChangesAsync();
        }
    }
}
