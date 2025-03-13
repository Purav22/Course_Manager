using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CourseManagerApp.Entities
{
    public class Student
    {
        public int StudentId { get; set; }

        [Required(ErrorMessage = "Enter Student Name!!!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Enter Email Address!!!")]
        public string Email { get; set; }

        public EnrollmentStatus EnrollmentStatus { get; set; } = EnrollmentStatus.ConfirmationMessageNotSent;

        public int CourseId { get; set; }

        public Course Course { get; set; }
    }

    public enum EnrollmentStatus
    {
        [Description("Enrollment confirmation not sent")]
        ConfirmationMessageNotSent,

        [Description("Enrollment confirmation sent")]
        ConfirmationMessageSent,

        [Description("Enrollment confirmed")]
        EnrollmentConfirmed,

        [Description("Enrollment declined")]
        EnrollmentDeclined
    }
}
