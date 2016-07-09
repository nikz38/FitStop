using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FitStop.Api.Models
{
    public class UserRegisterModel
    {
        [Required(ErrorMessage = "Password is required!")]
        public string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        [Required(ErrorMessage = "E-mail is required!")]
        public string EMail { get; set; }

        public string PhoneNumber { get; set; }
    }
}