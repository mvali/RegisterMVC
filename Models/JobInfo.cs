using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Register.Models
{
    public class JobInfo
    {
        [Required( ErrorMessage ="First Name required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage ="Last Name required")]
        public string LastName { get; set; }

        [Required(ErrorMessage ="Email address required")]
        [EmailAddress(ErrorMessage ="Invalid Email address")]
        public string JobEmail { get; set; }

        [Required(ErrorMessage ="Phone required")]
        [StringLength(30, MinimumLength=9, ErrorMessage ="Invalid phone number")]
        public string Phone { get; set; }

        [Required(ErrorMessage ="Password required")]
        [StringLength(255, ErrorMessage = "Password must be between 5 and 255 characters", MinimumLength = 5)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [StringLength(255, ErrorMessage = "Must be between 5 and 255 characters", MinimumLength = 5)]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage ="Password and Confirmed Password do not match")]
        public string Password2 { get; set; }

        [Range(typeof(bool),"true", "true", ErrorMessage ="In order to continue you must agree our terms and conditions!")]
        public bool TermsConditionsAgree { get; set; }

        public string JobEmailLogin { get; set; }
        public string PasswordLogin { get; set; }

        public JobInfo() {
            FirstName = "";
            LastName = "";
            JobEmail = "";
            Phone = "";
        }
    }
}