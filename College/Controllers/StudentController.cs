using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using College.Models;
using College.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace College.Controllers
{
    [Authorize(Roles = "Student, Admin")]
    public class StudentController : Controller
    {
        private UserManager<IdentityUser> userManager;
        private IStudentRepository repository;
        int pageSize = 5;
        private IHostingEnvironment _env;

        //Controller constructor
        public StudentController(UserManager<IdentityUser> userMnger, IStudentRepository repo, IHostingEnvironment env)
        {
            userManager = userMnger;
            repository = repo;
            _env = env;
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
            //file check
            if (file == null || file.Length == 0)
            {
                return Content("<p>No file uploaded, please try again.</p>");
            }else if (file.Length > 2000000)
            {
                return Content("<p>Your image is too large, please try images less than 2 MB.</p>");
            }

            //getting user
            var user = await userManager.GetUserAsync(User);
            Student student = repository.Students.FirstOrDefault(s => s.UserName == user.UserName);
            
            //declare path and file name based on 
            //username + student ID + original File Name to save file
            var webRoot = _env.WebRootPath;
            var path = Path.Combine(webRoot, "images/profile/" + student.UserName + student.StudentId + file.FileName);
            byte[] imb = null;
            //copy the file to path
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            imb = System.IO.File.ReadAllBytes(path);
            
            string base64String = Convert.ToBase64String(imb);
            string fileName = student.UserName + student.StudentId + file.FileName;
            string myParameters = $"image={base64String}";

            HttpClient client = new HttpClient();
            var values = new Dictionary<string, string>
                {
                    { "key", "2f98329e7d086fa631ba627c2728a284" },
                    { "image", base64String },
                    { "name", fileName}
                };
            //send data and recieve json
            var content = new FormUrlEncodedContent(values);
            var response = await client.PostAsync("https://api.imgbb.com/1/upload", content);
            var responseString = await response.Content.ReadAsStringAsync();
            JsonFile json = JsonConvert.DeserializeObject<JsonFile>(responseString);
            ImageBB imagebb = json.Data;

            if (json.Success)
            {
                //save the file name in account
                repository.SavePic(student, imagebb.Display_Url);
                //repository.SavePic(student, student.UserName + student.StudentId + file.FileName);
            }
            else
            {
                return Content($"<p>{json.Status}: Image can't upload, please try again.</p>");
            }

            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }

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
        ///
        
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
