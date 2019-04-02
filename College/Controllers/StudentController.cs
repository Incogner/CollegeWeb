using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using College.Models;
using College.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace College.Controllers
{
    [Authorize(Roles = "Student, Admin")]
    public class StudentController : Controller
    {
        private UserManager<IdentityUser> userManager;
        private IStudentRepository repository;
        int pageSize = 5;

        //Controller constructor
        public StudentController(UserManager<IdentityUser> userMnger, IStudentRepository repo)
        {
            userManager = userMnger;
            repository = repo;
        }
        ///////////////
        public ViewResult Profile(string userName)
        {
            Student student = repository.Students.FirstOrDefault(c => c.UserName == userName);
            return View(student);
        }
        ///////////////
        [HttpPost]
        public ViewResult Profile(Student student)
        {
            return View(student);
        }
        ///////////////
        [HttpPost]
        public RedirectToActionResult UpdateProfile(Student student)
        {
            if (ModelState.IsValid)
            {
                repository.SaveStudent(student);
                return RedirectToAction("Profile", student);
            }
            else
            {
                return RedirectToAction("/");
            }
        }
        ///////////////
        //upload a picture to profile to profile folder in images
        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            //getting user
            var user = await userManager.GetUserAsync(User);
            Student student = repository.Students.FirstOrDefault(s => s.UserName == user.UserName);
            //file check
            if (file == null || file.Length == 0) { 
                return Content("Something went wrong! try again.");
            }
            //declare path and file name based on 
            //username + student ID + original File Name to save file
            var path = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot/images/profile/",
                        student.UserName + student.StudentId+file.FileName);
            //copy the file to path
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            //save the file name in account
            repository.SavePic(student, student.UserName + student.StudentId + file.FileName);
            //return the profile page
            return RedirectToAction("Profile", student);
        }
        ///////////////
        //Upload button, opens upload picture page for account
        public ViewResult Picture(int id)
        {
            Student student = repository.Students.FirstOrDefault(s => s.StudentId == id);
            return View(student);
        }
        ///////////////
        [Authorize(Roles = "Admin")]
        public ViewResult List(int page = 1) =>
            View(new StudentListViewModel
            {
                Students = repository.Students
                    .OrderBy(c => c.StudentId)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = repository.Students.Count()
                }
            });
        ///////////////
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ViewResult ShowStudent(Student student)
        {
            return View("Student", student);
        }
    }
}
