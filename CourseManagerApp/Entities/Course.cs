using System.ComponentModel.DataAnnotations;

namespace CourseManagerApp.Entities
{
    public class Course
    {
        public Course()
        {
            Students = new List<Student>();
        }
        public int CourseId { get; set; }

        [Required(ErrorMessage = "Enter Course Name!!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Enter Instructor Name!!!")]
        public string Instructor { get; set; }

        [Required(ErrorMessage = "Enter Start Date!!!")]
        public DateTime? StartDate { get; set; }

        [Required(ErrorMessage = "Enter Room Number!!")]
        [RegularExpression(@"^[1-9][A-Z]\d{2}$", ErrorMessage = "Invalid Room Number. Please Enter correct Format: eg 1A15")]
        public string RoomNumber { get; set; }

        public ICollection<Student> Students { get; set; }
    }
}
