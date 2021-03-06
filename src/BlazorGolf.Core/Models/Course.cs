using BlazorGolf.Core.Models;

namespace BlazorGolf.Core.Models
{
    public class Course
    {
        public string CourseId { get; set; } = Guid.NewGuid().ToString();
        public string PartitionKey { get; set; } = "Course";
        public string? ETag { get; set; } = "initial" ;
        public string Name { get; set; } = String.Empty;
        public string? City { get; set; } = String.Empty;
        public string? State { get; set; } = String.Empty;
        public string? Phone { get; set; } = String.Empty;
        public IEnumerable<Tee> Tees { get; set; } = new List<Tee>();
    }
}
