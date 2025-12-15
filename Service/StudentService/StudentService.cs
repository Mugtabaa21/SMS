using mewo.Dtos;
using mewo.Models;
using mewo.Repository.StudentRepo;
using Microsoft.EntityFrameworkCore;

namespace mewo.Service.StudentService
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepo _studentRepo;
        public StudentService(IStudentRepo studentRepo)
        {
            _studentRepo = studentRepo;
        }
        public async Task<CreateStudentDto> CreateStudent(CreateStudentDto studentDto)
        {
            // FIX 1: Handle the file upload
            // You cannot assign IFormFile directly to string. 
            // Usually, you save the file to a folder and store the specific path.
            // For now, let's just use the FileName to fix the error.
            string imagePath = "default.png";

            if (studentDto.ImageUrl != null)
            {
                // TODO: Add logic here to actually save the file to "wwwroot/images"
                imagePath = studentDto.ImageUrl.FileName;
            }

            var newStudent = new Student
            {
                FullName = studentDto.FullName,
                College = studentDto.College,
                Department = studentDto.Department,
                Stage = studentDto.Stage,
                ImageUrl = imagePath // Assign the string (path), not the file object
            };

            await _studentRepo.CreateStudent(newStudent);

            // FIX 2: Return the DTO, not the Entity
            // You promised to return 'CreateStudentDto', so you must return that type.
            return studentDto;
        }

        public async Task DeleteStudent(Guid id)
        {
            await _studentRepo.DeleteStudent(id);
        }

        public async Task<IEnumerable<StudentDto>> GetAllStudentDefualt()
        {
            var students = await _studentRepo.GetAllStudentDefualt();
            var studentDtos = students.Select(student => new StudentDto
            {
                Id = student.Id,
                FullName = student.FullName,
                College = student.College,
                Department = student.Department,
                ImageUrl = student.ImageUrl,
                EnrollmentDate = student.EnrollmentDate
            });
            return studentDtos;
        }

        public async Task<StudentDto?> GetStudentById(Guid id)
        {
            var student = await _studentRepo.GetStudentById(id);
            return student == null ? null : new StudentDto
            {
                Id = student.Id,
                FullName = student.FullName,
                College = student.College,
                Department = student.Department,
                Stage = (int)student.Stage,
                ImageUrl = student.ImageUrl,
                EnrollmentDate = student.EnrollmentDate
            };

        }

        public async Task UpdateStudent(Guid id, UpdateStudentDto dto)
        {
            
            var currentStudent = await _studentRepo.GetStudentById(id);

            if (currentStudent == null) return;

            // 2. Update the fields (Partial Update)
            if (!string.IsNullOrEmpty(dto.FullName)) currentStudent.FullName = dto.FullName;
            if (!string.IsNullOrEmpty(dto.College)) currentStudent.College = dto.College;
            if (!string.IsNullOrEmpty(dto.Department)) currentStudent.Department = dto.Department;
            if (dto.Stage > 0) currentStudent.Stage = dto.Stage;
            if (dto.ImageUrl != null) currentStudent.ImageUrl = dto.ImageUrl.FileName;

            // 3. Save changes using the NEW repository method
            await _studentRepo.UpdateStudent(currentStudent);
        }
    }
}
