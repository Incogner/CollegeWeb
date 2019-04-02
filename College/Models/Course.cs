using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace College.Models
{
    public class Course
    {
        
        public int CourseId { get; set; }
        [Required(ErrorMessage = "Please enter course code")]
        [StringLength(10)]
        public string CourseCode { get; set; }
        [Required(ErrorMessage = "Please enter course name")]
        [StringLength(30)]
        public string CourseName { get; set; }
        [StringLength(200)]
        public string CourseDesc { get; set; }
        //public List<Student> Students { get; set; }
        public List<Enrollment> Enrollments { get; set; }
        //----------------

        [Required(ErrorMessage = "Please Enter the semester.")]
        public string Semester { get; set; }

        [Required(ErrorMessage = "Please Enter the course fee.")]
        [Range(150, 450, ErrorMessage = "Course fee must be within $150.00 and $450.00 range.")]
        [DataType(DataType.Currency)]
        public decimal CourseFee { get; set; }

        public string FacultyName { get; set; }

        [Required(ErrorMessage = "Please Enter the No of credit.")]
        [Range(1, 6, ErrorMessage = "Credit must be from 1 to 6.")]
        public int NoOfCredit { get; set; }

    }
}
