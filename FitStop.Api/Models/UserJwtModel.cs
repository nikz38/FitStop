using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FitStop.Api.Models
{
    public class UserJwtModel
    {
        public int Id { get; set; }

        public string EMail { get; set; }

        public DateTime ExpirationDate { get; set; }

        public string Role { get; set; }
    }
}