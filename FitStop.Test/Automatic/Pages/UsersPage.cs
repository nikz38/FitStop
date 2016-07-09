using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitStop.Test.Automatic.Pages
{
    public static class UsersPage
    {
        public static void GoTo()
        {
            Driver.Instance.Navigate().GoToUrl(Driver.BaseAddress + "/users");
        }

        public static bool IsAt
        {
            get
            {
                var pageTitle = Driver.Instance.FindElement(By.CssSelector("li[ui-sref-active].active a"));
                return pageTitle != null && pageTitle.Text == "Users";
            }
        }

    }

}
