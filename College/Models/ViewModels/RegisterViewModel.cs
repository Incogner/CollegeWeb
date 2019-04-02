using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace College.Models.ViewModels
{
    public class RegisterViewModel
    {
        
        [Required, MaxLength(256)]
        public string Username { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password), Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
        //stududent info
        [Required(ErrorMessage = "Please enter first name")]
        [StringLength(15)]
        public string FName { get; set; }
        [Required(ErrorMessage = "Please enter last name")]
        [StringLength(15)]
        public string LName { get; set; }
    }
}

