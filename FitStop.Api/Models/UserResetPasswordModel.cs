using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FitStop.Api.Models
{
    public class UserResetPasswordModel
    {
        public string EMail { get; set; }
        public string ConfirmHash { get; set; }
        public string NewPassword { get; set; }

    }
}