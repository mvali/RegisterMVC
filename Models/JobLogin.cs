using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Register.Models
{
    public class JobLogin
    {
        [Required(ErrorMessage = "Email address required")]
        [EmailAddress(ErrorMessage = "Invalid Email address")]
        public string JobEmail { get; set; }

        [Required(ErrorMessage = "Password required")]
        [StringLength(255, ErrorMessage = "Password must be between 5 and 255 characters", MinimumLength = 5)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public JobLogin()
        {
            JobEmail = "";
            Password = "";
        }
    }
}