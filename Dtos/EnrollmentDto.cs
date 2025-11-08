namespace mewo.Dtos
{
    // Create
    public class EnrollmentCreateDto
    {
        public Guid StudentId { get; set; }
        public Guid CourseId { get; set; }
    }

    // Read (Get)
    public class EnrollmentReadDto
    {
        public Guid Id { get; set; }
        public double? Grade { get; set; }

        public Guid StudentId { get; set; }
        public string StudentName { get; set; }

        public Guid CourseId { get; set; }
        public string CourseTitle { get; set; }
    }

    // Update
    public class EnrollmentUpdateDto
    {
        public double? Grade { get; set; }
    }
}
