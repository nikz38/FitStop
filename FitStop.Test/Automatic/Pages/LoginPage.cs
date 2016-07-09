using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitStop.Test.Automatic.Pages
{
    public static class LoginPage
    {
        public static void GoTo()
        {
            Driver.Instance.Navigate().GoToUrl(Driver.BaseAddress + "/login");
        }

        public static void LoginAsAdmin()
        {
            string email = "maksapn@gmail.com";
            string password = "milos";

            var usernameInput = Driver.Instance.FindElement(By.Name("eMail"));
            usernameInput.SendKeys(email);

            var passwordInput = Driver.Instance.FindElement(By.Name("password"));
            passwordInput.SendKeys(password);

            var loginButton = Driver.Instance.FindElement(By.Id("login-btn"));
            loginButton.Click();

            Driver.Wait(TimeSpan.FromSeconds(3));

        }


    }

}
