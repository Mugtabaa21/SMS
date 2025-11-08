using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mewo.Dtos;
using mewo.Models;

namespace mewo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly mewo.Models.AppDbContext _context;
        public CourseController(mewo.Models.AppDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllCourse()
        {
            var result = await _context.Courses.ToListAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCourseById(Guid id)
        {
            var course = await _context.Courses.FindAsync(id);

            if (course == null)
                return NotFound();

            return Ok(course);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCourse(CreateCourseDto createCourse)
        {
            var course = new Course
            {
                TeacherId = createCourse.TeacherId,
                CourseName = createCourse.CourseName,
                Credits = createCourse.Credits,
                CourseCode = createCourse.CourseCode,
                Description = createCourse.Description,
            };
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();
            return Ok(course);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCourse(Guid id, UpdateCourseDto updateCourse)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            course.TeacherId = updateCourse.TeacherId ?? course.TeacherId;
            course.CourseName = updateCourse.CourseName ?? course.CourseName;
            course.Credits = updateCourse.Credits ?? course.Credits;
            course.CourseCode = updateCourse.CourseCode ?? course.CourseCode;
            course.Description = updateCourse.Description ?? course.Description;
            await _context.SaveChangesAsync();
            return Ok(course);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(Guid id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            return Ok(course);
        }
    }
}
