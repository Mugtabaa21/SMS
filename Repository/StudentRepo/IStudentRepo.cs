using mewo.Models;

namespace mewo.Repository.StudentRepo
{
    public interface IStudentRepo
    {
        Task <IEnumerable<Student>> GetAllStudentDefualt();
        Task<Student> GetStudentById(Guid id);
        Task<Student> CreateStudent(Student student);
        Task DeleteStudent(Guid id);
        Task UpdateStudent(Student student);
    }
}
