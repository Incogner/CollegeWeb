using College.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace College.Models
{
    public class Cart
    {
        private List<CartLine> cLines = new List<CartLine>();

        public virtual void AddCourse(Course course)
        {
            CartLine cLine = cLines
                .Where(s => s.Course.CourseId == course.CourseId)
                .FirstOrDefault();

            if (cLine == null)
            {
                cLines.Add(new CartLine
                {
                    Course = course
                });
            }
        }

        public virtual void RemoveCourse(Course course) =>
            cLines.RemoveAll(s => s.Course.CourseId == course.CourseId);

        public virtual decimal ComputeTotalValue() =>
            cLines.Sum(e => e.Course.CourseFee);

        public virtual void Clear() => cLines.Clear();

        public virtual IEnumerable<CartLine> CartLines => cLines;
    }

    public class CartLine
    {
        public int LineId { get; set; }
        public Course Course { get; set; }
        public int NoOfCourse { get; set; }
    }
}
