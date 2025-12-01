using mewo.Dtos;
using mewo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System; 
using System.Linq;
using System.Threading.Tasks; 

namespace mewo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class TeacherController : ControllerBase
    {
        private readonly mewo.Models.AppDbContext _context;
        public TeacherController(mewo.Models.AppDbContext context)
        {
            _context = context;
        }
        [HttpGet("students")] 
        public async Task<IActionResult> GetAllStudent([FromQuery] TeacherFilter parameters)
        {
            // Start with the base query. Include Enrollments for the CourseId filter.
            IQueryable<Student> query = _context.Students
                                              .Include(s => s.Enrollments);            
            
               
                query = query.Where(p => p.IsActive == true);               
                if (!string.IsNullOrEmpty(parameters.Department))
                {
                    query = query.Where(p => p.Department == parameters.Department);
                }

                // Teacher Filter: Name (Search)
                if (!string.IsNullOrEmpty(parameters.Name))
                {
                    query = query.Where(p => p.FullName.Contains(parameters.Name));
                }


                if (parameters.Stage.HasValue)
                {
                    query = query.Where(p => p.Stage == parameters.Stage.Value);
                }
               
                if (parameters.CourseId.HasValue)
                {
                    query = query.Where(s => s.Enrollments
                        .Any(e => e.CourseId == parameters.CourseId.Value));
                }
            

            bool isDescending = (parameters.SortOrder == SortOrder.Desc);

            switch (parameters.SortBy)
            {
                case SortBy.Stage:
                    query = isDescending
                        ? query.OrderByDescending(p => p.Stage)
                        : query.OrderBy(p => p.Stage);
                    break;

                case SortBy.EnrollmentDate:
                    query = isDescending
                        ? query.OrderByDescending(p => p.EnrollmentDate)
                        : query.OrderBy(p => p.EnrollmentDate);
                    break;

                case SortBy.Alphabetical:
                default:
                    query = isDescending
                        ? query.OrderByDescending(p => p.FullName)
                        : query.OrderBy(p => p.FullName);
                    break;
            }


                query = query
                    .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                    .Take(parameters.PageSize);
            
            
            var students = await query.ToListAsync();
            return Ok(students);
        }

        [HttpGet("noAdd")]
        public async Task<IActionResult> GetAllTeacher()
        {
            var result = await _context.Teachers.ToListAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTeacherById(Guid id)
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