using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FitStop.Api.Models
{
    public class UserLoginModel
    {
        [Required(ErrorMessage = "E-mail is required!")]
        public string EMail { get; set; }

        [Required(ErrorMessage = "Password is required!")]
        public string Password { get; set; }
    }
}