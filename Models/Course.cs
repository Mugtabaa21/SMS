using System.ComponentModel.DataAnnotations.Schema;

namespace mewo.Models
{
    public class Course
    {
        public Guid Id { get; set; }
        public string CourseName { get; set; }
        public string CourseCode { get; set; }
        public string Description { get; set; }
        public int Credits { get; set; }

        // --- Relationships ---

        /// <summary>
        /// Foreign key for the Teacher who teaches this course (many-to-one).
        /// </summary>
        [ForeignKey("Teacher")]
        public Guid TeacherId { get; set; }
        public Teacher Teacher { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; }
    }

}

