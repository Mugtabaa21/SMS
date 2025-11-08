namespace mewo.Dtos
{
    public class CourseDto
    {
        public Guid Id { get; set; }
        public string CourseName { get; set; }
        public string CourseCode { get; set; }
        public string Description { get; set; }
        public int Credits { get; set; }
        public Guid TeacherId { get; set; }

    }

    public class CreateCourseDto
    {

        public string CourseName { get; set; }
        public string CourseCode { get; set; }
        public string Description { get; set; }
        public int Credits { get; set; }
        public Guid TeacherId { get; set; }

    }

    public class UpdateCourseDto
    {
        public string? CourseName { get; set; }
        public string? CourseCode { get; set; }
        public string? Description { get; set; }
        public int? Credits { get; set; }
        public Guid? TeacherId { get; set; }
    }

}
