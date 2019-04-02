using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace College.Models
{
    public interface IStudentRepository
    {

        IQueryable<Student> Students { get; }

        void SaveStudent(Student student);
        void SavePic(Student student, string pic);
        void DeleteStudent(Student student);

    }
}
