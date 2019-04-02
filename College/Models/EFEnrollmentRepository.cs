using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace College.Models
{
    public class EFEnrollmentRepository : IEnrollmentRepository
    {
        private ApplicationDbContext context;

        public EFEnrollmentRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }

        public IQueryable<Enrollment> Enrollments => context.Enrollments;

        public void SaveEnrollment(Enrollment enrollment)
        {
            if (enrollment.EnrollmentId == 0)
            {
                context.Enrollments.Add(enrollment);
            }
            else
            {
                Enrollment dbEntry = context.Enrollments
                    .FirstOrDefault(e => e.EnrollmentId == enrollment.EnrollmentId);
                if (dbEntry != null)
                {
                    dbEntry.StudentId = enrollment.StudentId;
                    dbEntry.CourseId = enrollment.CourseId;
                    dbEntry.EnrollDate = DateTime.Now;
                }
            }
            context.SaveChanges();
        }
    }
}
