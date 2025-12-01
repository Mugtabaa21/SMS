using mewo.Dtos;
using mewo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System; // Added for Guid, DateTime
using System.IO; // Needed for file operations
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting; // Added for IWebHostEnvironment
using System.Linq; // Added for IQueryable

namespace mewo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class StudentsController : ControllerBase
    {
        private readonly mewo.Models.AppDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public StudentsController(AppDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStudentDefualt()
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
        public async Task<IActionResult> CreateStudent([FromForm] CreateStudentDto createStudent)
        {
            string newImageUrl = null;
            // Changed to use the clear 'ImageFile' property
            if (createStudent.ImageUrl != null)
            {
                newImageUrl = await SaveImageAsync(createStudent.ImageUrl);
            }

            var student = new Student
            {
                FullName = createStudent.FullName,
                College = createStudent.College,
                Department = createStudent.Department,
                Stage = createStudent.Stage,
                ImageUrl = newImageUrl,
                EnrollmentDate = DateTime.UtcNow,
                IsActive = true
            };
            _context.Students.Add(student);
            await _context.SaveChangesAsync();
            return Ok(student);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudent(Guid id, [FromForm] UpdateStudentDto updateStudent)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            if (updateStudent.ImageUrl != null)
            {
                
                string oldImageUrl = student.ImageUrl;

                // 2. Save the new image to the server
                student.ImageUrl = await SaveImageAsync(updateStudent.ImageUrl);

                // 3. Delete the old image file (if it existed)
                DeleteImage(oldImageUrl);
            }

            student.FullName = updateStudent.FullName ?? student.FullName;
            student.College = updateStudent.College ?? student.College;
            student.Department = updateStudent.Department ?? student.Department;
            student.Stage = updateStudent.Stage ?? student.Stage;

            // Note: You probably don't want to update EnrollmentDate every time you edit
            // student.EnrollmentDate = DateTime.UtcNow; 

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

            // 1. Delete the associated image file first
            DeleteImage(student.ImageUrl);

            // 2. Now remove the student record
            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return Ok(student);
        }

       
        private async Task<string> SaveImageAsync(IFormFile imageFile)
        {
            string wwwRootPath = _hostEnvironment.WebRootPath;
            if (string.IsNullOrEmpty(wwwRootPath))
            {
                wwwRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            }

            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
            string imageDirectory = Path.Combine(wwwRootPath, "images", "students");

            if (!Directory.Exists(imageDirectory))
            {
                Directory.CreateDirectory(imageDirectory);
            }

            string filePath = Path.Combine(imageDirectory, fileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            return $"/images/students/{fileName}";
        }

        // --- NEW Helper Method to Delete Image ---
        private void DeleteImage(string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl))
                return;

            try
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                if (string.IsNullOrEmpty(wwwRootPath))
                {
                    wwwRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                }

                // Path.Combine correctly handles the leading "/"
                string filePath = Path.Combine(wwwRootPath, imageUrl.TrimStart('/'));

                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }
            catch (Exception ex)
            {
                // Log the error but don't stop the request
                Console.WriteLine($"Error deleting image: {ex.Message}");
            }
        }
    }
}