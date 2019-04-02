using System;
using System.Collections.Generic;
using System.Linq;
using College.Models;
using College.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace College.Controllers
{
    [Authorize]
    public class CourseController : Controller
    {
        private ICourseRepository cRepo;
        private IEnrollmentRepository eRepo;
        private IStudentRepository sRepo;
        private int PageSize = 5;
        //CourseController Constructor
        public CourseController(ICourseRepository repo, IEnrollmentRepository eRepo, IStudentRepository sRepo)
        {
            this.cRepo = repo;
            this.eRepo = eRepo;
            this.sRepo = sRepo;
        }

        // ********** SEARCH FEATURE **********
        [AllowAnonymous]
        public ViewResult DisplayPage(string searchString, string searchField, int page = 1)
        {
            IEnumerable<Course> courses = cRepo.Courses
                        .OrderBy(c => c.CourseId)
                        .Skip((page - 1) * PageSize)
                        .Take(PageSize);
            
            int totalItems = cRepo.Courses.Count();

            if (!String.IsNullOrEmpty(searchString))
            {
                switch (searchField)
                {
                    case "allfields":
                        courses = cRepo.Courses
                            .Where(c => (c.CourseCode.Contains(searchString) || 
                                c.CourseDesc.Contains(searchString) || 
                                c.CourseName.Contains(searchString)))
                            .OrderBy(c => c.CourseId)
                            .Skip((page - 1) * PageSize)
                            .Take(PageSize);
                        totalItems = cRepo.Courses
                            .Where(c => (c.CourseCode.Contains(searchString) || 
                            c.CourseDesc.Contains(searchString) || 
                            c.CourseName.Contains(searchString)))
                            .Count();
                        break;
                    case "coursecode":
                        courses = cRepo.Courses
                            .Where(c => c.CourseCode.Contains(searchString))
                            .OrderBy(c => c.CourseId)
                            .Skip((page - 1) * PageSize)
                            .Take(PageSize);
                        totalItems = cRepo.Courses
                            .Where(c => c.CourseCode.Contains(searchString))
                            .Count();
                        break;
                    case "coursename":
                        courses = cRepo.Courses
                            .Where(c => c.CourseName.Contains(searchString))
                            .OrderBy(c => c.CourseId)
                            .Skip((page - 1) * PageSize)
                            .Take(PageSize);
                        totalItems = cRepo.Courses
                            .Where(c => c.CourseName.Contains(searchString))
                            .Count();
                        break;
                    case "coursedescription":
                        courses = cRepo.Courses
                            .Where(c => c.CourseDesc.Contains(searchString))
                            .OrderBy(c => c.CourseId)
                            .Skip((page - 1) * PageSize)
                            .Take(PageSize);
                        totalItems = cRepo.Courses
                            .Where(c => c.CourseDesc.Contains(searchString))
                            .Count();
                        break;
                }
            }

            return View(new CourseListViewModel
            {
                Courses = courses,
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = totalItems
                },
                SearchString = searchString,
                SearchField = searchField
            });
        }
        // ********** SEARCH FEATURE **********

        [AllowAnonymous]
        public ActionResult PDF()
        {
            string pdfContent;//represents the content of the pdf file.
            pdfContent = "\tThe list of Courses:\n\n\n"
                + "ID\tCode\t\t\t\tName\t\t\t\t\t\t\tDescription\n\n";
            foreach (Course c in cRepo.Courses)
            {
                pdfContent += c.CourseId + "\t" + c.CourseCode + "\t" + c.CourseName + "\t\t" + c.CourseDesc + "\n";
                pdfContent += "---------------------------------------------------------------------------------"
                    + "-------------------------------------------------\n";
            }
            string filePath = "~/temp/temp.pdf";
            string contentType = "application/pdf";
            if (System.IO.File.Exists(filePath))//delete last version of file
            {
                System.IO.File.Delete(filePath);
            }
            var exportFolder = "wwwroot/temp/";
            var exportFile = System.IO.Path.Combine(exportFolder, "temp.pdf");
            using (var writer = new PdfWriter(exportFile))//generate the pdf file using iText7
            {
                using (var pdf = new PdfDocument(writer))
                {
                    var doc = new Document(pdf);
                    doc.Add(new Paragraph(pdfContent));
                }
            }
            //return pdf file generated and save in temp folder
            return File(filePath, contentType, "CourseList.pdf");
        }

        [Authorize(Roles = "Admin")]
        public ViewResult InsertPage()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ViewResult InsertPage(Course course)
        {
            if (ModelState.IsValid)
            {

                cRepo.SaveCourse(course);
                return View("Confirm", course);
            }
            else
            {
                return View();
            }
        }
     
        public ViewResult DataPage(int id)
        {
            Course cc = cRepo.Courses.FirstOrDefault(c => c.CourseId == id);
            if (ModelState.IsValid)
            {
                return View(cc);
            }
            else
            {
                return View();
            }
        }
        [AllowAnonymous]
        [HttpPost]
        public ViewResult DataPage(Course course)
        {
            if (ModelState.IsValid)
            {
                return View(course);
            }
            else
            {
                return View();
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ViewResult Updated(Course course)
        {
            if (ModelState.IsValid)
            {
                cRepo.SaveCourse(course);
                return View(course);
            }
            else
            {
                return View();
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ViewResult Deleted(Course course)
        {
            if (ModelState.IsValid)
            {
                cRepo.DeleteCourse(course);
                return View(course);
            }
            else
            {
                return View();
            }
        }

   
        //This method called when logged on student wants to see enrolled courses
        [Authorize(Roles = "Student")]
        public ViewResult MyCourse(string userName, int page = 1)
        {
            //find student
            Student student = sRepo.Students.FirstOrDefault(s => s.UserName == userName);
            //create a list of courses enrolled
            List<Course> courseList = new List<Course>();
            IEnumerable<Enrollment> enr = eRepo.Enrollments
                    .Where(e => e.StudentId == student.StudentId);
            foreach(Enrollment e in enr)
            {
                Course cc = cRepo.Courses.FirstOrDefault(c => c.CourseId == e.CourseId);
                courseList.Add(cc);
            }
            //create ordered list as ViewModel source
            IEnumerable<Course> courses = courseList
                        .OrderBy(c => c.CourseId)
                        .Skip((page - 1) * PageSize)
                        .Take(PageSize);

            if (ModelState.IsValid && courseList.Count != 0)
            {
                return View(new CourseListViewModel
                {
                    Courses = courses,
                    PagingInfo = new PagingInfo
                    {
                        CurrentPage = page,
                        ItemsPerPage = PageSize,
                        TotalItems = courses.Count()
                    }
                });
            }
            else
            {
                return View();
            }

        }
    }
}
