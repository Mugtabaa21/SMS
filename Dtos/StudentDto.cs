namespace mewo.Dtos
{
    public class StudentDto
    {
        public Guid Id { get; set; } 

        public string FullName { get; set; }
        public string College { get; set; }
        public string Department { get; set; }
        public int Stage { get; set; } 

        public string ImageUrl { get; set; }

        public DateTime? EnrollmentDate { get; set; }
    }

    public class CreateStudentDto
    {

        public string FullName { get; set; }
        public string College { get; set; }
        public string Department { get; set; }
        public int Stage { get; set; } 

        public IFormFile? ImageUrl { get; set; }

    }

    public class UpdateStudentDto
    {
        public string? FullName { get; set; }
        public string? College { get; set; }
        public string? Department { get; set; }
        public int? Stage { get; set; } 

        public IFormFile? ImageUrl { get; set; }

    }

}
