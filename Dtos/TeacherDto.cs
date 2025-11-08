namespace mewo.Dtos
{
    public class TeacherDto
    {
        public Guid Id { get; set; }

        public string FullName { get; set; }
        public string Department { get; set; }
        public string OfficeNumber { get; set; }

        public string Title { get; set; }

    }

    public class CreateTeacherDto
    {
       
        public string FullName { get; set; }
        public string Department { get; set; }
        public string OfficeNumber { get; set; }

        public string Title { get; set; }

    }

    public class UpdateTeacherDto 
    {
        public string? FullName { get; set; }
        public string? Department { get; set; }
        public string? OfficeNumber { get; set; }

        public string? Title { get; set; }
    }

}
