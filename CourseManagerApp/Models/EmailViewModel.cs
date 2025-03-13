using CourseManagerApp.Entities;

namespace CourseManagerApp.Models
{
    public class EmailViewModel
    {
        public Course? Course { get; set; }
        public Student? Student { get; set; }
        public string BaseUrl { get; set; }
    }
}
