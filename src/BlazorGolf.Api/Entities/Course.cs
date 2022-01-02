using BlazorGolf.Core.Models;

namespace BlazorGolf.Api.Entities
{
    public class Course
    {
        public string CourseID { get; set; } = Guid.NewGuid().ToString();
        public string PartitionKey { get; set; } = "Course";
        public string? ETag { get; set; }
        public string Name { get; set; } = String.Empty;
        public int Slope { get; set; } = 55;
        //public int Rating { get; set; } = 71.8;
        public string? City { get; set; }
        public string? State { get; set; } = String.Empty;
    }
}
