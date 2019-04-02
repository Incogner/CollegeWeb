using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace College.Models
{
    public class EFCourseRepository : ICourseRepository
    {
        private ApplicationDbContext context;

        public EFCourseRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }

        public IQueryable<Course> Courses => context.Courses;

        public void SaveCourse(Course course)
        {
            if (course.CourseId == 0)
            {
                context.Courses.Add(course);
            }
            else
            {
                Course dbEntry = context.Courses
                    .FirstOrDefault(c => c.CourseId == course.CourseId);
                if (dbEntry != null)
                {
                    dbEntry.CourseName = course.CourseName;
                    dbEntry.CourseCode = course.CourseCode;
                    dbEntry.CourseDesc = course.CourseDesc;
                    dbEntry.Enrollments = course.Enrollments;
                    //------
                    dbEntry.Semester = course.Semester;
                    dbEntry.CourseFee = course.CourseFee;
                    dbEntry.FacultyName = course.FacultyName;
                    dbEntry.NoOfCredit = course.NoOfCredit;
                }
            }
            context.SaveChanges();
        }

        public void DeleteCourse(Course course)
        {
            Course dbEntry = context.Courses
                .FirstOrDefault(c => c.CourseId == course.CourseId);
            if (dbEntry != null)
            {
                context.Courses.Remove(dbEntry);
                context.SaveChanges();
            }
        }

    }
}
