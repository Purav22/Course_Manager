using Microsoft.EntityFrameworkCore;

namespace CourseManagerApp.Entities
{
    public class CourseDBContext : DbContext
    {
        public CourseDBContext(DbContextOptions<CourseDBContext> options)
            : base(options)
        {
        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>().HasData(
                 new Course
                 {
                     CourseId = 1,
                     Name = "Microsoft Web Technologies",
                     Instructor = "Manny Singh",
                     StartDate = DateTime.Now,
                     RoomNumber = "4G01"
                 },
                 new Course
                 {
                     CourseId = 2,
                     Name = "Software Development Project",
                     Instructor = "Moti Ahmed",
                     StartDate = DateTime.Now.AddDays(+2),
                     RoomNumber = "3G13"
                 });

        }
    }
}
