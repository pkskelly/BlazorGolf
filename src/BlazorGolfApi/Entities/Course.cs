namespace BlazorGolfApi.Entities
{
    public class Course
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = String.Empty;
        public int Slope { get; set; } = 55;
    }

}
