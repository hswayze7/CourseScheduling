﻿using CourseScheduling.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CourseScheduling.Controllers
{
    public class CourseController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CourseController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Show the list of available courses
        public async Task<IActionResult> Index()
        {
            var studentId = HttpContext.Session.GetInt32("StudentId");

            if (studentId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var availableCourses = await _context.Courses.ToListAsync();

            // Fetch the student's enrolled courses
            var student = await _context.Students
                .Include(s => s.Enrollments)
                .ThenInclude(e => e.Course)
                .FirstOrDefaultAsync(s => s.StudentId == studentId);

            
            var enrolledCourses = student?.Enrollments.ToList() ?? new List<Enrollment>();

            var viewModel = new CourseEnrollmentViewModel
            {
                AvailableCourses = availableCourses,
                EnrolledCourses = enrolledCourses
            };

            return View(viewModel); // Pass the view model to the view
        }


        // Enroll in a course
        [HttpPost]
        public async Task<IActionResult> Enroll([FromBody] EnrollViewModel model)
        {
            //Getting student ID
            var studentId = HttpContext.Session.GetInt32("StudentId");
            //Error checking
            if (studentId == null)
            {
                return Unauthorized("Student not logged in.");
            }
            //Error checking for courseID to make sure course is there
            if (model.CourseId <= 0)
            {
                return BadRequest("Invalid courseId.");
            }

            // Check if the student is already enrolled in the course
            var existingEnrollment = await _context.Enrollments
                .FirstOrDefaultAsync(e => e.StudentId == studentId && e.CourseId == model.CourseId);

            if (existingEnrollment != null)
            {
                return BadRequest("You are already enrolled in this course.");
            }

            var course = await _context.Courses.FindAsync(model.CourseId);
            if (course == null)
            {
                return BadRequest("Course not found.");
            }

            // Create a new enrollment
            var enrollment = new Enrollment
            {
                StudentId = (int)studentId,
                CourseId = model.CourseId,
                EnrollmentDate = DateTime.Now
            };

            _context.Enrollments.Add(enrollment);
            await _context.SaveChangesAsync();

            return Ok("Enrollment successful.");
        }

        //Gives the student an option to delete a course after selecting to enroll in said course
        [HttpPost]
        public async Task<IActionResult> DeleteEnrollment([FromBody] int enrollmentId)
        {
            var studentId = HttpContext.Session.GetInt32("StudentId");
            if (studentId == null)
            {
                return Unauthorized("Student not logged in.");
            }

            var enrollment = await _context.Enrollments
                .FirstOrDefaultAsync(e => e.EnrollmentId == enrollmentId && e.StudentId == studentId);

            if (enrollment == null)
            {
                return BadRequest("Enrollment not found or not authorized to delete.");
            }

            _context.Enrollments.Remove(enrollment);
            await _context.SaveChangesAsync();

            Console.WriteLine("Enrollment deleted successfully.");
            return Ok("Enrollment deleted successfully.");
        }


        //Function to grab what classes the student is enrolled in + add to calendar
        [HttpGet]
        public async Task<IActionResult> GetEnrolledCourses()
        {
            //Assigns studentID to variable
            var studentId = HttpContext.Session.GetInt32("StudentId");
            if (studentId == null)
            {
                return Unauthorized("Student not logged in.");
            }

            var enrollments = await _context.Enrollments
                .Where(e => e.StudentId == studentId)
                .Include(e => e.Course)
                .Distinct()
                .ToListAsync();

            // Prepare events for FullCalendar
            var calendarEvents = new List<object>();

            foreach (var enrollment in enrollments)
            {
                var startDateTimes = GetDateTimesForEvent(enrollment.Course.Time, "start");
                var endDateTimes = GetDateTimesForEvent(enrollment.Course.Time, "end");

                for (int i = 0; i < startDateTimes.Count; i++)
                {
                    calendarEvents.Add(new
                    {
                        title = enrollment.Course.CourseName,
                        start = startDateTimes[i].ToString("s"), // ISO 8601 format
                        end = endDateTimes[i].ToString("s")      // ISO 8601 format
                    });
                }
            }

            return Json(calendarEvents);
        }


        [HttpGet]
        public async Task<IActionResult> GetEnrolledCoursesTable()
        {
            var studentId = HttpContext.Session.GetInt32("StudentId");
            if (studentId == null)
            {
                return Unauthorized("Student not logged in.");
            }

            var enrollments = await _context.Enrollments
                .Where(e => e.StudentId == studentId)
                .Include(e => e.Course)
                .ToListAsync();

            return PartialView("_EnrolledCourses", enrollments);
        }


        private List<DateTime> GetDateTimesForEvent(string timeString, string type)
        {
            // Mapping of days to DayOfWeek values
            var dayMap = new Dictionary<string, DayOfWeek>
            {
                { "M", DayOfWeek.Monday },
                { "T", DayOfWeek.Tuesday },
                { "W", DayOfWeek.Wednesday },
                { "TR", DayOfWeek.Thursday },
                { "F", DayOfWeek.Friday },
                { "S", DayOfWeek.Saturday },
                { "U", DayOfWeek.Sunday }
            };

            // Split the timeString into day(s) and time range
            var parts = timeString.Split(new[] { ' ' }, 3);
            if (parts.Length != 3)
            {
                throw new FormatException("Invalid time format. Expected format: 'Days StartTime - EndTime'.");
            }

            var days = parts[0].Split('/'); // Split days if there are multiple, -> "M/W/F"
            var timeRange = parts[1] + " " + parts[2]; // Combine start time and end time
            var times = timeRange.Split(new[] { " - " }, StringSplitOptions.None);
            if (times.Length != 2)
            {
                throw new FormatException("Invalid time range format. Expected format: 'StartTime - EndTime'.");
            }

            var timeToParse = type == "start" ? times[0] : times[1];

            // Parse the time into hours, minutes, and AM/PM
            var timeMatch = System.Text.RegularExpressions.Regex.Match(timeToParse.Trim(), @"(\d+):(\d+) (AM|PM)");
            if (!timeMatch.Success)
            {
                throw new FormatException("Invalid time format. Expected format: 'HH:MM AM/PM'.");
            }

            int hours = int.Parse(timeMatch.Groups[1].Value);
            int minutes = int.Parse(timeMatch.Groups[2].Value);
            string meridian = timeMatch.Groups[3].Value;

            if (meridian == "PM" && hours != 12) hours += 12;
            if (meridian == "AM" && hours == 12) hours = 0;

            DateTime today = DateTime.Today;
            HashSet<DateTime> uniqueEventDates = new HashSet<DateTime>();

            // Generate dates for each specified day
            foreach (var day in days)
            {
                if (dayMap.TryGetValue(day.Trim(), out DayOfWeek targetDay))
                {
                    int daysUntilTarget = ((int)targetDay - (int)today.DayOfWeek + 7) % 7;
                    DateTime eventDate = today.AddDays(daysUntilTarget).AddHours(hours).AddMinutes(minutes);

                    // Use HashSet to ensure uniqueness of event dates
                    uniqueEventDates.Add(eventDate);
                }
            }

            return uniqueEventDates.ToList();
        }





    }
}
