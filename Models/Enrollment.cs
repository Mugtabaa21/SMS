using System.ComponentModel.DataAnnotations.Schema;

namespace mewo.Models
{
    public class Enrollment
    {
        public Guid Id { get; set; }
        public Double? Grade { get; set; }


        [ForeignKey("Student")]
        public Guid StudentId { get; set; }
        public Student Student { get; set; }

        [ForeignKey("Course")]
        public Guid CourseId { get; set; }
        public Course Course { get; set; }
    }
}
