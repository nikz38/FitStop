using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FitStop.Test.Automatic
{
    public static class Driver
    {

        public static IWebDriver Instance { get; set; }

        public static string BaseAddress { get; set; }

        public static void Initialize(string baseAddress)
        {
            BaseAddress = baseAddress;
            Instance = new FirefoxDriver();

            //var testProjectPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            //Instance = new ChromeDriver(testProjectPath + "/Automatic/Drivers");
            //Instance = new InternetExplorerDriver(testProjectPath + "/Automatic/Drivers");

            TurnOnWait();
        }

        public static void Close()
        {
            Instance.Close();
        }

        #region Wait methods
        public static void Wait(TimeSpan timeSpan)
        {
            Thread.Sleep((int)timeSpan.TotalMilliseconds);
        }

        public static void WaitUntil(Func<IWebDriver, bool> condition)
        {
            var wait = new WebDriverWait(Driver.Instance, TimeSpan.FromSeconds(10));
            wait.Until(condition);
        }

        public static void WaitUntilElementIsDisplayed(string cssSelector)
        {
            WaitUntil(driver => driver.FindElement(By.CssSelector(cssSelector)).Displayed);
        }

        public static void NoWait(Action action)
        {
            TurnOffWait();
            action();
            TurnOnWait();
        }

        private static void TurnOnWait()
        {
            Instance.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(5));
        }

        private static void TurnOffWait()
        {
            Instance.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(0));
        }
        #endregion

        #region Driver methods

        public static IWebElement FindElement(string cssSelector)
        {
            var element = Driver.Instance.FindElement(By.CssSelector(cssSelector));
            if (element == null)
            {
                throw new Exception("Could not find the element by given selector!");
            }
            return element;
        }

        public static ReadOnlyCollection<IWebElement> FindElements(string cssSelector)
        {
            var elements = Driver.Instance.FindElements(By.CssSelector(cssSelector));
            if (elements == null && !elements.Any())
            {
                throw new Exception("Could not find the elements by given selector!");
            }

            return elements;
        }

        public static void Click(string cssSelector)
        {
            FindElement(cssSelector).Click();
        }

        public static void TypeText(string cssSelector, object inputText, ClearText clearText = ClearText.No)
        {
            if (clearText == ClearText.Yes)
            {
                FindElement(cssSelector).Clear();
            }
            FindElement(cssSelector).SendKeys(inputText.ToString());
        }

        public static IWebElement GetLastElement(string cssSelector)
        {
            return FindElements(cssSelector).Last();
        }

        public static int GetElementCount(string cssSelector)
        {
            return FindElements(cssSelector).Count;
        }

        public static void SelectFromDropDownByName(string name, object value)
        {
            Instance.FindElement(By.Name(name)).FindElement(By.CssSelector("option[value='" + value.ToString() + "']")).Click();
        }

        public static void SelectFromDropDownById(string id, object value)
        {
            Instance.FindElement(By.Id(id)).FindElement(By.CssSelector("option[value='" + value.ToString() + "']")).Click();
        }

        #endregion
    }

    public enum ClearText
    {
        Yes, No
    }
}
