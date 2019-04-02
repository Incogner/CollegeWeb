using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace College.Models
{
    public interface IEnrollmentRepository
    {

        IQueryable<Enrollment> Enrollments { get; }

        void SaveEnrollment(Enrollment enrollment);

    }
}
