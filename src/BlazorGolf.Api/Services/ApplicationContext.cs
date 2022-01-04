using BlazorGolf.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorGolf.Api.Services
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }
        public DbSet<Course>? Courses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultContainer("Courses");

            modelBuilder.Entity<Course>().ToContainer("Courses");
            modelBuilder.Entity<Course>().HasKey(c => c.CourseId);
            modelBuilder.Entity<Course>().HasPartitionKey(c => c.PartitionKey);
            modelBuilder.Entity<Course>().HasDiscriminator<string>("PartitionKey").HasValue<Course>("Course");
            modelBuilder.Entity<Course>().Property(c => c.ETag).IsETagConcurrency();
        }
    }
}