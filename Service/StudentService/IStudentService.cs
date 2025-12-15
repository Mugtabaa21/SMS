using mewo.Dtos;

namespace mewo.Service.StudentService
{
    public interface IStudentService
    {
        Task<IEnumerable<StudentDto>> GetAllStudentDefualt();
        Task<StudentDto?> GetStudentById(Guid id);
        Task<CreateStudentDto> CreateStudent(CreateStudentDto student);
        Task DeleteStudent(Guid id);
        Task UpdateStudent(Guid id,UpdateStudentDto student);
    }
}
