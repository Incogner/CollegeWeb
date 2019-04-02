using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace College.Models
{
    public static class Seed
    {
        public static void EnsurePopulated(IApplicationBuilder app)
        {
            ApplicationDbContext context = app.ApplicationServices
                .GetRequiredService<ApplicationDbContext>();
            context.Database.Migrate();
            if (!context.Courses.Any())
            {
                context.Courses.AddRange(
                    new Course
                    {
                        CourseCode = "Comp123",
                        CourseName = "Computer Program",
                        Semester = "Winter 2019",
                        CourseFee = 300.00M,
                        FacultyName = "Narendra Parshad",
                        NoOfCredit = 4,
                        CourseDesc = "Just another course for programming"
                    },
                    new Course
                    {
                        CourseCode = "Comp225",
                        CourseName = "Advance Database",
                        Semester = "Fall 2019",
                        CourseFee = 300.00M,
                        FacultyName = "Blessing",
                        NoOfCredit = 4,
                        CourseDesc = "Non-Sql and Sql databases"
                    },
                    new Course
                    {
                        CourseCode = "Eng210",
                        CourseName = "Communication01",
                        Semester = "Winter 2019",
                        CourseFee = 300.00M,
                        FacultyName = "Eden Fieldstone",
                        NoOfCredit = 4,
                        CourseDesc = "The first english course in college"
                    },
                    new Course
                    {
                        CourseCode = "Gned500",
                        CourseName = "Citizenship",
                        Semester = "Winter 2019",
                        CourseFee = 300.00M,
                        FacultyName = "Philip Alalibo",
                        NoOfCredit = 4,
                        CourseDesc = "Course about global citizenship"
                    },
                    new Course
                    {
                        CourseCode = "Comp228",
                        CourseName = "Java Programming",
                        Semester = "Fall 2019",
                        CourseFee = 300.00M,
                        FacultyName = "Wallace",
                        NoOfCredit = 4,
                        CourseDesc = "Post Secondary-Introduction to Java and Eclipse"
                    }, new Course
                    {
                        CourseCode = "Comp125",
                        CourseName = "Client-Side Web Development",
                        Semester = "Summer 2019",
                        CourseFee = 300.00M,
                        FacultyName = "Jake",
                        NoOfCredit = 4,
                        CourseDesc = "Learning JavaScript to implement on websites"
                    }, new Course
                    {
                        CourseCode = "Comp301",
                        CourseName = "Unix/Linux Operating Systems",
                        Semester = "Winter 2019",
                        CourseFee = 300.00M,
                        FacultyName = "Laxmi",
                        NoOfCredit = 4,
                        CourseDesc = "Fundamentals of Bash scripting"
                    }, new Course
                    {
                        CourseCode = "Comp200",
                        CourseName = "Scripting with Python",
                        Semester = "Summer 2019",
                        CourseFee = 300.00M,
                        FacultyName = "Elia Nika",
                        NoOfCredit = 4,
                        CourseDesc = "An introduction to coding in python"
                    }

                );
            }
            if (!context.Students.Any())
            {
                context.Students.AddRange(
                    new Student
                    {
                        UserName = "Admin",
                        FName = "John",
                        LName = "Doe",
                        Address = "123 Yong St",
                        City = "Toronto",
                        Pic = "Admin1admin.png"
                    },
                    new Student
                    {
                        UserName = "General",
                        FName = "Malik",
                        LName = "Muddasir",
                        Address = "22 King St",
                        City = "Toronto",
                        Pic = "General2Student1.png"
                    },
                    new Student
                    {
                        UserName = "Student1",
                        FName = "George",
                        LName = "Washington",
                        Address = "100 Queen St",
                        City = "Etobicoke",
                        Pic = "General2Student1.png"
                    }, new Student
                    {
                        UserName = "Student2",
                        FName = "Sara",
                        LName = "Williams",
                        Address = "33 BackStreet St",
                        City = "Oshawa",
                        Pic = "General2Student1.png"
                    }, new Student
                    {
                        UserName = "Student3",
                        FName = "Frank",
                        LName = "Einchman",
                        Address = "65 Main St",
                        City = "Toronto",
                        Pic = "General2Student1.png"
                    }
                    
                );
            }
            context.SaveChanges();
        }
    }
}  

