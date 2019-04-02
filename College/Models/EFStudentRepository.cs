using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace College.Models
{
    public class EFStudentRepository : IStudentRepository
    {
        private ApplicationDbContext context;

        public EFStudentRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }

        public IQueryable<Student> Students => context.Students;

        public void SaveStudent(Student student)
        {
            if (student.StudentId == 0)
            {
                context.Students.Add(student);
            }
            else
            {
                Student dbEntry = context.Students
                    .FirstOrDefault(s => s.StudentId == student.StudentId);
                if (dbEntry != null)
                {
                    dbEntry.UserName = student.UserName;
                    dbEntry.FName = student.FName;
                    dbEntry.LName = student.LName;
                    dbEntry.Address = student.Address;
                    dbEntry.City = student.City;
                    dbEntry.Province = student.Province;
                    dbEntry.Enrollments = student.Enrollments;
                    dbEntry.Pic = student.Pic;
                }
            }
            context.SaveChanges();
        }
        public void SavePic(Student student, string pic)
        {
            if (student != null)
            {
                Student dbEntry = context.Students
                    .FirstOrDefault(s => s.StudentId == student.StudentId);
                if (dbEntry != null)
                {
                    dbEntry.Pic = pic;
                }
            }
            context.SaveChanges();
        }

        public void DeleteStudent(Student student)
        {
            Student dbEntry = context.Students
                .FirstOrDefault(s => s.StudentId == student.StudentId);
            if (dbEntry != null)
            {
                context.Students.Remove(dbEntry);
                context.SaveChanges();
            }
        }

    }
}
