using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mewo.Models;
using mewo.Dtos;

namespace mewo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnrollmentController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EnrollmentController(AppDbContext context)
        {
            _context = context;
        }

        
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var enrollments = await _context.Enrollments
                .Include(e => e.Student)
                .Include(e => e.Course)
                .Select(e => new EnrollmentReadDto
                {
                    Id = e.Id,
                    Grade = e.Grade,
                    StudentId = e.StudentId,
                    StudentName = e.Student.FullName,
                    CourseId = e.CourseId,
                    CourseTitle = e.Course.CourseName
                })
                .ToListAsync();

            return Ok(enrollments);
        }

        
        [HttpGet("{id}")]
        public async Task<ActionResult<EnrollmentReadDto>> GetById(Guid id)
        {
            var e = await _context.Enrollments
                .Include(x => x.Student)
                .Include(x => x.Course)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (e == null)
                return NotFound();

            return new EnrollmentReadDto
            {
                Id = e.Id,
                Grade = e.Grade,
                StudentId = e.StudentId,
                StudentName = e.Student.FullName,
                CourseId = e.CourseId,
                CourseTitle = e.Course.CourseName
            };
        }

        
        [HttpPost]
        public async Task<ActionResult<EnrollmentReadDto>> Create(EnrollmentCreateDto dto)
        {
            var enrollment = new Enrollment
            {
                Id = Guid.NewGuid(),
                StudentId = dto.StudentId,
                CourseId = dto.CourseId
            };

            _context.Enrollments.Add(enrollment);
            await _context.SaveChangesAsync();

            var result = await _context.Enrollments
                .Include(e => e.Student)
                .Include(e => e.Course)
                .Where(e => e.Id == enrollment.Id)
                .Select(e => new EnrollmentReadDto
                {
                    Id = e.Id,
                    Grade = e.Grade,
                    StudentId = e.StudentId,
                    StudentName = e.Student.FullName,
                    CourseId = e.CourseId,
                    CourseTitle = e.Course.CourseName
                })
                .FirstAsync();

            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        
        //[HttpPut("{id}")]
        //public async Task<IActionResult> Update(Guid id, EnrollmentUpdateDto dto)
        //{
        //    var enrollment = await _context.Enrollments.FindAsync(id);
        //    if (enrollment == null)
        //        return NotFound();
        //    enrollment.Grade = dto.Grade ?? enrollment.Grade;
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var enrollment = await _context.Enrollments.FindAsync(id);
            if (enrollment == null)
                return NotFound();

            _context.Enrollments.Remove(enrollment);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
