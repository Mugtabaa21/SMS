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
    public class StudentsController : ControllerBase
    {
        private readonly mewo.Models.AppDbContext _context;
        public StudentsController(mewo.Models.AppDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllStudent()
        {
            var result = await _context.Students.ToListAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudentById(Guid id)
        {
            var student = await _context.Students.FindAsync(id);

            if (student == null)
                return NotFound();

            return Ok(student);
        }


        [HttpPost]
        public async Task<IActionResult> CreateStudent(CreateStudentDto createStudent)
        {
            var student = new Student
            {
                FullName = createStudent.FullName,
                College = createStudent.College,
                Department = createStudent.Department,
                Stage = createStudent.Stage,
                ImageUrl = createStudent.ImageUrl,
                EnrollmentDate = DateTime.UtcNow
            };
            _context.Students.Add(student);
            await _context.SaveChangesAsync();
            return Ok(student);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudent(Guid id, UpdateStudentDto updateStudent)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            student.FullName = updateStudent.FullName ?? student.FullName;
            student.College = updateStudent.College ?? student.College;
            student.Department = updateStudent.Department ?? student.Department;
            student.Stage = updateStudent.Stage ?? student.Stage;
            student.ImageUrl = updateStudent.ImageUrl ?? student.ImageUrl;
            student.EnrollmentDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return Ok(student);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(Guid id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return Ok(student);
        }

    }
}
