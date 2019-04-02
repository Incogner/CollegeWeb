using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using College.Models;
using College.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace College.Controllers
{
    [Authorize(Roles = "Student")]
    public class CartController : Controller
    {
        private IEnrollmentRepository enrollRepo;
        private ICourseRepository cRepo;
        private IStudentRepository stuRepo;
        private Cart cart;
        private UserManager<IdentityUser> userManager;
        
        //CartController Constructor
        public CartController(IEnrollmentRepository eRepo,ICourseRepository cRepo, Cart cartService,
            UserManager<IdentityUser> userManager, IStudentRepository stuRepo)
        {
            enrollRepo = eRepo;
            this.stuRepo = stuRepo;
            this.cRepo = cRepo;
            cart = cartService;
            this.userManager = userManager;
        }

        public ViewResult Index(string returnUrl)
        {
            return View(new CartViewModel
            {
                Cart = cart,
                ReturnUrl = returnUrl
            });
        }
        //open Enrolled page for already enrolled courses
        public ViewResult Enrolled(string returnUrl)
        {
            return View(new CartViewModel
            {
                Cart = cart,
                ReturnUrl = returnUrl
            });
        }

        //adding a course item to the cart
        public async Task<RedirectToActionResult> AddCourse(int courseId, string returnUrl)
        {
            //find student
            var user = await userManager.GetUserAsync(User);
            Student student = stuRepo.Students.FirstOrDefault(s => s.UserName == user.UserName);
            //check if student already enrolled is course
            IEnumerable<Enrollment> enrolls = enrollRepo.Enrollments
                    .Where(e => e.StudentId == student.StudentId && e.CourseId == courseId);
            if(enrolls.Count() > 0)
            {
                return RedirectToAction("Enrolled", new { returnUrl });
            }
            //find course and add to cart
            Course course = cRepo.Courses
                .FirstOrDefault(c => c.CourseId == courseId);
            if (course != null)
            {
               cart.AddCourse(course);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        //remove course item from the cart
        public RedirectToActionResult RemoveFromCart(int courseId, string returnUrl)
        {
            Course course = cRepo.Courses
                .FirstOrDefault(c => c.CourseId == courseId);

            if (course != null)
            {
                cart.RemoveCourse(course);

            }
            return RedirectToAction("Index", new { returnUrl });
        }
        //Enrolling a student to one or many courses
        public async Task<RedirectToActionResult> CheckoutAsync()
        {
            var user = await userManager.GetUserAsync(User);
            Student student = stuRepo.Students.FirstOrDefault(s => s.UserName == user.UserName);
            if(cart.CartLines.Count() != 0)
            {
                foreach (CartLine c in cart.CartLines)
                {
                    if(c != null)
                    {
                        Enrollment aEnrollment = new Enrollment()
                        {
                            CourseId = c.Course.CourseId,
                            StudentId = student.StudentId
                        };
                        enrollRepo.SaveEnrollment(aEnrollment);
                        
                    }
                };
                cart.Clear();
            }
            return RedirectToAction("DisplayPage", "Course");
        }
    }
}
