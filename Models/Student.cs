using System.ComponentModel.DataAnnotations.Schema;

namespace mewo.Models
{
    public class Student
    {
        public Guid Id { get; set; } // The Student's own primary key

        // Profile Fields
        public string FullName { get; set; }
        public string? College { get; set; }
        public string? Department { get; set; }
        public int? Stage { get; set; } // e.g., 1, 2, 3, or 4

        /// <summary>
        /// Stores the URL/path to the student's profile picture.
        /// The '?' makes the string "nullable" (optional).
        /// </summary>
        public string? ImageUrl { get; set; }

        public DateTime EnrollmentDate { get; set; }

        // --- Relationships ---

        /// <summary>
        /// This is the foreign key that creates the one-to-one link 
        /// between the Student Profile and the ApplicationUser login.
        /// </summary>
        [ForeignKey("ApplicationUser")]
        public string? ApplicationUserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }
        public ICollection<Enrollment>? Enrollments { get; set; }
    }
}
