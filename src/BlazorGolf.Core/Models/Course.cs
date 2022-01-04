using BlazorGolf.Core.Models;

namespace BlazorGolf.Core.Models
{
    public class Course
    {
        public string CourseId { get; set; } = Guid.NewGuid().ToString();
        public string PartitionKey { get; set; } = "Course";
        public string? ETag { get; set; }
        public string Name { get; set; } = String.Empty;
        public string? City { get; set; }
        public string? State { get; set; } = String.Empty;
        public IEnumerable<Tee> Tees { get; set; } = new List<Tee>();
    }
}
