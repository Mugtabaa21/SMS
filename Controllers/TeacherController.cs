using mewo.Dtos;
using mewo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace mewo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TeacherController : ControllerBase
    {
        private readonly mewo.Models.AppDbContext _context;
        public TeacherController(mewo.Models.AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTeacher()
        {
            var result = await _context.Teachers.ToListAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async  Task<IActionResult> GetTeacherById(Guid id)
        {
           
             var teacher = await _context.Teachers.FindAsync(id);
             if (teacher == null)
             return NotFound();
             return Ok(teacher);

        }

        [HttpPost]
        public async Task<IActionResult> CreateTeacher(CreateTeacherDto createTeacher)
        {
            var teacher = new Teacher
            {
                FullName = createTeacher.FullName,
                Department = createTeacher.Department,
                OfficeNumber = createTeacher.OfficeNumber,
                Title = createTeacher.Title
            };
            _context.Teachers.Add(teacher);
            await _context.SaveChangesAsync();
            return Ok(teacher);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTeacher(Guid id, UpdateTeacherDto updateTeacher)
        {
            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher == null)
            {
                return NotFound();
            }
            teacher.FullName = updateTeacher.FullName ?? teacher.FullName;
            teacher.Department = updateTeacher.Department ?? teacher.Department;
            teacher.OfficeNumber = updateTeacher.OfficeNumber ?? teacher.OfficeNumber;
            teacher.Title = updateTeacher.Title ?? teacher.Title;
            await _context.SaveChangesAsync();
            return Ok(teacher);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeacher(Guid id)
        {
            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher == null)
            {
                return NotFound();
            }
            _context.Teachers.Remove(teacher);
            await _context.SaveChangesAsync();
            return Ok(teacher);
        }
    }
}
