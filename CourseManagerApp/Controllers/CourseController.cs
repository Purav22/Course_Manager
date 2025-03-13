using Azure.Core;
using CourseManagerApp.Entities;
using CourseManagerApp.Models;
using CourseManagerApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace CourseManagerApp.Controllers
{
    public class CourseController : UserCookiesController
    {
        private readonly CourseDBContext _context;
        private readonly IEmail _email;
        public CourseController(CourseDBContext dbContext, IEmail email)
        {
            _context = dbContext;
            _email = email;
        }

        [HttpGet("courses")]
        public IActionResult Items()
        {
            ViewBag.UserVisitsMessage = UserVisitMessage();
            var courses = _context.Courses
                            .Include(c => c.Students)
                            .ToList();
            return View("Items", courses);
        }

        [HttpGet("courses/add-course")]
        public IActionResult Create()
        {
            ViewBag.UserVisitsMessage = UserVisitMessage();
            return View("Create", new Course());
        }

        [HttpPost("courses/add-course")]
        public IActionResult Create(Course course)
        {
            ViewBag.UserVisitsMessage = UserVisitMessage();
            if (ModelState.IsValid)
            {
                _context.Courses.Add(course);
                _context.SaveChanges();
                TempData["LastActionMessage"] = "New Course added successfully.";
                return RedirectToAction("Items");
            }
            return View("Create", course);
        }

        [HttpGet("courses/{id}/edit-course")]
        public IActionResult Edit(int id)
        {
            ViewBag.UserVisitsMessage = UserVisitMessage();
            var course = _context.Courses.Find(id);
            return View("Edit", course);
        }

        [HttpPost("courses/{id}/edit-course")]
        public IActionResult Edit(Course course, int id)
        {
            ViewBag.UserVisitsMessage = UserVisitMessage();
            if (id != course.CourseId) return NotFound();
            if (ModelState.IsValid)
            {
                _context.Courses.Update(course);
                _context.SaveChanges();
                TempData["LastActionMessage"] = "Course updated successfully.";
                return RedirectToAction("Items");
            }
            return View("Edit", course);
        }

        [HttpGet("courses/{id}/manage-course")]
        public IActionResult ManageCourse(int id)
        {
            ViewBag.UserVisitsMessage = UserVisitMessage();
            CourseViewModel courseViewModel = new();
            var course = _context.Courses
                            .Include(c => c.Students)
                            .FirstOrDefault(c => c.CourseId == id);
            courseViewModel.Course = course;
            if (course != null && course.Students != null && course.Students.Any())
            {
                courseViewModel.EmailNotSentCount = course.Students.Where(s => s.EnrollmentStatus == EnrollmentStatus.ConfirmationMessageNotSent).Count();
                courseViewModel.EmailSentCount = course.Students.Where(s => s.EnrollmentStatus == EnrollmentStatus.ConfirmationMessageSent).Count();
                courseViewModel.EnrollmentConfirmedCount = course.Students.Where(s => s.EnrollmentStatus == EnrollmentStatus.EnrollmentConfirmed).Count();
                courseViewModel.EnrollmentDeclinedCount = course.Students.Where(s => s.EnrollmentStatus == EnrollmentStatus.EnrollmentDeclined).Count();
            }
            return View("ManageCourse", courseViewModel);
        }

        [HttpPost("courses/{id}/add-student")]
        public IActionResult CreateStudent(CourseViewModel courseViewModel, int id)
        {
            ViewBag.UserVisitsMessage = UserVisitMessage();
            var course = _context.Courses
                        .Include(c => c.Students)
                        .FirstOrDefault(c => c.CourseId == id);
            if (course != null)
            {
                course.Students.Add(courseViewModel.NewStudent);
            }
            _context.SaveChanges();
            TempData["LastActionMessage"] = "Student successfully added to the course.";
            return RedirectToAction("ManageCourse", new { id });
        }

        [HttpPost("courses/{id}/send-confirmation")]
        public IActionResult SendConfirmation(int id)
        {
            ViewBag.UserVisitsMessage = UserVisitMessage();
            var students = _context.Students.Where(s => s.CourseId == id
                                                    && s.EnrollmentStatus == EnrollmentStatus.ConfirmationMessageNotSent).ToList();

            var course = _context.Courses.Find(id);
            string baseUrl = $"{Request.Scheme}://{Request.Host}";
            foreach (var item in students)
            {
                string subject = String.Format(@"Enrollment confirmation for ""{0}"" required", course.Name);
                string body = String.Format(@"<h1>Hello {0}:</h1><p>Your request to enroll in the course ""{1}"" in room {2} starting {3} with instructor {4}.",
                    item.Name,
                    course.Name,
                    course.RoomNumber,
                    course.StartDate.Value.ToString("MM/dd/yyy", CultureInfo.InvariantCulture),
                   course.Instructor);

                string confirmEnrollmentUrl = String.Format(@"{0}/courses/{1}/confirm-enrollment",
                    baseUrl,
                   item.StudentId);

                body += String.Format(@"We are pleased to have you in the course so if you could <a href=""{0}"">confirm your enrollment</a> as soon as possible that would be appreciated!.</p>",
                    confirmEnrollmentUrl);
                body += String.Format(@"<p>Sincerely,</p>");
                body += String.Format(@"<p>The course manager App</p>");
                _email.SendEmail(subject, body, item.Email);
                item.EnrollmentStatus = EnrollmentStatus.ConfirmationMessageSent;
            }
            _context.SaveChanges();
            TempData["LastActionMessage"] = "Confirmation messages sent successfully.";

            return RedirectToAction("ManageCourse", new { id });
        }

        [HttpGet("courses/{studentId}/confirm-enrollment")]
        public IActionResult ConfirmEnrollment(int studentId)
        {
            ViewBag.UserVisitsMessage = UserVisitMessage();
            Student student = _context.Students.Find(studentId);
            CourseViewModel courseViewModel = new()
            {
                NewStudent = student,
                Course = _context.Courses.Find(student.CourseId)
            };
            return View(courseViewModel);
        }

        [HttpPost("courses/{studentId}/confirm-enrollment")]
        public IActionResult ConfirmEnrollment(CourseViewModel courseViewModel, int studentId)
        {
            ViewBag.UserVisitsMessage = UserVisitMessage();
            var student = _context.Students.Find(studentId);
            if (courseViewModel.IsConfirmEnrollment == "Yes")
            {
                student.EnrollmentStatus = EnrollmentStatus.EnrollmentConfirmed;
                ViewBag.ResponseMessage = "We are happy that you will be joining us!";
            }
            else
            {
                student.EnrollmentStatus = EnrollmentStatus.EnrollmentDeclined;
                ViewBag.ResponseMessage = "We are sorry to see you go!";
            }
            _context.SaveChanges();
            return View("EnrollmentStatus");
        }
    }
}
