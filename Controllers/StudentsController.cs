using mewo.Dtos;
using mewo.Service.StudentService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace mewo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IStudentService _studentService;
        public StudentsController(IStudentService student, IWebHostEnvironment hostEnvironment)

        {
            _studentService = student;
            _hostEnvironment = hostEnvironment;
        }

        // 1. GET: api/Students
        [HttpGet]
        public async Task<IActionResult> GetAllStudents()
        {
            var students = await _studentService.GetAllStudentDefualt();
            return Ok(students);
        }

        // 2. GET: api/Students/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudentById(Guid id)
        {
            var student = await _studentService.GetStudentById(id);

            if (student == null)
            {
                return NotFound($"Student with ID {id} not found.");
            }

            return Ok(student);
        }

        // 3. POST: api/Students
        [HttpPost]
        public async Task<IActionResult> CreateStudent([FromForm] CreateStudentDto studentDto)
        {
            // Note: We use [FromForm] because your DTO likely contains an image file (IFormFile)
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdStudent = await _studentService.CreateStudent(studentDto);

            // Returns 200 OK with the created data
            return Ok(createdStudent);
        }
        [HttpPut("{id}")]
        public async Task<OkObjectResult> UpdateStudent(Guid id, UpdateStudentDto dto)
        {
            await _studentService.UpdateStudent(id, dto);
            return Ok("Updated successfully");
        }

        // 5. DELETE: api/Students/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(Guid id)
        {
            try
            {
                await _studentService.DeleteStudent(id);
                return Ok("Student deleted successfully.");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}