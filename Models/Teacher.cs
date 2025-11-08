using System.ComponentModel.DataAnnotations.Schema;

namespace mewo.Models
{
    public class Teacher
    {
        public Guid Id { get; set; } // The Teacher's own primary key

        // Profile Fields
        public string FullName { get; set; }
        public string? Department { get; set; }
        public string? OfficeNumber { get; set; }

        /// <summary>
        /// e.g., "Professor", "Lecturer", "Associate Professor"
        /// </summary>
        public string Title { get; set; }

        // --- Relationships ---

        /// <summary>
        /// This is the foreign key that creates the one-to-one link 
        /// between the Teacher Profile and the ApplicationUser login.
        /// </summary>
        [ForeignKey("ApplicationUser")]
        public String ?ApplicationUserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }
        public ICollection<Course>? Courses { get; set; }
    }
}
