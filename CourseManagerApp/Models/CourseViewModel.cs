using CourseManagerApp.Entities;

namespace CourseManagerApp.Models
{
    public class CourseViewModel
    {
        public Course? Course { get; set; }
        public Student? NewStudent { get; set; }
        public string IsConfirmEnrollment { get; set; }
        public int EmailNotSentCount { get; set; }
        public int EmailSentCount { get; set; }
        public int EnrollmentConfirmedCount { get; set; }
        public int EnrollmentDeclinedCount { get; set; }
    }
}
