namespace mewo.Dtos
{
    public class TeacherFilter
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? Name { get; set; }
        public string? Department { get; set; }
        public int? Stage { get; set; }       
        public Guid? CourseId { get; set; }
        public SortBy SortBy { get; set; } = SortBy.Alphabetical;
        public SortOrder SortOrder { get; set; } = SortOrder.Asc;
    }
    
    public enum SortOrder
    {
        Asc,  // Ascending
        Desc  // Descending
    }

    public enum SortBy
    {
        Alphabetical,  
        Stage,
        EnrollmentDate
    }
}
